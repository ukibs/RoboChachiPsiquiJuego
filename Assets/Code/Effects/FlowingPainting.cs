using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowingPainting : MonoBehaviour {
	
	#region Public Attributes
	#endregion
	
	#region Private Attributes
	private Renderer paintRenderer;
	#endregion
	
	#region MonoDevelop Methods
	// Use this for initialization
	void Start () {
		paintRenderer = gameObject.GetComponent<Renderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		float textureOffsetY = paintRenderer.material.GetTextureOffset ("_MainTex").y;
		paintRenderer.material.SetTextureOffset ("_MainTex", new Vector2(0,  textureOffsetY + 0.01f));
	}
	#endregion
	
	#region User Methods
	
	#endregion
}
