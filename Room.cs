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

					SpecialEventParam userParams = new SpecialEventParam();
					userParams.PutVariable("userName", userDataParts[0]);
					userParams.PutVariable("userId", userDataParts[1]);
					userParams.PutVariable("isMe", false);
					userParams.PutVariable("userVariables", userDataParts[2]);

					User u = new User(userParams);
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

