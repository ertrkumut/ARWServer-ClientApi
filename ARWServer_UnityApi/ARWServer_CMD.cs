using System;

namespace ARWServer_UnityApi
{
	public static class ARWServer_CMD
	{
		public static string Connection_Success = "CONNECTIONSUCCESS_EVENT";
		public static string Connection_Error 	= "CONNECTION_ERROR";
		public static string Connection_Lost 	= "CONNECTION_LOST";
		public static string Disconnection 		= "DISCONNECTION";
		public static string Login              = "LOGIN_EVENT";
		public static string Join_Room          = "ROOM_JOIN";
		public static string Any_Join_Room      = "ANY_ROOM_JOIN";
		public static string User_Enter_Room    = "USER_ENTER_ROOM";
		public static string User_Exit_Room     = "USER_EXIT_ROOM";
		public static string Room_Create		= "ROOM_CREATE";
	}
}

