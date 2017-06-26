using System;
using System.Linq;
using UnityEngine;

namespace ARWServer_UnityApi
{
	public class PrivateEventHandlers
	{
		public void P_Connection(ARWServer server, ARWObject obj){
			if (obj.GetString ("error") == "")
				server.isConnected = true;

			if (ARWEvents.CONNECTION.handler != null)
				ARWEvents.CONNECTION.handler (obj);
		}

		public void P_Disconnection(ARWServer server, ARWObject obj){
			server.isConnected = false;
			if (ARWEvents.DISCONNECTION.handler != null)
				ARWEvents.DISCONNECTION.handler (obj);
		}

		public void P_Connection_Lost(ARWServer server, ARWObject obj){
			server.isConnected = false;
			if (ARWEvents.CONNECTION_LOST.handler != null)
				ARWEvents.CONNECTION_LOST.handler (obj);
		}

		public void P_Login(ARWServer server, ARWObject obj){
			User me = new User (obj.eventParams);
			server.me = me;

			if(ARWEvents.LOGIN.handler != null)
				ARWEvents.LOGIN.handler (obj);
		}

		public void P_Room_Create(ARWServer server, ARWObject obj){
			Room newRoom = new Room(obj.eventParams);
			ARWServer.allRooms.Add (newRoom);

			Console.WriteLine ("Room Create : " + newRoom.name + " : " + newRoom.tag + " : " + newRoom.GetUserList().Length);
		}

		public void P_Join_Room(ARWServer server, ARWObject obj){
			P_Room_Create(server, obj);

			Room currentRoom = obj.GetRoom();
			User currentUser = server.me;
			server.me.lastJoinedRoom = currentRoom;

			currentRoom.AddUser(currentUser);

			if (ARWEvents.ROOM_JOIN.handler != null) {
				ARWEvents.ROOM_JOIN.handler (obj);
			}
		}

		public void P_Extension_Handler(ARWServer server, ARWObject obj){
			string cmd = obj.eventParams.GetString("cmd");

			ExtensionRequest req = ARWEvents.extensionRequests.Where(a=>a.cmd == cmd).FirstOrDefault();
		
			if(req != null){
				req.handler(obj);
			}
		}
	}
}

