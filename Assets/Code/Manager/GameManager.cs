﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	
	#region Public Attributes
	#endregion
	
	#region Private Attributes
	#endregion
	
	#region MonoDevelop Methods
	//
	void Awake() {
		DontDestroyOnLoad(transform.gameObject);
	}

	// Use this for initialization
	void Start () {
		//

		//
		SceneManager.LoadSceneAsync("TestScene", LoadSceneMode.Single);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	#endregion
	
	#region User Methods
	
	#endregion
}
