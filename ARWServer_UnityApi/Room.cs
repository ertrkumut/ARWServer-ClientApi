using System;
using System.Collections.Generic;

namespace ARWServer_UnityApi
{
	public class Room
	{
		public string tag;
		public string name;
		public User[] userList;

		public Room(SpecialEventParam e){
			try{
				this.name = e.GetString ("RoomName");
				this.tag = e.GetString ("RoomTag");

				userList = new User[e.GetInt ("RoomCappacity")];
			}catch(System.NullReferenceException){

			}
		}

	}
}

