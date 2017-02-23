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

		TcpClient client;
		NetworkStream ns{
			get{
				if (client != null)
					return client.GetStream ();
				return null;
			}
		}

		TcpListener listener;

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

		public void ProcessEvents(){
			IPAddress ipAddrss = IPAddress.Parse (this.host);
			listener = new TcpListener (ipAddrss, this.tcpPort);

			listener.Start ();

			while (true) {
				try{
					ThreadStart threadFunc = new ThreadStart (delegate() {
						byte[] readBytes = new byte[this.client.ReceiveBufferSize];
						this.ns.Read(readBytes,0,readBytes.Length);
						var message = System.Text.Encoding.UTF8.GetString(readBytes).Replace("\0", null);
						ARWObject newObj = ARWObject.ExtractARWObject(readBytes);

						if(newObj.GetRequestName() == "ConnectionSuccess"){
							ARWEvents.CONNECTION.handler(newObj);
						}
					});
					Thread childSocketThreat = new Thread (threadFunc);
					childSocketThreat.Start ();
				}catch(System.ObjectDisposedException){
				
				}
			}
		}

		public void SendReqeust(ARWObject arwObject){
			if (arwObject == null)
				return;

			byte[] bytesToSend = arwObject.CompressARWObject ();
			client.Client.Send (bytesToSend);
		}

		public void SendLoginRequest(string username, string password, ARWObject arwObject = null){
			if (arwObject == null)
				arwObject = new ARWObject ();
		}

		public void AddEventHandler(ARWEvent evnt, EventHandler handler){
			evnt.handler += handler;
		}

		#region PrivateHandlers

		#endregion
	}

	class Program{

		public static ARWServer arwServer;

		public static void Main(){
			arwServer = new ARWServer();
			ARWEvents.Init ();

			Console.WriteLine("================================");
			arwServer.host = "127.0.0.1";
			arwServer.tcpPort = 8081;

			arwServer.AddEventHandler (ARWEvents.CONNECTION, Deneme);
			arwServer.Connect();

			arwServer.ProcessEvents ();
		}

		public static void Deneme(ARWObject evntObj){
			Console.WriteLine ("Connection Success");
		}
	}
}
