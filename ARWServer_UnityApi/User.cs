using System;

namespace ARWServer_UnityApi
{
	public class User
	{

		public string name;
		public int id;
		public bool isMe;

		public User (SpecialEventParam e){
			this.name = e.GetString ("userName");
			this.id = e.GetInt ("userId");
			this.isMe = bool.Parse (e.GetString ("isMe"));
		}
	}
}

