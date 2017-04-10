using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event1 : MonoBehaviour {
	
	#region Public Attributes

	#endregion
	
	#region Private Attributes
	private GameObject player;
	private ControlWithMouse playerScript;

	private GameObject painter;
	private Vector3 playerDirection;

	public GameObject securityBot;

	private int step = 0;				//For controlling the progress trough the scene
	#endregion
	
	#region MonoDevelop Methods
	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Player");
		playerScript = player.GetComponent<ControlWithMouse> ();

		painter = GameObject.Find ("Painter");
		playerDirection = (player.transform.position - transform.position).normalized;

	}
	
	// Update is called once per frame
	void Update () {
		switch (step) {
		case 0:						//The painter turns towards the player
			Vector3 currentForward = transform.forward;
			currentForward.y = 0.0f;
			Vector3 newForward = Vector3.Slerp (currentForward, playerDirection, Time.deltaTime * 1f);
			painter.transform.forward = newForward;
				//If done, next step
			if (painter.transform.eulerAngles.y - playerDirection.y <= Mathf.Epsilon) {
				step++;
			}
			break;
		case 1:
			Debug.Log ("Step1");
			break;
		}
	}
	#endregion
	
	#region User Methods
	
	#endregion
}
