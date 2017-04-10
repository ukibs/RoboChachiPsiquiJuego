using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlWithMouse : MonoBehaviour {

	#region Public Attributes
	public float maxSpeed = 5.0f;
	public Texture[] interactionIcons;
	public float defaultInteractionDistance = 1.5f;
	#endregion

	#region Private Attributes
	/*
	 * 		Different status of the player
	 * 		normal
	 * 		selecting
	 * 		talking
	 */ 
	private State status = State.Normal;
	private int actionToDo = 0;
	private Animator modelAnimator;
	private Rigidbody rb;

	private float actualSpeed;
	private Vector3 placeToGo, direction;
	private GameObject interactable;

	private Quaternion toRotation;
	private string[] textToUse;
	private int textCounter = 0;

	private Rect[] interactionZones;
	private float distanceToInteract;
	private AudioSource cannotUseSound;
	#endregion

	enum State{
		Normal,
		Selecting,
		Examining,
		Talking,
		InEvent,

		Count
	}

	#region MonoDevelop Methods
	// Use this for initialization
	void Start () {
		rb = gameObject.GetComponent<Rigidbody> ();
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
		//
		distanceToInteract = defaultInteractionDistance;

		cannotUseSound = gameObject.GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		bool lClickDown = Input.GetMouseButtonDown (0);		//Boton izquierdo abajo
		bool lClickUp = Input.GetMouseButtonUp(0);			//Boton izquierdo arriba
		if(lClickUp)
			CheckClick ();
		

		UpdateAction ();
		
	}

	//Para iconos de accion
	void OnGUI(){
		if (status == State.Selecting) {
			//Lo hacemos asi porque se descuadra en y
			GUI.DrawTexture (new Rect(interactionZones[0].x, Screen.height - interactionZones[0].y - interactionZones[0].height, 
				interactionZones[0].width, interactionZones[0].height), 
				interactionIcons [0], ScaleMode.ScaleToFit, true);
			GUI.DrawTexture (new Rect(interactionZones[1].x, Screen.height - interactionZones[1].y - interactionZones[1].height, 
				interactionZones[1].width, interactionZones[1].height), 
				interactionIcons [1], ScaleMode.ScaleToFit, true);

		}
		else if (status == State.Examining) {
			GUI.Label(new Rect(Screen.width*1/5, Screen.height*4/5, 500, 100), textToUse[textCounter]);
		}
		else if(status == State.Talking){
			//Luego añadiremos para que diferencie quien habla
			GUI.Label(new Rect(Screen.width*1/5, Screen.height*4/5, 500, 100), textToUse[textCounter]);
		}
	}

	//Para pruebas
	void OnDrawGizmos() {
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(placeToGo, 1);
	}
	#endregion

	#region User Methods
	//
	void CheckClick(){
		//CLicando lo mandamos a un sitio
		switch(status){
		case State.Normal:
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit)) {
				if (hit.transform.tag == "Interactable" || hit.transform.tag == "Character") {
					interactable = hit.transform.gameObject;

					interactionZones [0] = new Rect (Input.mousePosition.x-150, Input.mousePosition.y-20, 100, 100);
					interactionZones [1] = new Rect (Input.mousePosition.x+50, Input.mousePosition.y-20, 100, 100);
					
					status = State.Selecting;
				}
				else {
					interactable = null;
					placeToGo = hit.point;		//Para decidir a donde ir
					distanceToInteract = defaultInteractionDistance;
				}
			}
			break;
		case State.Examining:
			if (textCounter < textToUse.Length - 1)
				textCounter++;
			else {
				status = State.Normal;
				textCounter = 0;
			}
			break;
		case State.Talking:							//De momento igual que examinar
			if (textCounter < textToUse.Length - 1)	//Lo cambiaremos mas adelante
				textCounter++;
			else {
				status = State.Normal;
				textCounter = 0;
			}
			break;
		case State.Selecting:
			actionToDo = 0;
			for (int i = 0; i < interactionZones.Length; i++) {
				if (interactionZones [i].Contains (Input.mousePosition)) {
					actionToDo = i + 1;
					placeToGo = interactable.transform.position;
					InteractableObject interactableScript = interactable.GetComponent<InteractableObject> ();
					distanceToInteract = interactableScript.GetDistanceToInteract ();
				}
			}
			status = State.Normal;
			break;
		}
	}

	//
	void UpdateAction(){
		//Hasta que este lo bastante cerca intenrara llegar
		//Sacamos la posicion ignorando la altura para comparar
		Vector2 horizontalPosition = new Vector2(transform.position.x, transform.position.z);
		Vector2 horizontalPlaceToGo = new Vector2 (placeToGo.x, placeToGo.z);
		if (Vector2.Distance (horizontalPlaceToGo, horizontalPosition) >= distanceToInteract 
			&& actualSpeed < maxSpeed) {
			actualSpeed += 0.2f;
			modelAnimator.SetFloat ("Speed", actualSpeed/maxSpeed);
		}	
		else if(actualSpeed > 0.0f){
			actualSpeed -= 0.2f;
			modelAnimator.SetFloat ("Speed", actualSpeed/maxSpeed);
		}
		//Cuando se detiene porque ha llegado, si hay un objeto almacenado
		else if (interactable && status == State.Normal) {
			InteractableObject interactableScript = interactable.GetComponent<InteractableObject> ();

			switch (actionToDo) {
			case 1:
				textToUse = interactableScript.Examinate ();
				status = State.Talking;
				break;
			case 2:
				bool succes;
				if (interactable.transform.tag == "Character") {
					textToUse = interactableScript.Talk ();
					status = State.Talking;
					succes = true;
				} else {
					succes = interactableScript.Use ();
				}
				if (!succes) {
					//Aquí pondermos ruidito de negación o algo
					cannotUseSound.Play();
				}
				break;
			}
			interactable = null;
		}
		//Para que no se esté ejecutando cuando no haga falta
		//if (actualSpeed > 0.0f) {
		if(actualSpeed != rb.velocity.z){
			//Aquí lo movemos, si se tiene que mover
			rb.velocity = transform.forward * actualSpeed;
		}
		//Aquí lo vamos rotando
		direction = (placeToGo - transform.position).normalized;
		Vector3 currentForward = transform.forward;
		currentForward.y = 0.0f;
		Vector3 newForward = Vector3.Slerp(currentForward, direction, Time.deltaTime * 5f);
		transform.forward = newForward;
	}

	//For working with events
	public void SetEventStatus(){
		status = State.InEvent;
	}

	public void UnsetEventStatus(){
		status = State.Normal;
	}
	#endregion
}
