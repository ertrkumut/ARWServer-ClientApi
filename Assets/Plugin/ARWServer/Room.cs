﻿using System;
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
				string[] userArray = e.GetString("Users").Split(new string[] {"''"}, StringSplitOptions.None);
				for(int ii = 0; ii< userArray.Length; ii++){
					string[] userDataParts = userArray[ii].Split(new string[]{"^^"}, StringSplitOptions.None);
					User u = new User(userDataParts[0], int.Parse(userDataParts[1]), bool.Parse(userDataParts[2]));
				}
			}catch(System.NullReferenceException){
			}catch(System.IndexOutOfRangeException){}
		}
	}
}

