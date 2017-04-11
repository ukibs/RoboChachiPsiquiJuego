using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInOut : MonoBehaviour {
	
	#region Public Attributes
	public Texture fadeTexture;
	public float fadeSpeed = 0.2f;
	#endregion

	enum FadeState{
		Stopped,
		FadeIn,
		FadeOut,

		Count
	}

	#region Private Attributes

	private int drawDepth = -100;

	private float alpha = 1.0f;
	private int fadeDir = -1;

	private Color fadeColor;

	private FadeState fadeState = FadeState.FadeIn;
	#endregion
	
	#region MonoDevelop Methods
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		alpha += fadeDir * fadeSpeed * Time.deltaTime;
		alpha = Mathf.Clamp01 (alpha);
		fadeColor = GUI.color;
		fadeColor.a = alpha;

		//
		if(alpha == 1.0f && fadeDir == 1){
			//Reseteamos cambiamos escena
		}
	}

	//
	void OnGUI(){
		GUI.color = fadeColor;
		GUI.depth = drawDepth;

		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), fadeTexture);
	}
	#endregion
	
	#region User Methods
	//
	public void Switch(){
		fadeDir *= -1;
	}
	#endregion
}
