using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using System.IO;

public class Event1 : MonoBehaviour {
	
	#region Public Attributes
	public string name;
	public GameObject securityBotPrefab;
	public Transform securityBotSpamPoint;
	public GameObject ragdollPlayer;
	#endregion
	
	#region Private Attributes
	private GameObject player;
	private ControlWithMouse playerScript;

	private GameObject painter;

	private int step = -1;				//For controlling the progress trough the scene

	private XmlDocument xml_d;
	private XmlReader xml_r;
	private string[] eventText;
	private int currentText = 0;

	private GameObject securityBot;
	private CharacterController securityBotRB;
	#endregion
	
	#region MonoDevelop Methods
	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Player");
		playerScript = player.GetComponent<ControlWithMouse> ();

		painter = GameObject.Find ("Painter");

		GetEventTextXML ();
	}
	
	// Update is called once per frame
	void Update () {
		//Delta time
		float dt = Time.deltaTime;
		//Controls
		bool lClickDown = Input.GetMouseButtonDown (0);		//Boton izquierdo abajo 
		//
		switch (step) {
		case 0:						//The painter turns towards the player
			/*Vector3 playerDirection = (player.transform.position - painter.transform.position);
			float objectiveAngle = Vector2.Angle(new Vector2(0.0f,1.0f), new Vector2(playerDirection.x, playerDirection.z));

			float angleOffset = objectiveAngle - transform.rotation.y;
			if(Mathf.Abs(angleOffset) > 180.0f) angleOffset *= -1.0f;
			angleOffset = Mathf.Clamp (angleOffset, -10.0f, 10.0f);
			painter.transform.Rotate (transform.up * angleOffset);*/
			//Aquí lo vamos rotando
			Vector3 direction = (player.transform.position - painter.transform.position).normalized;
			Vector3 currentForward = painter.transform.forward;
			currentForward.y = 0.0f;
			Vector3 newForward = Vector3.Slerp(currentForward, direction, Time.deltaTime * 5f);
			painter.transform.forward = newForward;
				//If done, next step
			//if (Mathf.Abs(angleOffset) <= 10.0f) {
			//Debug.Log(Vector3.Angle(newForward, direction));
			if(Vector3.Angle(newForward, direction) < 15.0f){		//Provisional
				step++;
			}
			break;
		case 1:					//Little conversation
			if (lClickDown) {
				currentText++;
				if(currentText == 2)
					step++;
			}
			break;
		case 2:					//Apears the security bot
			securityBot = Instantiate (securityBotPrefab, securityBotSpamPoint.position, securityBotSpamPoint.rotation);
			securityBotRB = securityBot.GetComponent<CharacterController> ();
			step++;
			break;
		case 3:
			securityBotRB.Move(securityBot.transform.forward * 5.0f * dt);
			if (Vector3.Distance (securityBot.transform.position, player.transform.position) < 3.0f) {
				step++;
			}
			break;
		case 4:
			if (lClickDown) {
				step++;
			}
			break;
		case 5:
			Instantiate (ragdollPlayer, player.transform.position, player.transform.rotation);
			Destroy (player);
			step++;
			break;
		case 6:
			GameObject fadeInOut = GameObject.Find ("FadeInOut");
			//FadeInOut fadeInOutScript = fadeInOut.GetComponent<FadeInOut> ();
			fadeInOut.SendMessage ("Switch");
			step++;
			break;
		}
	}

	//For the text
	void OnGUI(){
		if (step == 1 || step == 4) {
			GUI.Label (new Rect (Screen.width * 1 / 5, Screen.height * 4 / 5, 500, 100), eventText [currentText]);
		}
	}
	#endregion
	
	#region User Methods
	//
	protected void GetEventTextXML(){
		XmlNode objectToUse;
		XmlNodeList xmlDescription;
		TextAsset textasset = (TextAsset)Resources.Load("English", typeof(TextAsset));

		xml_d = new XmlDocument();
		xml_d.LoadXml(textasset.text);
		//Buscamos el que cuadra con el ID, o nombre
		string route = "MAIN/EVENTS/EVENT[@name='" + name + "']";
		objectToUse = xml_d.SelectSingleNode(route);

		//Y recogemos el texto que lleva
		//Vigilando también casos de texto vacio
		if (objectToUse != null) {
			xmlDescription = ((XmlElement)objectToUse).GetElementsByTagName ("texto");
			eventText = new string[xmlDescription.Count];
			int j = 0;
			foreach (XmlNode node in xmlDescription) {
				eventText [j] = node.InnerText;
				j++;
			}
		} else {
			Debug.Log ("Error, text not found for object " + name);
		}
	}

	//
	public void StartEvent(){
		step = 0;
	}
	#endregion
}
