using System;
using System.Collections.Generic;
using UnityEngine;

namespace ARWServer_UnityApi
{
	public class Room
	{
		public string tag;
		public string name;
		private User[] userList;

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

		public User[] GetUserList(){
			int count = 0;

			for(int ii = 0; ii< this.userList.Length; ii++){
				if(this.userList[ii] != null)
					count++;
			}

			User[] tempArray = new User[count];			int tempCount = 0;
			for(int ii = 0; ii< this.userList.Length; ii++){
				if(this.userList[ii] != null){
					tempArray[tempCount] = this.userList[ii];
					tempCount++;
				}
			}
			return tempArray;
		}

		public int GetUserCount(){
			return this.GetUserList().Length;
		}

		public void AddUser(User u){
			Debug.Log(this.userList.Length + " : " + this.GetUserCount());
			this.userList[this.GetUserCount()] = u;
		}
	}
}

