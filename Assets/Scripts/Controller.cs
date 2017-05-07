using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ARWServer_UnityApi;

public class Controller : MonoBehaviour {

	public float speed;
	
	private float tempVertical = 0, tempHorizontal = 0;
	public float vertical, horizontal;

	public User user;

	void Update () {
	
		if(this.user.isMe){
			this.transform.Translate(this.transform.TransformDirection(Vector3.forward) * Input.GetAxisRaw("Vertical") * speed * Time.deltaTime);
			this.transform.Rotate(this.transform.TransformDirection(Vector3.up) * Input.GetAxisRaw("Horizontal") * speed * 30 * Time.deltaTime);

			if(tempVertical != Input.GetAxisRaw("Vertical")){
				tempVertical = Input.GetAxisRaw("Vertical");

				ARWObject obj = new ARWObject();
				obj.PutFloat("vertical", tempVertical);
				obj.PutInt("millisecond", System.DateTime.Now.Millisecond);
				obj.PutInt("second", System.DateTime.Now.Second);
				obj.PutFloat("posX", this.transform.position.x);
				obj.PutFloat("posZ", this.transform.position.z);
				ServerController.instanse.server.SendExtensionRequest("VerticalUpdate", obj, true, true);
			}

			if(tempHorizontal != Input.GetAxisRaw("Horizontal")){
				tempHorizontal = Input.GetAxisRaw("Horizontal");

				ARWObject obj = new ARWObject();
				obj.PutFloat("horizontal", tempHorizontal);
				obj.PutInt("millisecond", System.DateTime.Now.Millisecond);
				obj.PutInt("second", System.DateTime.Now.Second);
				obj.PutFloat("rotX", this.transform.eulerAngles.x);
				obj.PutFloat("rotY", this.transform.eulerAngles.y);
				obj.PutFloat("rotZ", this.transform.eulerAngles.z);
				ServerController.instanse.server.SendExtensionRequest("HorizontalUpdate", obj, true, true);
			}
		}else{
			this.transform.Translate(this.transform.TransformDirection(Vector3.forward) * this.vertical * speed * Time.deltaTime);
			this.transform.Rotate(this.transform.TransformDirection(Vector3.right) * this.horizontal * speed * Time.deltaTime);
		}
		
	}
}
