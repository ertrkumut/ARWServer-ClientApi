using System;

namespace ARWServer
{
	public class ARWEvents
	{
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
			CONNECTION 			= new ARWEvent();
			CONNECTION_LOST 	= new ARWEvent ();
			EXTENSION_RESPONCE 	= new ARWEvent ();
			LOGIN 				= new ARWEvent ();
			LOGIN_ERROR 		= new ARWEvent ();
			LOGOUT 				= new ARWEvent ();
			ROOM_JOIN 			= new ARWEvent ();
			ROOM_JOIN_ERROR 	= new ARWEvent ();
			USER_ENTER_ROOM 	= new ARWEvent ();
			USER_EXIT_ROOM 		= new ARWEvent ();
		}
	}
}

