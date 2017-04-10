using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using System.IO;

public class TalkableCharacter : InteractableObject {
	
	#region Public Attributes
	#endregion
	
	#region Private Attributes
	private XmlDocument xml_d;
	private XmlReader xml_r;
	protected string[] conversation;
	#endregion
	
	#region MonoDevelop Methods
	// Use this for initialization
	void Start () {
		InitializeObject ();	//Para que inicialicen los hijos
		GetTextXML ();
		GetConversationXML ();
	}
	#endregion
	
	#region User Methods
	//
	protected void GetConversationXML(){
		XmlNode objectToUse;
		XmlNodeList xmlDescription;
		TextAsset textasset = (TextAsset)Resources.Load("English", typeof(TextAsset));

		xml_d = new XmlDocument();
		xml_d.LoadXml(textasset.text);
		//Buscamos el que cuadra con el ID, o nombre
		string route = "MAIN/CONVERSATIONS/CONVERSATION[@name='" + name + "']";
		objectToUse = xml_d.SelectSingleNode(route);

		//Y recogemos el texto que lleva
		//Vigilando también casos de texto vacio
		if (objectToUse != null) {
			xmlDescription = ((XmlElement)objectToUse).GetElementsByTagName ("texto");
			conversation = new string[xmlDescription.Count];
			int j = 0;
			foreach (XmlNode node in xmlDescription) {
				conversation [j] = node.InnerText;
				j++;
			}
		} else {
			Debug.Log ("Error, text not found for object " + name);
		}
	}

	//
	public override string[] Talk(){
		//Ponerlo en modo conversación
		return conversation;
	}
	#endregion
}
