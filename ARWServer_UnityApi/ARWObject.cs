using System;
using System.Collections.Generic;
using System.Linq;

#pragma warning disable 0168 // variable declared but not used.
#pragma warning disable 0219 // variable assigned but not used.
#pragma warning disable 0414 // private field assigned but not used.

namespace ARWServer
{
	public class ARWObject
	{
		private string 						requestName;
		public SpecialEventParam 			specialParam;

		private IDictionary<string, object> dataList;


		public ARWObject(){
			dataList 		= new Dictionary<string, object> ();
			requestName 	= String.Empty;
			specialParam 	= new SpecialEventParam ();
		}

		public void SetRequestName(string reqName){
			this.requestName = reqName;
		}

		public string GetRequestName(){
			return this.requestName;
		}
		public void PutString(string key, string value){
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

		public static ARWObject Extract(byte[] bytes){
			string data = System.Text.Encoding.UTF8.GetString (bytes).Replace("\0", null).Replace("\"",null);

			ARWObject newObj = new ARWObject ();
			string[] dataParts = data.Split ('.');
			newObj.requestName = dataParts [0];

			string[] prms = dataParts [1].Split ('_');
			foreach (string p in prms) {
				string[] paramParts = p.Split ('#');
				if (paramParts.Length == 2)
					newObj.dataList.Add (paramParts [0], paramParts [1]);
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

			data += "." + specialParam.Compress ();

			byte[] bytes = System.Text.Encoding.UTF8.GetBytes (data);
			return bytes;
		}
	}
}
