using System;

namespace ARWServer
{

	public delegate void EventHandler();

	public class ARWEvent
	{
		public EventHandler handler;

	}
}

