using System;
using System.Net.Sockets;

namespace ARWServer
{

	public class ARWServer{

		public string host;
		public int tcpPort;
		public int udpPort;
		
		public void Connect(){
			TcpClient client = new TcpClient();
			client.Connect(host, tcpPort);

			NetworkStream newMsj = client.GetStream();
			byte[] bytesToSend = System.Text.Encoding.ASCII.GetBytes("Success");
			client.Client.Send(bytesToSend);
		}
	}

	class Program{

		public static ARWServer arwServer;

		public static void Main(){
			arwServer = new ARWServer();
			Console.WriteLine("================================");
			arwServer.host = "localhost";
			arwServer.tcpPort = 8081;

			arwServer.Connect();
		}
	}
}
