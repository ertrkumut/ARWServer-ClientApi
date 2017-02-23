﻿using System;
using System.Collections.Generic;
using System.Linq;

#pragma warning disable 0168 // variable declared but not used.
#pragma warning disable 0219 // variable assigned but not used.
#pragma warning disable 0414 // private field assigned but not used.

namespace ARWServer
{
	public class ARWObject
	{
		private string requestName 	= String.Empty;
		private string param 		= String.Empty;

		private IDictionary<string, object> dataList;


		public ARWObject(){
			dataList = new Dictionary<string, object> ();
			requestName = String.Empty;
			param = String.Empty;
		}

		public void SetRequestName(string reqName){
			this.requestName = reqName;
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

		public byte[] CompressARWObject(){
			string data = String.Empty;

			data += this.requestName + ".";
			foreach (KeyValuePair<string, object> p in dataList) {
				data += p.Key + "#" + p.Value + "_";
			}
			data = data.TrimEnd ('_');

			byte[] bytes = System.Text.Encoding.ASCII.GetBytes (data);
			return bytes;
		}
	}
}
