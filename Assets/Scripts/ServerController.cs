﻿using UnityEngine;
using System.Collections;
using ARWServer_UnityApi;
using System.Linq;
using System;

public class ServerController : MonoBehaviour {

	public static ServerController instanse;

	public ARWServer server;

	public string host;
	public string userName;

	void Start(){

		server = new ARWServer();
		server.Init();

		// server.host = "192.168.1.101";
		server.host = host;
		server.tcpPort = 8081;

		server.AddEventHandler(ARWEvents.CONNECTION, OnConnectionHandler);
		server.AddEventHandler(ARWEvents.LOGIN, OnLoginHandler);
		server.AddEventHandler(ARWEvents.LOGIN_ERROR, OnLoginError);
		server.AddEventHandler(ARWEvents.DISCONNECTION, OnDisconectionHandler);
		server.AddEventHandler(ARWEvents.ROOM_JOIN, RoomJoinSuccess);
		server.AddEventHandler(ARWEvents.USER_ENTER_ROOM, UserEnterRoom);

		// server.AddExtensionRequest("IamReady", IamReadyHandler);

		server.AddExtensionRequest("VerticalUpdate", VerticalUpdateHandler);
		server.AddExtensionRequest("HorizontalUpdate", HorizontalUpdateHandler);
		server.Connect();

		instanse = this;
	}

	void Update(){
		if(server != null)
			server.ProcessEvents();
	}

	private void VerticalUpdateHandler(ARWObject obj){
		Debug.Log("Vertical Update");
		int userID = obj.GetInt("userId");
		float value = obj.GetFloat("vertical");

		int requestSecond = obj.GetInt("second");
		int requestMillisecond = obj.GetInt("millisecond");

		int differenceSecond = System.DateTime.Now.Second - requestSecond;
		int differenceMillisecond = System.DateTime.Now.Millisecond - requestMillisecond;
		differenceMillisecond = Mathf.Abs(differenceMillisecond);

		Vector3 requestPos = new Vector3(obj.GetFloat("posX"), 0, obj.GetFloat("posZ"));
		Debug.Log(requestPos + " : " + differenceMillisecond);
		User user = server.me.lastJoinedRoom.GetUserList().Where(a=>a.id == userID).FirstOrDefault();
		if(user != null){
			user.character.transform.position = requestPos;
			user.character.transform.Translate(user.character.transform.TransformDirection(Vector3.forward) * value * differenceMillisecond * 0.001f);
			user.character.GetComponent<Controller>().vertical = value;
		}
	}

	private void HorizontalUpdateHandler(ARWObject obj){
		Debug.Log("Horizontal Update");
		int userID = obj.GetInt("userId");
		float value = obj.GetFloat("horizontal");
		User user = server.me.lastJoinedRoom.GetUserList().Where(a=>a.id == userID).FirstOrDefault();
		if(user != null){
			user.character.GetComponent<Controller>().horizontal = value;
		}
	}

	private void OnConnectionHandler(ARWObject obj){
		if(obj.GetString("error") != ""){
			Debug.Log("Connection Fail");
			return;
		}
		
		Debug.Log("Connection Success");
		server.SendLoginRequest(userName, new ARWObject());
	}

	private void OnLoginHandler(ARWObject obj){
		User user = obj.GetUser();
		Debug.Log(user.name + " : " + user.id + " : " + user.isMe);

		ARWObject findRoomRequest = new ARWObject();
		findRoomRequest.PutString("roomTag", "GameRoom");

		server.SendExtensionRequest("FindRoom", findRoomRequest);
	}

	private void OnLoginError(ARWObject obj){
		Debug.Log(obj.GetString("error"));
	}

	private void RoomJoinSuccess(ARWObject obj){
		Room currentRoom = obj.GetRoom();
		Debug.Log("Join Room : " + currentRoom.name + " User Count : " + currentRoom.GetUserList().Length);

		for(int ii = 0; ii< currentRoom.GetUserCount(); ii++){
			User u = currentRoom.GetUserList()[ii];
			
			if(u != server.me){
				Vector3 spawnPoint1;
				if(u.name == "umut")
					spawnPoint1 = Vector3.zero;
				else
					spawnPoint1 = new Vector3(4, 0, 0);

				u.character = (GameObject)Instantiate(Resources.Load<GameObject>("Player"), spawnPoint1, Quaternion.identity);
				u.character.name = u.name;
				u.character.GetComponent<Controller>().user = u;
			}
		}

		Vector3 spawnPoint;
		if(server.me.name == "umut")
			spawnPoint = Vector3.zero;
		else
			spawnPoint = new Vector3(4, 0, 0);

		server.me.character = (GameObject)Instantiate(Resources.Load<GameObject>("Player"), spawnPoint, Quaternion.identity);
		server.me.character.name = server.me.name;
		server.me.character.GetComponent<Controller>().user = server.me;
		// server.SendExtensionRequest("IamReady", new ARWObject(), true);
	}
	
	private void UserEnterRoom(ARWObject obj){
		User newUser = new User(obj.eventParams);
		Debug.Log("User Enter Room = " + newUser.name);
		server.me.lastJoinedRoom.AddUser(newUser);

		Vector3 spawnPoint;
		if(newUser.name == "umut")
			spawnPoint = Vector3.zero;
		else
			spawnPoint = new Vector3(4, 0, 0);

		newUser.character = (GameObject)Instantiate(Resources.Load<GameObject>("Player"), spawnPoint, Quaternion.identity);
		newUser.character.name = newUser.name;
		newUser.character.GetComponent<Controller>().user = newUser;
	}

	private void IamReadyHandler(ARWObject obj){

	}


	private void OnDisconectionHandler(ARWObject obj){
		Debug.Log("Disconnection!");
	}

	private void OnApplicationQuit(){
		server.Disconnect();
	}
}
