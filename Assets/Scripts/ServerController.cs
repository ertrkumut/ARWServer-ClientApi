﻿using UnityEngine;
using System.Collections;
using ARWServer_UnityApi;

public class ServerController : MonoBehaviour {

	public ARWServer server;

	void Start(){

		server = new ARWServer();
		server.Init();

		server.host = "127.0.0.1";
		server.tcpPort = 8081;

		server.AddEventHandler(ARWEvents.CONNECTION, OnConnectionHandler);
		server.AddEventHandler(ARWEvents.LOGIN, OnLoginHandler);
		server.AddEventHandler(ARWEvents.LOGIN_ERROR, OnLoginError);
		server.AddEventHandler(ARWEvents.DISCONNECTION, OnDisconectionHandler);
		server.AddEventHandler(ARWEvents.USER_ENTER_ROOM, UserEnterRoom);
		server.AddExtensionRequest("Hello", TestHandler);
		server.Connect();
	}

	void Update(){
		if(server != null)
			server.ProcessEvents();
	}

	private void OnConnectionHandler(ARWObject obj){
		if(obj.GetString("error") != ""){
			Debug.Log("Connection Fail");
			return;
		}
		
		Debug.Log("Connection Success");
		server.SendLoginRequest("umut", new ARWObject());
	}

	private void OnLoginHandler(ARWObject obj){
		User user = obj.GetUser();
		Debug.Log(user.name + " : " + user.id + " : " + user.isMe);
	}

	private void OnLoginError(ARWObject obj){
		Debug.Log(obj.GetString("error"));
	}

	private void TestHandler(ARWObject obj){
		Debug.Log(obj.GetString("Deneme"));
	}
	
	private void UserEnterRoom(ARWObject obj){
		Debug.Log("User Enter Room = " + obj.eventParams.GetString("userName") + " ID = " + obj.eventParams.GetInt("userId"));
	}

	private void OnDisconectionHandler(ARWObject obj){
		Debug.Log("Disconnection!");
	}
}
