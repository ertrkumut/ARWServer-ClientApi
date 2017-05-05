using System;
using UnityEngine;

namespace ARWServer_UnityApi
{
	public class User
	{

		public string name;
		public int id;
		public bool isMe;

		public Room lastJoinedRoom;

		public GameObject character;

		public User(){
		}
		
		public User (SpecialEventParam e){
			this.name = e.GetString ("userName");
			this.id = e.GetInt ("userId");
			this.isMe = bool.Parse (e.GetString ("isMe"));
			this.lastJoinedRoom = null;

			UserManager.allUserInGame.Add (this);
		}

		public User(string name, int id, bool isMe){
			this.name = name;
			this.id = id;
			this.isMe = isMe;
		}

		public static void SpawnUser(Vector3 pos, User user){
			GameObject userGameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("PlayerObj"), pos, Quaternion.identity) as GameObject;
			user.character = userGameObject;
		}
	}
}

