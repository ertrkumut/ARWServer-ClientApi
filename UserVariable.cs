using System;
using UnityEngine;

namespace ARWServer_UnityApi{
	public class UserVariable{
		public string key;
		private object value;

		public UserVariable(string key, object value){
			this.key = key;
			this.value = value;
		}

		public string GetStringValue(){
			return this.value.ToString();
		}

		public int GetIntValue(){
			return int.Parse(this.value.ToString());
		}

		public float GetFloatValue(){
			return float.Parse(this.value.ToString());
		}

		public bool GetBoolValue(){
			return bool.Parse(this.value.ToString());
		}
	}	
}