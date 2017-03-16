using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Linq;

namespace ARWServer_UnityApi
{

	public class ARWServer{

		public static List<Room> allRooms;

		public string host;
		public int tcpPort;
		public int udpPort;

		public bool isConnected;

		private TcpClient client;
		private NetworkStream ns{
			get{
				if (client != null)
					return client.GetStream ();
				return null;
			}
		}

		private TcpListener tcpListener;

		public User me;

		public void Connect(){
			client = new TcpClient();
			try{
				client.Connect(host, tcpPort);
				ARWObject newObj = new ARWObject ();
				newObj.SetRequestName (ARWServer_CMD.Connection_Success);
				SendReqeust (newObj);

			}catch(System.Net.Sockets.SocketException e){
				ARWObject obj = new ARWObject ();
				obj.PutString ("error", e.Message);
				ARWEvents.CONNECTION.p_handler (this, obj);
				return;
			}
		}

		public void Init(){
			UserManager.allUserInGame = new List<User> ();
			allRooms = new List<Room> ();
			ARWEvents.Init ();
		}

		public void ProcessEvents(){
			tcpListener = new TcpListener (IPAddress.Any, this.tcpPort);

			while (true) {
				try{

					if(this.client.Client.Poll(1, SelectMode.SelectRead) && !this.ns.DataAvailable){
						ARWEvents.DISCONNECTION.p_handler(this, new ARWObject());
					}
						

					byte[] readBytes = new byte[1024];
					this.ns.Read(readBytes,0,readBytes.Length);
					var message = System.Text.Encoding.UTF8.GetString(readBytes).Replace("\0", null);

					ARWObject newObj = ARWObject.Extract(readBytes);
					ARWEvent currentEvent = ARWEvents.allEvents.Where(a=>a.eventName == newObj.GetRequestName()).FirstOrDefault();

					if(currentEvent != null){
						if(currentEvent.p_handler != null){
							currentEvent.p_handler(this, newObj);
						}else{
							if(currentEvent.handler!=null)
								currentEvent.handler(newObj);
						}
					}
				
				}catch(System.ObjectDisposedException e){

				}catch(System.IO.IOException a){

				}catch(System.OutOfMemoryException a){
					
				}
			}
		}

		public void SendJoin_AnyRoomRequest(string roomTag, ARWObject arwObj){
			if (arwObj == null)
				arwObj = new ARWObject ();

			arwObj.SetRequestName (ARWServer_CMD.Any_Join_Room);
			arwObj.eventParams.PutVariable ("roomTag", roomTag);

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

		public void Disconnection(){
			this.client.Close ();
			if (ARWEvents.DISCONNECTION.handler != null)
				ARWEvents.DISCONNECTION.handler (new ARWObject ());
		}
	}
}
