using System;
using System.Net.Sockets;
using System.Net;
using System.Threading;

#pragma warning disable 0168 // variable declared but not used.
#pragma warning disable 0219 // variable assigned but not used.
#pragma warning disable 0414 // private field assigned but not used.

namespace ARWServer
{

	public class ARWServer{

		public string host;
		public int tcpPort;
		public int udpPort;

		public bool isConnected{
			get{ 
				if (this.client == null)
					return false;

				return this.client.Connected;
			}
		}

		private TcpClient client;
		private NetworkStream ns{
			get{
				if (client != null)
					return client.GetStream ();
				return null;
			}
		}

		private TcpListener listener;

		public void Connect(){
			client = new TcpClient();
			try{
				client.Connect(host, tcpPort);

			}catch(System.Net.Sockets.SocketException e){
				Console.WriteLine (e);
				return;
			}

			ARWObject newObj = new ARWObject ();
			newObj.SetRequestName ("ConnectionSuccess");
			SendReqeust (newObj);
		}

		public void Init(){
			ARWEvents.Init ();
		}

		public void ProcessEvents(){
			IPAddress ipAddrss = IPAddress.Parse (this.host);
			listener = new TcpListener (ipAddrss, this.tcpPort);

			listener.Start ();

			while (true) {
				try{
					ThreadStart threadFunc = new ThreadStart (delegate() {
						try{
							byte[] readBytes = new byte[this.client.ReceiveBufferSize];
							this.ns.Read(readBytes,0,readBytes.Length);
							var message = System.Text.Encoding.UTF8.GetString(readBytes).Replace("\0", null);
							ARWObject newObj = ARWObject.Extract(readBytes);

							if(newObj.GetRequestName() == "ConnectionSuccess"){
								if(ARWEvents.CONNECTION.handler == null)
									return;
								
								ARWEvents.CONNECTION.handler(newObj);
							}
						}catch(System.ObjectDisposedException e){
								
						}catch(System.IO.IOException a){
							
						}
					});
					Thread childSocketThreat = new Thread (threadFunc);
					childSocketThreat.Start ();
				}catch(System.ObjectDisposedException){
					Console.WriteLine ("Object Disposed Exception");
				}
			}
		}

		public void SendLoginRequest(string userName, ARWObject arwObject){
			ThreadStart threadFunc = new ThreadStart(delegate() {
				ARWObject loginObj = arwObject;
				if (loginObj == null)
					loginObj = new ARWObject ();

				loginObj.SetRequestName ("LoginEvent");
				loginObj.specialParam.PutVariable ("userName", "umut");

				SendReqeust(loginObj);
			});
			Thread loginThread = new Thread (threadFunc);
			loginThread.Start ();
		}

		public void SendReqeust(ARWObject arwObject){
			if (arwObject == null)
				return;

			byte[] bytesToSend = arwObject.Compress ();
			client.Client.Send (bytesToSend);
			Console.WriteLine (arwObject.GetRequestName() + " Send");
		}

		public void AddEventHandler(ARWEvent evnt, EventHandler handler){
			evnt.handler += handler;
		}

		#region PrivateHandlers

		#endregion
	}
}
