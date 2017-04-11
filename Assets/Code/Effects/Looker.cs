using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Looker : MonoBehaviour {
	
	#region Public Attributes
	#endregion
	
	#region Private Attributes
	private Transform objective;
	#endregion
	
	#region MonoDevelop Methods
	// Use this for initialization
	void Start () {
		transform.LookAt (objective);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	#endregion
	
	#region User Methods
	public void SetObjective(Transform target){
		objective = target;
	}
	#endregion
}
