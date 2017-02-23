using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public /*abstract*/ class InteractableWithAction : InteractableObject {

	// Use this for initialization
	/*void Start () {
		InitializeObject ();
	}*/
	
	// Update is called once per frame
	void Update () {
		
	}

	protected virtual void Interact(){			//Igual lo hacemos pubico para mejorar la interaccion con el personaje
		Debug.Log (description[1]);
		//Debug.Log ("You've used it");
	}
}
