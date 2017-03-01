using System;
using System.Collections.Generic;
using System.Linq;

#pragma warning disable 0168 // variable declared but not used.
#pragma warning disable 0219 // variable assigned but not used.
#pragma warning disable 0414 // private field assigned but not used.

namespace ARWServer
{
	public class SpecialEventParam
	{

		private IDictionary<string, object> dataList;

		public void PutVariable(string key, object value){
			this.dataList.Add (key, value);
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

		public string Compress(){
			string data = string.Empty;

			foreach (KeyValuePair<string, object> p in dataList) {
				data += p.Key + "#" + p.Value + "_";
			}
			data = data.TrimEnd ('_');

			return data;
		}

		public static SpecialEventParam Extract(byte[] bytes){
			SpecialEventParam newSpecialEventParam = new SpecialEventParam();

			string data = System.Text.Encoding.UTF8.GetString (bytes).Replace ("\0", null).Replace ("\"", null);

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

