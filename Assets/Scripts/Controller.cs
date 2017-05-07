using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ARWServer_UnityApi;

public class Controller : MonoBehaviour {

	public float speed;
	
	private float tempVertical = 0, tempHorizontal = 0;

	void FixedUpdate () {
	
		this.transform.Translate(this.transform.TransformDirection(Vector3.forward) * Input.GetAxisRaw("Vertical") * speed);
		this.transform.Translate(this.transform.TransformDirection(Vector3.right) * Input.GetAxisRaw("Horizontal") * speed);

		if(tempVertical != Input.GetAxisRaw("Vertical")){
			tempVertical = Input.GetAxisRaw("Vertical");

			ARWObject obj = new ARWObject();
			obj.PutFloat("vertical", tempVertical);

			ServerController.instanse.server.SendExtensionRequest("VerticalUpdate", obj, true, true);
		}

		if(tempHorizontal != Input.GetAxisRaw("Horizontal")){
			tempHorizontal = Input.GetAxisRaw("Horizontal");

			ARWObject obj = new ARWObject();
			obj.PutFloat("horizontal", tempHorizontal);

			ServerController.instanse.server.SendExtensionRequest("HorizontalUpdate", obj, true, true);
		}

	}
}
