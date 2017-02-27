using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

	public bool locked = false;
	public Vector3 displacedPosition;		//Posicion respecto al origen cuando se mueva
	public float movementSpeed = 0.5f;

	private Vector3 originalPosition;
	private Vector3 nextPosition, previousPosition;
	private float journeyLength, startTime;

	// Use this for initialization
	void Start () {
		originalPosition = transform.position;
		displacedPosition += originalPosition;
		nextPosition = originalPosition;
		previousPosition = originalPosition;
		//
		journeyLength = Vector3.Distance(originalPosition, displacedPosition);
	}
	
	// Update is called once per frame
	void Update () {
		float distCovered = (Time.time - startTime) * movementSpeed;
		float fracJourney = distCovered / journeyLength;
		transform.position = Vector3.Lerp (previousPosition, nextPosition, fracJourney);
	}

	//Para abrirla/cerrarla
	public bool Activate(){
		if (!locked) {
			if (nextPosition == originalPosition) {
				nextPosition = displacedPosition;
				previousPosition = originalPosition;
			} else {
				nextPosition = originalPosition;
				previousPosition = displacedPosition;
			}
			startTime = Time.time;
			return true;
		} else {
			//Podremos ruido o algo de que está cerrada
			return false;
		}
	}
}
