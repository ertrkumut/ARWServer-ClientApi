using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ARWServer_UnityApi;

public class Controller : MonoBehaviour {

	public float speed;
	
	private float tempVertical = 0, tempHorizontal = 0;
	public float vertical, horizontal;

	public GameObject body;

	public User user;

	void Update () {
		if(this.user == null)
			return;

		if(this.user.isMe){
			this.transform.Translate(Vector3.forward * Input.GetAxisRaw("Vertical") * speed * Time.deltaTime);
			this.transform.Rotate(Vector3.up * Input.GetAxisRaw("Horizontal") * speed * 50 * Time.deltaTime);

			if(tempVertical != Input.GetAxisRaw("Vertical")){
				tempVertical = Input.GetAxisRaw("Vertical");

				ARWObject obj = new ARWObject();
				obj.PutFloat("vertical", tempVertical);
				obj.PutInt("millisecond", ServerController.instanse.server.serverTime.Millisecond);
				obj.PutInt("second", ServerController.instanse.server.serverTime.Second);
				obj.PutFloat("posX", this.transform.position.x);
				obj.PutFloat("posZ", this.transform.position.z);
				ServerController.instanse.server.SendExtensionRequest("VerticalUpdate", obj, true, true);
			}

			if(tempHorizontal != Input.GetAxisRaw("Horizontal")){
				tempHorizontal = Input.GetAxisRaw("Horizontal");

				ARWObject obj = new ARWObject();
				obj.PutFloat("horizontal", tempHorizontal);
				obj.PutInt("millisecond", ServerController.instanse.server.serverTime.Millisecond);
				obj.PutInt("second", ServerController.instanse.server.serverTime.Second);
				obj.PutFloat("rotX", this.transform.eulerAngles.x);
				obj.PutFloat("rotY", this.transform.eulerAngles.y);
				obj.PutFloat("rotZ", this.transform.eulerAngles.z);
				ServerController.instanse.server.SendExtensionRequest("HorizontalUpdate", obj, true, true);
			}
		}else{	
			this.body.transform.position = new Vector3(this.transform.position.x, this.body.transform.position.y, this.transform.position.z);
			this.transform.Translate(Vector3.forward * this.vertical * speed * Time.deltaTime);
			this.transform.Rotate(new Vector3(0,1,0) * this.horizontal * speed * 100 * Time.deltaTime);
			body.transform.Rotate(this.transform.TransformDirection(Vector3.right) * speed * 100 * vertical * Time.deltaTime);
		}
		
	}
}
