using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Linq;
using UnityEngine;

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

		private string wrongData = string.Empty;

		private DateTime _serverTime;
		public DateTime serverTime{
			set{	_serverTime = value; }
			get{
				TimeSpan different = DateTime.Now - firstDateTime;
				return _serverTime.Add(different);
			}
		}
		private DateTime firstDateTime;

		public void Connect(){
			client = new TcpClient();
			try{
				this.firstDateTime = DateTime.Now;
				client.Connect(host, tcpPort);
				ARWObject newObj = new ARWObject ();
				newObj.SetRequestName (ARWServer_CMD.Connection_Success);
				
				Thread t = new Thread(() => SendReqeust(newObj));
				t.Start();
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

			if(this.client == null)
				return;

			try{
				if(this.client.Client.Poll(1, SelectMode.SelectRead) && !this.ns.DataAvailable){
					ARWEvents.DISCONNECTION.p_handler(this, new ARWObject());
				}
					
				byte[] readBytes = new byte[4096];
				if(this.ns != null && this.ns.DataAvailable){
					this.ns.Read(readBytes, 0, readBytes.Length);
				}
				else
					return;
				
				ParseRequestData(readBytes);
			
			}catch(System.ObjectDisposedException e){
			}catch(System.IO.IOException a){
			}catch(System.OutOfMemoryException a){
			}
		}

		private void ParseRequestData(byte[] readBytes){
			var message = System.Text.Encoding.UTF8.GetString(readBytes).Replace("\0", null);
			string[] requestParts = message.Split('|');
			for(int ii = 0; ii< requestParts.Length; ii++){
				string part = requestParts[ii];
				if(part != ""){
					// Thread t = new Thread(() => HandleRequest(part));
					// t.Start();
					HandleRequest(part);
				}
			}
		}

		private void HandleRequest(string data){

			ARWObject newObj = ARWObject.Extract(data);
			ARWEvent currentEvent = ARWEvents.allEvents.Where(a=>a.eventName == newObj.GetRequestName()).FirstOrDefault();

			if(currentEvent != null){
				if(currentEvent.p_handler != null){
					currentEvent.p_handler(this, newObj);
				}else{
					if(currentEvent.handler!=null){
						currentEvent.handler(newObj);
					}
				}
			}else{
				wrongData += data;

				if(ARWObject.CanBeARWObject(wrongData)){
					wrongData += data;
					HandleRequest(wrongData);
					wrongData = "";
					return;
				}
			}
		}

		public void SendLoginRequest(string userName, ARWObject arwObject){
			ThreadStart threadFunc = new ThreadStart(delegate() {
				ARWObject loginObj = arwObject;
				if (loginObj == null)
					loginObj = new ARWObject ();

				loginObj.SetRequestName (ARWServer_CMD.Login);
				loginObj.eventParams.PutVariable ("userName", userName);

				Thread t = new Thread(() => SendReqeust(loginObj));
				t.Start();
			});
			Thread loginThread = new Thread (threadFunc);
			loginThread.Start ();
		}

		public void SendExtensionRequest(string cmd, ARWObject arwObj, bool room = false, bool isTcp = true){
			
			if(arwObj == null)
				arwObj = new ARWObject();
				
			arwObj.eventParams.PutVariable("cmd", cmd);
			arwObj.eventParams.PutVariable("isRoomRequest", room);

			if(room){
				arwObj.eventParams.PutVariable("roomId", me.lastJoinedRoom.id);
			}
			
			arwObj.SetRequestName(ARWServer_CMD.Extension_Request);

			Thread t = new Thread(() => SendReqeust(arwObj));
			t.Start();
		}



		public void AddExtensionRequest(string cmd, EventHandler handler){

			ExtensionRequest isEventExist = ARWEvents.extensionRequests.Where(a=>a.cmd == cmd).FirstOrDefault();
			if(isEventExist != null)
				return;

			ExtensionRequest newEvent = new ExtensionRequest();
			newEvent.cmd = cmd;
			newEvent.handler = handler;

			ARWEvents.extensionRequests.Add(newEvent);
		}

		private void SendReqeust(ARWObject arwObject){
			if (arwObject == null)
				return;

			byte[] bytesToSend = arwObject.Compress ();
			client.Client.Send (bytesToSend);
		}

		public void AddEventHandler(ARWEvent evnt, EventHandler handler){
			evnt.handler += handler;
		}

		public void Disconnect(){
			this.client.Close ();
			if (ARWEvents.DISCONNECTION.handler != null)
				ARWEvents.DISCONNECTION.handler (new ARWObject ());
		}

		public void SetServerTime(DateTime firstDateTime){
			TimeSpan requestDelay = DateTime.Now - this.firstDateTime;
			Debug.Log("Request Delay : " + requestDelay.Seconds + " : " + requestDelay.Milliseconds);
			firstDateTime.AddMilliseconds(requestDelay.Milliseconds);
			this.firstDateTime = firstDateTime;
		}
	}
}
