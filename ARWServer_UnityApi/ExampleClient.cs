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

			arwServer.AddEventHandler (ARWEvents.CONNECTION, ConnectionSuccess);
			arwServer.AddEventHandler (ARWEvents.LOGIN, LoginHandler);
			arwServer.AddEventHandler (ARWEvents.ROOM_JOIN, RoomJoinHandler);
			arwServer.Connect();

			arwServer.ProcessEvents ();
		}

		public static void ConnectionSuccess(ARWObject evntObj){
			Console.WriteLine ("Connection Success");
			arwServer.SendLoginRequest ("deniz", null);
		}

		public static void LoginHandler(ARWObject evntObj){
			User loginedUser = new User (evntObj.eventParams);

			Console.WriteLine("Login Success : Name = " + loginedUser.name + " - Id = " + loginedUser.id + "- IsMe : " + loginedUser.isMe);
			arwServer.SendJoin_AnyRoomRequest ("any", null);
		}

		public static void RoomJoinHandler(ARWObject evntObj){
//			Room currentRoom = new Room (evntObj.eventParams);

			Console.WriteLine("Room Join Success : " + evntObj.eventParams.GetString("roomName"));
		}
	}
}

