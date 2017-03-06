using System;
using System.Collections.Generic;
using System.Linq;

#pragma warning disable 0168 // variable declared but not used.
#pragma warning disable 0219 // variable assigned but not used.
#pragma warning disable 0414 // private field assigned but not used.

namespace ARWServer_UnityApi
{
	public class SpecialEventParam
	{

		private IDictionary<string, object> dataList;

		public void PutVariable(string key, object value){
			this.dataList.Add (key, value);
		}

		public string GetString(string key){

			try{
				var entry = dataList.Where (a => a.Key == key).Select (a => (KeyValuePair<string,object>?) a).FirstOrDefault ();
				return entry.Value.Value.ToString();
			}catch(System.NullReferenceException e){
				Console.WriteLine ("There was nothing like " + key);
			}

			return string.Empty;	
		}

		public int GetInt(string key){
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

		public string Compress(){
			string data = string.Empty;

			foreach (KeyValuePair<string, object> p in dataList) {
				data += p.Key + "#" + p.Value + "_";
			}
			data = data.TrimEnd ('_');

			return data;
		}

		public static SpecialEventParam Extract(string data){
			SpecialEventParam newSpecialEventParam = new SpecialEventParam();

			if (data == null)
				return newSpecialEventParam;

			string[] variables = data.Split ('_');

			foreach (string variable in variables) {
				string[] varParts = variable.Split ('#');
				if (varParts.Length == 2)
					newSpecialEventParam.dataList.Add (varParts [0], varParts [1]);
			}

			return newSpecialEventParam;
		}

		public SpecialEventParam ()
		{
			this.dataList = new Dictionary<string, object> ();
		}
	}
}

