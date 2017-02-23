using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpener : InteractableWithAction {

	public GameObject objectiveDoor;

	// Use this for initialization
	/*void Start () {
		
	}*/
	
	// Update is called once per frame
	void Update () {
		
	}

	protected override void Interact(){			//Igual lo hacemos pubico para mejorar la interaccion con el personaje
		objectiveDoor.SendMessage("Activate");
		Debug.Log("Interacting");
	}
}
