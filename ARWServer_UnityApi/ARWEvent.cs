using System;

namespace ARWServer_UnityApi
{

	public delegate void EventHandler(ARWObject evntObj);

	public class ARWEvent
	{
		public EventHandler handler;
		public string eventName;

		public ARWEvent(){
			eventName = String.Empty;
			handler = null;
		}

		public ARWEvent(string eventName){
			this.eventName = eventName;
			handler = null;
		}
	}
}

