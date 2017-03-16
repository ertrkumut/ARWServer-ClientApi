using System;
using System.Collections.Generic;
using System.Linq;

#pragma warning disable 0168 // variable declared but not used.
#pragma warning disable 0219 // variable assigned but not used.
#pragma warning disable 0414 // private field assigned but not used.

namespace ARWServer_UnityApi
{

	public class ARWObject
	{
		private string 						requestName;
		public SpecialEventParam 			eventParams;

		private IDictionary<string, object> dataList;


		public ARWObject(){
			dataList 		= new Dictionary<string, object> ();
			requestName 	= String.Empty;
			eventParams 	= new SpecialEventParam ();
		}

		public void SetRequestName(string reqName){
			this.requestName = reqName;
		}

		public string GetRequestName(){
			return this.requestName;
		}

		public void PutString(string key, string value){
			Console.WriteLine (key + " : " + value);
			dataList.Add (key, value);	
		}

		public void PutInt(string key, int value){
			dataList.Add (key, value);	
		}

		public void PutFloat(string key, float value){
			dataList.Add (key, value);
		}

		public string GetString(string key){
			var entry = dataList.Where (a => a.Key == key).Select (a => (KeyValuePair<string,object>?) a).FirstOrDefault ();

			try{
				return entry.Value.Value.ToString();
			}catch(System.NullReferenceException e){
				Console.WriteLine ("There was nothing like " + key);
			}

			return string.Empty;
		}

		public float GetInt(string key){
			var entry = dataList.Where (a => a.Key == key).Select (a => (KeyValuePair<string,object>?) a).FirstOrDefault ();

			try{
				return int.Parse(entry.Value.Value.ToString());
			}catch(System.NullReferenceException e){
				Console.WriteLine ("There was nothing like " + key);
			}

			return 0;
		}

		public float GetFloat(string key){
			var entry = dataList.Where (a => a.Key == key).Select (a => (KeyValuePair<string,object>?) a).FirstOrDefault ();

			try{
				return float.Parse(entry.Value.Value.ToString());
			}catch(System.NullReferenceException e){
				Console.WriteLine ("There was nothing like " + key);
			}

			return 0.0f;
		}

		public static ARWObject Extract(byte[] bytes){
			string data = System.Text.Encoding.UTF8.GetString (bytes).Replace("\0", null).Replace("\"",null);

			ARWObject newObj = new ARWObject ();
			string[] dataParts = data.Split ('.');
			if (dataParts.Length == 3) {
				newObj.requestName = dataParts [0];

				string[] prms = dataParts [1].Split ('_');
				foreach (string p in prms) {
					string[] paramParts = p.Split ('#');
					if (paramParts.Length == 2)
						newObj.dataList.Add (paramParts [0], paramParts [1]);
				}

				newObj.eventParams = SpecialEventParam.Extract (dataParts [2]);
				return newObj;
			}
			return newObj;
		}

		public byte[] Compress(){
			string data = String.Empty;

			data += this.requestName + ".";
			foreach (KeyValuePair<string, object> p in dataList) {
				data += p.Key + "#" + p.Value + "_";
			}
			data = data.TrimEnd ('_');

			data += "." + eventParams.Compress ();

			byte[] bytes = System.Text.Encoding.UTF8.GetBytes (data);
			return bytes;
		}

		public User GetUser(){
			User u = UserManager.allUserInGame.Where (a => a.id == this.eventParams.GetInt ("userId")).FirstOrDefault ();
			return u;
		}

		public Room GetRoom(){
			Room newRoom = ARWServer.allRooms.Where (a => a.name == this.eventParams.GetString ("RoomName")).FirstOrDefault ();
			return newRoom;
		}
	}
}
