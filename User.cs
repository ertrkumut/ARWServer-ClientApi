using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ARWServer_UnityApi
{
	public class User
	{

		public string name;
		public int id;
		public bool isMe;

		public Room lastJoinedRoom;
		private List<UserVariable> userVariables;

		public User(){
		}
		
		public User (SpecialEventParam e){
			this.name = e.GetString ("userName");
			this.id = e.GetInt ("userId");
			this.isMe = bool.Parse (e.GetString ("isMe"));
			this.lastJoinedRoom = null;

			string userVariables = e.GetString("userVariables");
			string[] userVars = userVariables.Split('§');

			foreach(string userVar in userVars){
				string[] variableParts = userVar.Split('½');
				this.userVariables.Add(new UserVariable(variableParts[0], variableParts[1]));
			}

			UserManager.allUserInGame.Add (this);
		}

		public UserVariable GetUserVariables(string key){
			UserVariable currentVariable = this.userVariables.Where(a=>a.key == key).FirstOrDefault();
			return currentVariable;
		}
	}
}

