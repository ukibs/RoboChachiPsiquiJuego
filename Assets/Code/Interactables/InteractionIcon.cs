using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionIcon : MonoBehaviour {

	/*
	 * Actions
	 * 1 - Examine			Mas adelante lo ahremos mejor
	 * 2 - Use
	 */
	public int action;

	private GameObject player;
	private GameObject interactable;
	private RectTransform imageRectTrans;
	private Rect imageRect;

	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Player");
		imageRectTrans = gameObject.GetComponent<RectTransform> ();
		imageRect = imageRectTrans.rect;
		Debug.Log (imageRect.ToString ());
	}
	
	// Update is called once per frame
	void Update () {
		bool clickUp = Input.GetMouseButtonUp (0);
		if (clickUp){
			if (imageRect.Contains (Event.current.mousePosition)) {
				Debug.Log ("Mouse Up");
			}
		}
	}

	//
	void OnMouseUp(){
		player.SendMessage ("ActionToDo", action);
		player.SendMessage ("InteractableToInteract", interactable);
		Debug.Log ("Mouse Up");
	}

	//
	public void SetInteractable(GameObject newInteractable){
		interactable = newInteractable;
	}
}
