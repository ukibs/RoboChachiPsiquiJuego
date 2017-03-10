using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlWithMouse : MonoBehaviour {

	public float maxSpeed = 5.0f;
	public Texture[] interactionIcons;


	/*
	 * 		Different status of the player
	 * 		normal
	 * 		selecting
	 * 		talking
	 */ 
	private string status = "normal";
	private int actionToDo = 0;
	private Animator modelAnimator;

	private float actualSpeed;
	private Vector3 placeToGo, direction;
	private GameObject interactable;

	private Quaternion toRotation;
	private string[] textToUse;
	private int textCounter = 0;

	private Rect[] interactionZones;

	// Use this for initialization
	void Start () {
		placeToGo = transform.position;
		actualSpeed = 0f;
		//textToUse = new string[10];			//Provisional
		interactionZones = new Rect[2];

		for (int i = 0; i < interactionZones.Length; i++) {
			interactionZones [i] = new Rect (100 * i, 100 * i, 100, 100);
		}
		//
		Transform child = transform.GetChild(0);
		modelAnimator = child.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		bool lClickDown = Input.GetMouseButtonDown (0);		//Boton izquierdo abajo
		bool lClickUp = Input.GetMouseButtonUp(0);			//Boton izquierdo arriba
		if(lClickDown)
			CheckClickDown ();
		if (lClickUp)
			CheckClickUp ();

		//Hasta que este lo bastante cerca intenrara llegar
		//Comprobamos still por si le detiene algún evento
		if (Vector3.Distance (placeToGo, transform.position) >= 1.5f && actualSpeed < maxSpeed) {
			actualSpeed += 0.2f;
			modelAnimator.SetFloat ("Speed", actualSpeed);
		}	
		else if(actualSpeed > 0.0f){
			actualSpeed -= 0.2f;
			modelAnimator.SetFloat ("Speed", actualSpeed);
		}
		//Cuando se detiene porque ha llegado, si hay un objeto almacenado
		else if (interactable && status == "normal") {
			InteractableObject interactableScript = interactable.GetComponent<InteractableObject> ();

			switch (actionToDo) {
			case 1:
				textToUse = interactableScript.Examinate ();
				status = "talking";
				break;
			case 2:
				bool succes = interactableScript.Use ();
				if (!succes) {
					//Aquí pondermos ruidito de negación o algo
				}
				break;
			}
			interactable = null;
		}
		//Para que no se esté ejecutando cuando no haga falta
		if (actualSpeed > 0.0f) {
			//Aquí lo movemos, si se tiene que mover
			transform.Translate (0.0f, 0.0f, actualSpeed * Time.deltaTime);
			//Aquí lo vamos rotando
			direction = (placeToGo - transform.position).normalized;
			Vector3 currentForward = transform.forward;
			Vector3 newForward = Vector3.Slerp(currentForward, direction, Time.deltaTime * 5f);
			transform.forward = newForward;
				//Correct inclination
			transform.eulerAngles = new Vector3 (0.0f, transform.eulerAngles.y, 0.0f);
		}
	}

	//Para iconos de accion
	void OnGUI(){
		if (status == "selecting") {
			GUI.DrawTexture (interactionZones [0], interactionIcons [0], ScaleMode.ScaleToFit, true);
			GUI.DrawTexture (interactionZones [1], interactionIcons [1], ScaleMode.ScaleToFit, true);
		}
		if (status == "talking") {
			GUI.Label(new Rect(Screen.height*4/5, Screen.width*1/5, 500, 100), textToUse[textCounter]);
		}
	}

	//Para pruebas
	void OnDrawGizmos() {
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(placeToGo, 1);
	}

	//
	public void SetInteractable(GameObject newInteractable){
		interactable = newInteractable;
	}

	//
	void CheckClickDown(){
		//CLicando lo mandamos a un sitio
		switch(status){
		case "normal":
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit)) {
				if (hit.transform.tag == "Interactable") {
					interactable = hit.transform.gameObject;

					interactionZones [0] = new Rect (Input.mousePosition.x-100, Input.mousePosition.y-20, 100, 100);
					interactionZones [1] = new Rect (Input.mousePosition.x+100, Input.mousePosition.y-20, 100, 100);
					
					status = "selecting";
				}
				else {
					interactable = null;
				}
			}
			break;
		case "talking":
			if (textCounter < textToUse.Length-1)
				textCounter++;
			else
				status = "normal";
			break;
		}
	}

	//
	void CheckClickUp(){
		//
		switch(status){
		case "normal":
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit)) {
				placeToGo = hit.point;		//Para decidir a donde ir
			}
			break;
		case "selecting":
			actionToDo = 0;
			for (int i = 0; i < interactionZones.Length; i++) {
				if (interactionZones [i].Contains (Input.mousePosition)) {
					actionToDo = i + 1;
					placeToGo = interactable.transform.position;
				}
			}
			status = "normal";
			break;
		}
	}
}
