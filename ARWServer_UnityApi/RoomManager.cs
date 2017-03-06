using System;
using System.Collections.Generic;

namespace ARWServer_UnityApi
{
	public class RoomManager
	{
		private static List<Room> _allRooms;
		public static List<Room> allRooms{
			get{ 
				if (_allRooms == null)
					_allRooms = new List<Room> ();

				return _allRooms;
			}
		}


	}
}

