using System;

namespace ARWServer
{
	public class ExampleClient
	{
		public static ARWServer arwServer;

		public static void Main(){
			arwServer = new ARWServer();

			Console.WriteLine("================================");
			arwServer.host = "127.0.0.1";
			arwServer.tcpPort = 8081;

			arwServer.Init ();

			arwServer.AddEventHandler (ARWEvents.CONNECTION, ConnectionSuccess);
			arwServer.Connect();

			arwServer.ProcessEvents ();
		}

		public static void ConnectionSuccess(ARWObject evntObj){
			Console.WriteLine ("Connection Success");
			arwServer.SendLoginRequest ("umut", null);
		}
	}
}

