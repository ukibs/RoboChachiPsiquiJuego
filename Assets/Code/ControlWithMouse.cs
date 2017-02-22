using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlWithMouse : MonoBehaviour {

	public float maxSpeed = 5.0f;

	private float actualSpeed;
	private bool still = false;
	private Vector3 placeToGo, direction;
	private GameObject interactable;
	private Quaternion toRotation;

	// Use this for initialization
	void Start () {
		placeToGo = transform.position;
		actualSpeed = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		bool click = Input.GetMouseButtonDown (0);
		CheckClick (click);

		//Hasta que este lo bastante cerca intenrara llegar
		//Comprobamos still por si le detiene algún evento
		if (Vector3.Distance (placeToGo, transform.position) >= 1.5f && still == false && actualSpeed < maxSpeed) {
			actualSpeed += 0.2f;
		}	
		else if(actualSpeed > 0.0f){
			actualSpeed -= 0.2f;
		}
		//Cuando se detiene porque ha llegado, si hay un objeto almacenado
		else if (interactable) {
			still = true;								//Lo detenemos para que 
			interactable.SendMessage ("Interact");
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
	void CheckClick(bool click){
		//CLicando lo mandamos a un sitio
		if (click) {
			if (still == true) {
				still = false;
			} else {
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast (ray, out hit)) {
					placeToGo = hit.point;
					if (hit.transform.tag == "Interactable")
						interactable = hit.transform.gameObject;
					else
						interactable = null;
				}
			}
		}
	}
}
