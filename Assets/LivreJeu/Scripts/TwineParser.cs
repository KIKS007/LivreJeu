/**
 * Original Twine Parser by Benjamin Leblanc
 * 
 * What was added:
 * 	- Using Twine Tags as Variable used in Unity
 * 	- Custom Condition system using Title Parsing from Twine Node
 * 	- Cleaning Node Titles to make it usable in UI
 *  - Name Parsing in Title
 *  - "Suivant" system to make the reading more comfy when the user has no choice/interaction with the story
 * 
 **/

using UnityEngine;
using System.Collections;


using System;

using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Text;

public class TwineParser : MonoBehaviour
{
	public TextAsset txt;

	public bool Suivant = false;

	public string Suivant_Node;

	public GameObject UI_Text_Main;
	public GameObject UI_Text_Button1;
	public GameObject UI_Text_Button2;
	public GameObject UI_Text_Button3;
	public GameObject UI_Text_Button4;
	public GameObject UI_Text_Button5;
	public GameObject UI_Button1;
	public GameObject UI_Button2;
	public GameObject UI_Button3;
	public GameObject UI_Button4;
	public GameObject UI_Button5;
	public GameObject GameManager;

	//public int ChoiceNumber;

	bool autoCondition = true;


	private Node LastNode;
	private int iRead = 0;

	private Dictionary<string, Node> Dialogue = new Dictionary<string, Node> ();
	// Use this for initialization
	void Start ()
	{
		string text = txt.text;

		text = text.Replace (" hidden", "");
		//text.Replace ("><", "\n");


		string content;
		string[] contentArr;
		string tag;
		string[] tags;

		XmlTextReader reader = new XmlTextReader (new StringReader (text));
		while (reader.Read ()) {
			if (reader.Name == "tw-passagedata") {

				string name = reader.GetAttribute ("name");
				tag = reader.GetAttribute ("tags");
				tags = tag.Split (" " [0]);
				content = reader.ReadElementContentAsString ();
				content = content.Replace ("]]", "");
				content = content.Replace ("\n", "");
				contentArr = content.Split ("[[" [0]);

				for (int i = 0; i <= contentArr.Length - 1; i++) {
					Node n = new Node ();
					if (i == 0) {
						n.content = contentArr [i];
						n.choiceText = name;
						n.Tags = tags;
					}

					if (n.content != null) {
						Dialogue.Add (n.choiceText.ToLower (), n);
					}
				}
			}
		}

		/*
		Debug.Log ("- NODE LIST ------");
		Node myNode;
		Dialogue.TryGetValue ("Start", out myNode);
		

		foreach (Node n in Dialogue.Values) {
			Debug.Log (n.choiceText.Length);
		}
		Debug.Log ("-----------------");
*/
		reader = new XmlTextReader (new StringReader (text));
		while (reader.Read ()) {
			if (reader.Name == "tw-passagedata") {
				
				string name = reader.GetAttribute ("name");
				content = reader.ReadElementContentAsString ();
				content = content.Replace ("]]", "");
				content = content.Replace ("\n", "");
				contentArr = content.Split ("[[" [0]);
				
				for (int i = 0; i <= contentArr.Length - 1; i++) {
					Node n;
					if (Dialogue.TryGetValue (name.ToLower (), out n)) {
						if (i % 2 == 0 && i > 0) {
							// WEIRD CLEANING
							if (contentArr [i] [contentArr [i].Length - 1].GetHashCode () == 13)
								contentArr [i] = contentArr [i].Substring (0, contentArr [i].Length - 1);
							if (Dialogue.TryGetValue (contentArr [i].ToLower (), out n)) {
								Dialogue [name.ToLower ()].ConnectTo (Dialogue [contentArr [i].ToLower ()]);
							}
						}
					}
				}
			}
		}
			
		Node myNode;
		Dialogue.TryGetValue ("start", out myNode);
		LastNode = myNode;
		UI_Text_Main.GetComponent<TypewritingScript> ().TypeWrite (myNode.content);
		int local = 0;

		foreach (Node n in myNode.GetNext().Values) {

			if (local == 4) {
				UI_Text_Button5.transform.parent.gameObject.SetActive (true);
				UI_Text_Button5.GetComponent<TextRead> ().ShowText_External (n.choiceText);
				UI_Button5.GetComponent<ButtonTrigger> ().SetNextChoice (n.choiceText);
				//Debug.Log ("Max Choices Reached");
			}
			if (local == 3) {
				UI_Text_Button4.transform.parent.gameObject.SetActive (true);
				UI_Text_Button4.GetComponent<TextRead> ().ShowText_External (n.choiceText);
				UI_Button4.GetComponent<ButtonTrigger> ().SetNextChoice (n.choiceText);
				local = 4;
			}
			if (local == 2) {
				UI_Text_Button3.transform.parent.gameObject.SetActive (true);
				UI_Text_Button3.GetComponent<TextRead> ().ShowText_External (n.choiceText);
				UI_Button3.GetComponent<ButtonTrigger> ().SetNextChoice (n.choiceText);
				local = 3;
			}
			if (local == 1) {
				UI_Text_Button2.transform.parent.gameObject.SetActive (true);
				UI_Text_Button2.GetComponent<TextRead> ().ShowText_External (n.choiceText);
				UI_Button2.GetComponent<ButtonTrigger> ().SetNextChoice (n.choiceText);
				local = 2;
			}

			if (local == 0) {
				UI_Text_Button1.transform.parent.gameObject.SetActive (true);
				UI_Text_Button1.GetComponent<TextRead> ().ShowText_External (n.choiceText);
				UI_Button1.GetComponent<ButtonTrigger> ().SetNextChoice (n.choiceText);
				local = 1;
			}
		}
		
	}

	public void HideChoiceButtons ()
	{
		UI_Text_Button1.transform.parent.gameObject.SetActive (false);
		UI_Text_Button2.transform.parent.gameObject.SetActive (false);
		UI_Text_Button3.transform.parent.gameObject.SetActive (false);
		UI_Text_Button4.transform.parent.gameObject.SetActive (false);
		UI_Text_Button5.transform.parent.gameObject.SetActive (false);

	}

	public void ReadNode_Suivant ()
	{
		if (Suivant) {
			Suivant = false;
			ReadNode (Suivant_Node);
		}
	}

	public void ReadNode (string Key)
	{

		HideChoiceButtons ();

		GameManager.GetComponent<StatManager> ().ClearDebugUI ();

		Node myNode;

		if (Suivant) {
			Suivant = false;
			Key = Suivant_Node;
		}

		LastNode.GetNext ().TryGetValue (Key, out myNode);

		UI_Text_Main.GetComponent<TypewritingScript> ().TypeWrite (myNode.content);
		int local = 0;
		LastNode = myNode;


		string Local_StatName;
		string TempStringDeux;
		int Local_StatChangeValue;
		string[] TempArray;

		foreach (string LocalTag in myNode.Tags) {
				
			if (LocalTag.Length > 0) {
				TempArray = LocalTag.Split (":" [0]);
				Local_StatName = TempArray [0];

				if (TempArray [1].Contains ("=")) {
					TempStringDeux = TempArray [1].Replace ("=", "");
				} else {
					TempStringDeux = TempArray [1];
				}
				//Debug.Log (TempStringDeux);
				Local_StatChangeValue = int.Parse (TempStringDeux);
				StatManager.ins.ModifyStatValue (Local_StatName, Local_StatChangeValue);
			} else {
				GameManager.GetComponent<StatManager> ().ChangePersoName (0);
				//Debug.Log ("Empty Tags");
			}
		}
		foreach (Node n in myNode.GetNext().Values) {
			autoCondition = true;
			string CleanText;
			CleanText = n.choiceText;
			int tempBool = 0;
			if (n.choiceText.Contains ("{")) {

				autoCondition = false;

				string output;
				output = n.choiceText.Split (new char[]{ '{', '}' }) [1];

				string[] conditions;
				string[] TempArrayConditions_1;
				string[] tempCleanText;
				List<bool> ListOfBool = new List<bool> ();
				conditions = output.Split (";" [0]);
				tempCleanText = n.choiceText.Split ("{" [0]);
				CleanText = tempCleanText [0];

				foreach (string condition in conditions) {
					string varName;
					string TempString;
					string signe;
					string valueString;
					int value;
					TempArrayConditions_1 = condition.Split (":" [0]);
					varName = TempArrayConditions_1 [0];
					TempString = TempArrayConditions_1 [1];
					signe = TempString.Substring (0, 1);
					valueString = TempString.Substring (1, TempString.Length - 1);
					value = int.Parse (valueString);

					ListOfBool.Add (GameManager.GetComponent<StatManager> ().CompareValue (varName, signe, value));
					//Debug.Log ("Nom var: " + varName + " Signe: " + signe + " Valeur: " + value);
				}

				foreach (bool l in ListOfBool) {
					if (l == false) {
						tempBool++;
						//Debug.Log ("Something is false");
					}
				}
			}
			if (tempBool != 0) {
				autoCondition = false;
			} else {
				autoCondition = true;
			}

			if (CleanText.Contains ("(")) {
				string[] TempArrayCleaner;
				TempArrayCleaner = CleanText.Split ("(" [0]);
				CleanText = TempArrayCleaner [0];
			}
							
			if (local == 4 && autoCondition) {
				UI_Text_Button5.transform.parent.gameObject.SetActive (true);
				UI_Text_Button5.GetComponent<TextRead> ().ShowText_External (CleanText);
				UI_Button5.GetComponent<ButtonTrigger> ().SetNextChoice (n.choiceText);
				//Debug.Log ("Max Choices Reached");
			}
			if (local == 3 && autoCondition) {
				UI_Text_Button4.transform.parent.gameObject.SetActive (true);
				UI_Text_Button4.GetComponent<TextRead> ().ShowText_External (CleanText);
				UI_Button4.GetComponent<ButtonTrigger> ().SetNextChoice (n.choiceText);
				local = 4;
			}
			if (local == 2 && autoCondition) {
				UI_Text_Button3.transform.parent.gameObject.SetActive (true);
				UI_Text_Button3.GetComponent<TextRead> ().ShowText_External (CleanText);
				UI_Button3.GetComponent<ButtonTrigger> ().SetNextChoice (n.choiceText);
				local = 3;
			}
			if (local == 1 && autoCondition) {
				UI_Text_Button2.transform.parent.gameObject.SetActive (true);
				UI_Text_Button2.GetComponent<TextRead> ().ShowText_External (CleanText);
				UI_Button2.GetComponent<ButtonTrigger> ().SetNextChoice (n.choiceText);
				local = 2;
			}

			if (local == 0 && autoCondition) {

				if (n.choiceText.Contains ("Suivant") || n.choiceText.Contains ("suivant")) {
					Suivant = true;
					Suivant_Node = n.choiceText;
					UI_Text_Button1.GetComponent<TextRead> ().ShowText_External ("Suivant");

				} else {
					UI_Text_Button1.GetComponent<TextRead> ().ShowText_External (CleanText);
				}
				UI_Text_Button1.transform.parent.gameObject.SetActive (true);
				UI_Button1.GetComponent<ButtonTrigger> ().SetNextChoice (n.choiceText);

				local = 1;
			}
		}
	}


	// Update is called once per frame
	void Update ()
	{
	
	}
}
