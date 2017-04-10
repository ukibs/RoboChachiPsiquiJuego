using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using System.IO;

public /*abstract*/ class InteractableObject : MonoBehaviour {

	[XmlAttribute("name")] public string name;						//Para editarlo en el editor
	public float distanceToInteract = 1.5f;

	private Color startcolor;
	private Renderer rend;
	private GameObject player;


	private XmlDocument xml_d;
	private XmlReader xml_r;
	protected string[] description;



	// Use this for initialization
	void Start () {
		InitializeObject ();	//Para que inicialicen los hijos
		GetTextXML ();
	}

	//Que destque cuando le pongamos el ratón encima
	void OnMouseEnter(){
		//rend.material.color = Color.yellow;
	}

	//
	void OnMouseExit(){
		//rend.material.color = startcolor;
	}

	//Interaccion de prueba
	public virtual string[] Examinate(){			//Igual lo hacemos pubico para mejorar la interaccion con el personaje
		//Debug.Log (description[0]);
		return description;
	}

	//
	public virtual bool Use(){
		Debug.Log ("This cannot be used");
		return false;
	}

	//
	public virtual string[] Talk(){
		return null;
	}

	//
	public float GetDistanceToInteract(){
		return distanceToInteract;
	}

	//
	protected virtual void InitializeObject(){
		
		//rend = GetComponent<Renderer>();
		//startcolor = rend.material.color;
		player = GameObject.Find("Player");
		//canvas = GameObject.Find("Canvas");					No funciona con inactivos
	}

	//
	protected void GetTextXML(){
		XmlNode objectToUse;
		XmlNodeList xmlDescription;
		TextAsset textasset = (TextAsset)Resources.Load("English", typeof(TextAsset));

		xml_d = new XmlDocument();
		xml_d.LoadXml(textasset.text);
		//Buscamos el que cuadra con el ID, o nombre
		string route = "MAIN/OBJECTS/OBJECT[@name='" + name + "']";
		objectToUse = xml_d.SelectSingleNode(route);

		//Y recogemos el texto que lleva
		//Vigilando también casos de texto vacio
		if (objectToUse != null) {
			xmlDescription = ((XmlElement)objectToUse).GetElementsByTagName ("texto");
			description = new string[xmlDescription.Count];
			int j = 0;
			foreach (XmlNode node in xmlDescription) {
				description [j] = node.InnerText;
				j++;
			}
		} else {
			Debug.Log ("Error, text not found for object " + name);
		}
	}
}
