using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class butt : MonoBehaviour {

	#region Public Attributes
	#endregion
	
	#region Private Attributes
	#endregion
	
	#region typedef
	#endregion

	#region Properties
	#endregion
	
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void ButtonPlay()
	{
		Application.LoadLevel("Preload");
	}

	public void ButtonExit()
	{
		Application.Quit();
	}
}
