using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintCube : InteractableObject {
	
	#region Public Attributes
	public Vector3 positionOverThePlayer = new Vector3(0.0f, 2.0f, 0.0f);
	public float timePainting = 2.0f;
	public GameObject fallingPaintPrefab;
	public GameObject event1;
	#endregion
	
	#region Private Attributes
	private Vector3 originalPosition;
	private bool triggered = false;
	private float count = 0.0f;
	private GameObject fallingPaint;
	private GameObject player;
	private ControlWithMouse playerScript;
	#endregion
	
	#region MonoDevelop Methods
	/*void Start () {
		InitializeObject ();	//Para que inicialicen los hijos
		GetTextXML ();
	}*/

	void Update(){
		if (triggered) {
			count += Time.deltaTime;
			if (count >= timePainting) {
				Destroy(fallingPaint);
				//Creamos pegote en el suelo
				//Aquí activaremos el evento
				//playerScript.UnsetEventStatus();		//Este lo quitamos cuando esté hecho el evento
				//Instantiate(event1, transform.position, Quaternion.identity);
				event1.SendMessage("StartEvent");
				triggered = false;
				transform.position = originalPosition;
				//Aquí lo volvemos a dejar en el suelo
			}
		}
	}
	#endregion
	
	#region User Methods
	public override bool Use(){			//Igual lo hacemos pubico para mejorar la interaccion con el personaje
		//objectiveDoor.SendMessage("Activate");
		originalPosition = transform.position;
		player = GameObject.Find("Player");
		playerScript = player.GetComponent<ControlWithMouse> ();
		playerScript.SetEventStatus ();
		transform.position = player.transform.position + positionOverThePlayer;
		fallingPaint = Instantiate (fallingPaintPrefab, player.transform.position, Quaternion.identity);
		triggered = true;
		return true;
	}
	#endregion
}
