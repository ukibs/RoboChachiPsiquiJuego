using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : MonoBehaviour {

	private Transform cameraPoint;

	// Use this for initialization
	void Start () {
		//cameraPoint = transform.FindChild ("Camera Point");
		cameraPoint = transform.parent;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//
	void OnTriggerEnter(Collider other){
		if (other.transform.name == ("Player")) {
			Camera.main.transform.position = cameraPoint.position;
			Camera.main.transform.eulerAngles = cameraPoint.eulerAngles;
			//Debug.Log ("Ha entrado");
		}
	}
}
