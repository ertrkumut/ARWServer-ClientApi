using System;

namespace ARWServer_UnityApi
{

	public delegate void EventHandler(ARWObject evntObj);
	public delegate void PrivateHandler(ARWServer server, ARWObject evntObj);

	public class ARWEvent
	{
		public EventHandler handler;
		public PrivateHandler p_handler;
		public string eventName;

		public ARWEvent(){
			eventName = String.Empty;
			handler = null;
			p_handler = null;
		}

		public ARWEvent(string eventName){
			this.eventName = eventName;
			handler = null;
			p_handler = null;
		}
	}
}

