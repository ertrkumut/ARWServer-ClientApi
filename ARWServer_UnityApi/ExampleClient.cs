using System;

namespace ARWServer_UnityApi
{
	public class ExampleClient
	{
		public static ARWServer arwServer;

		public static void Main(){

			arwServer = new ARWServer();

			Console.WriteLine("================================");
			arwServer.host = "127.0.0.1";
			arwServer.tcpPort = 8081;

			arwServer.Init ();

			arwServer.AddEventHandler (ARWEvents.CONNECTION, Connection);
			arwServer.AddEventHandler (ARWEvents.LOGIN, LoginHandler);
			arwServer.AddEventHandler (ARWEvents.ROOM_JOIN, RoomJoinHandler);
			arwServer.AddEventHandler (ARWEvents.LOGIN_ERROR, Login_Error);
			arwServer.Connect();

			arwServer.ProcessEvents ();
		}

		public static void Connection(ARWObject evntObj){
			if (evntObj.GetString ("error") == "") {
				Console.WriteLine ("ConnectionSuccess");
				arwServer.SendLoginRequest ("deniz", null);
				return;
			}

			Console.WriteLine(evntObj.GetString("error"));
		}

		public static void LoginHandler(ARWObject evntObj){
			User loginedUser = evntObj.GetUser ();

			Console.WriteLine("Login Success : Name = " + loginedUser.name + " - Id = " + loginedUser.id + "- IsMe : " + loginedUser.isMe);
			arwServer.SendJoin_AnyRoomRequest ("Game_Room", null);
		}

		public static void Login_Error(ARWObject obj){
			Console.WriteLine("==> Login Error : " + obj.GetString("error"));
		}


		public static void RoomJoinHandler(ARWObject evntObj){
			Room currentRoom = evntObj.GetRoom ();
			Console.WriteLine("Room Join Success : " + currentRoom.name + " : " + currentRoom.tag + " : " + currentRoom.userList.Length);
		}
	}
}

