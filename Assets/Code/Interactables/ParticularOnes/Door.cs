using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

	public bool locked = false;
	public Vector3 displacedPosition;		//Posicion respecto al origen cuando se mueva

	private Vector3 originalPosition;
	private Vector3 nextPosition;

	// Use this for initialization
	void Start () {
		originalPosition = transform.position;
		displacedPosition += originalPosition;
		nextPosition = originalPosition;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.Lerp (transform.position, nextPosition, 1.0f * Time.deltaTime);
	}

	//Para abrirla/cerrarla
	public void Activate(){
		if (!locked) {
			if (nextPosition == originalPosition)
				nextPosition = displacedPosition;
			else
				nextPosition = originalPosition;
		} else {
			//Podremos ruido o algo de que está cerrada
		}
	}
}
