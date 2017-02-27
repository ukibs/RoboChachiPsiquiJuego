using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using System.IO;

public /*abstract*/ class InteractableObject : MonoBehaviour {

	private Color startcolor;
	private Renderer rend;
	private GameObject player;

	private XmlDocument xml_d;
	private XmlReader xml_r;
	protected string[] description;

	[XmlAttribute("name")] public string name;						//Para editarlo en el editor

	// Use this for initialization
	void Start () {
		InitializeObject ();	//Para que inicialicen los hijos
		GetText ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//Que destque cuando le pongamos el ratón encima
	void OnMouseEnter(){
		rend.material.color = Color.yellow;
	}

	//
	void OnMouseExit(){
		rend.material.color = startcolor;
	}

	//Interaccion de prueba
	public virtual bool Examinate(){			//Igual lo hacemos pubico para mejorar la interaccion con el personaje
		Debug.Log (description[0]);
		return true;
	}
	//
	public virtual bool Use(){
		Debug.Log ("This cannot be used");
		return false;
	}

	//
	protected virtual void InitializeObject(){
		rend = GetComponent<Renderer>();
		startcolor = rend.material.color;
		player = GameObject.Find("Player");
		description = new string[10];			//Provisional
	}

	protected void GetText(){
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
