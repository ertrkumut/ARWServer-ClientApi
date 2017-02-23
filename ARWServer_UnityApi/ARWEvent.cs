using System;

namespace ARWServer
{

	public delegate void EventHandler(ARWObject evntObj);

	public class ARWEvent
	{
		public EventHandler handler;

	}
}

