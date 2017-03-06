using System;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Linq;

#pragma warning disable 0168 // variable declared but not used.
#pragma warning disable 0219 // variable assigned but not used.
#pragma warning disable 0414 // private field assigned but not used.

namespace ARWServer_UnityApi
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

		private TcpListener tcpListener;

		public void Connect(){
			client = new TcpClient();
			try{
				client.Connect(host, tcpPort);
				ARWObject newObj = new ARWObject ();
				newObj.SetRequestName (ARWServer_CMD.Connection_Success);
				SendReqeust (newObj);

			}catch(System.Net.Sockets.SocketException e){
				Console.WriteLine (e);
				return;
			}
		}

		public void Init(){
			ARWEvents.Init ();
		}

		public void ProcessEvents(){
//			IPAddress ipAddrss = IPAddress.Parse (this.host);
//			tcpListener = new TcpListener (ipAddrss, this.tcpPort);
			tcpListener = new TcpListener (IPAddress.Any, this.tcpPort);
//			tcpListener.Start ();

			while (true) {
				try{
					byte[] readBytes = new byte[1024];
					this.ns.Read(readBytes,0,readBytes.Length);
					var message = System.Text.Encoding.UTF8.GetString(readBytes).Replace("\0", null);
					ARWObject newObj = ARWObject.Extract(readBytes);
					ARWEvent currentEvent = ARWEvents.allEvents.Where(a=>a.eventName == newObj.GetRequestName()).FirstOrDefault();
					currentEvent.handler(newObj);

				}catch(System.ObjectDisposedException e){

				}catch(System.IO.IOException a){

				}catch(System.NullReferenceException a){
					Console.WriteLine("Event Not Found !!!");
				}catch(System.OutOfMemoryException a){
					
				}
			}
		}

		public void SendJoin_AnyRoomRequest(string roomTag, ARWObject arwObj){
			if (arwObj == null)
				arwObj = new ARWObject ();

			arwObj.SetRequestName (ARWServer_CMD.Any_Join_Room);
			arwObj.eventParams.PutVariable ("RoomTag", roomTag);

			SendReqeust (arwObj);
		}

		public void SendLoginRequest(string userName, ARWObject arwObject){
			ThreadStart threadFunc = new ThreadStart(delegate() {
				ARWObject loginObj = arwObject;
				if (loginObj == null)
					loginObj = new ARWObject ();

				loginObj.SetRequestName (ARWServer_CMD.Login);
				loginObj.eventParams.PutVariable ("userName", userName);

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
		}

		public void AddEventHandler(ARWEvent evnt, EventHandler handler){
			evnt.handler += handler;
		}

		#region PrivateHandlers

		#endregion
	}
}
