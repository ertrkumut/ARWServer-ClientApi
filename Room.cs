using System;
using System.Collections.Generic;
using UnityEngine;

namespace ARWServer_UnityApi
{
	public class Room
	{
		public string tag;
		public string name;
		public int id;

		private List<User> userList;

		public Room(SpecialEventParam e){
			try{
				this.name = e.GetString ("RoomName");
				this.tag = e.GetString ("RoomTag");
				this.id = e.GetInt("RoomId");
				
				userList = new List<User>();
				string[] userArray = e.GetString("Users").Split(new string[] {"''"}, StringSplitOptions.None);

				for(int ii = 0; ii< userArray.Length; ii++){
					string[] userDataParts = userArray[ii].Split(new string[]{"^^"}, StringSplitOptions.None);
					User u = new User(userDataParts[0], int.Parse(userDataParts[1]), bool.Parse(userDataParts[2]));
					userList.Add(u);
				}
			}catch(System.NullReferenceException){
			}catch(System.IndexOutOfRangeException){}
		}

		public User[] GetUserList(){

			User[] tempArray = new User[this.userList.Count];
			for(int ii = 0; ii< this.userList.Count; ii++){
				tempArray[ii] = this.userList[ii];
			}
			return tempArray;
		}

		public int GetUserCount(){
			return this.userList.Count;
		}

		public void AddUser(User u){
			// Debug.Log(this.userList.Length + " : " + this.GetUserCount());
			u.lastJoinedRoom = this;
			this.userList.Add(u);
		}
	}
}

