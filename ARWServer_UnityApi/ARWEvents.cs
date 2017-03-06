using System;
using System.Collections.Generic;

namespace ARWServer_UnityApi
{
	public class ARWEvents
	{
		public static List<ARWEvent> allEvents;

		public static ARWEvent CONNECTION;
		public static ARWEvent CONNECTION_LOST;
		public static ARWEvent EXTENSION_RESPONCE;
		public static ARWEvent LOGIN;
		public static ARWEvent LOGIN_ERROR;
		public static ARWEvent LOGOUT;
		public static ARWEvent ROOM_JOIN;
		public static ARWEvent ROOM_JOIN_ERROR;
		public static ARWEvent USER_ENTER_ROOM;
		public static ARWEvent USER_EXIT_ROOM;


		public static void Init(){
			allEvents = new List<ARWEvent> ();

			CONNECTION 			= new ARWEvent(ARWServer_CMD.Connection_Success);
			CONNECTION_LOST 	= new ARWEvent ();
			EXTENSION_RESPONCE 	= new ARWEvent ();
			LOGIN 				= new ARWEvent (ARWServer_CMD.Login);
			LOGIN_ERROR 		= new ARWEvent ();
			LOGOUT 				= new ARWEvent ();
			ROOM_JOIN 			= new ARWEvent (ARWServer_CMD.Join_Room);
			ROOM_JOIN_ERROR 	= new ARWEvent ();
			USER_ENTER_ROOM 	= new ARWEvent (ARWServer_CMD.User_Enter_Room);
			USER_EXIT_ROOM 		= new ARWEvent (ARWServer_CMD.User_Exit_Room);

			allEvents.Add (CONNECTION);
			allEvents.Add (LOGIN);
			allEvents.Add (ROOM_JOIN);
			allEvents.Add (USER_ENTER_ROOM);
			allEvents.Add (USER_EXIT_ROOM);
		}
	}
}

