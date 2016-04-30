using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class StatManager : MonoBehaviour
{

	public static StatManager ins;

	public GameObject DebugUI_Text;
	public GameObject PersoNameUI_Text;
	Text PersoNameUI_TextComponent;
	Text DebugUI_TexteComponent;

	public string[] StatisticsListName;

	public string[] Perso_Name;
	public string CurrentPersonnage;

	public List<Statistics> StatisticsList = new List<Statistics> ();


	// Use this for initialization
	void Awake ()
	{
		if (StatManager.ins == null) {
			ins = this;
		} else {
			Destroy (gameObject);
		}
	}

	void Start ()
	{
		DebugUI_TexteComponent = DebugUI_Text.GetComponent<Text> ();
		PersoNameUI_TextComponent = PersoNameUI_Text.GetComponent<Text> ();
		Initialisation (StatisticsListName);
	}

	void Initialisation (string[] CreateStats)
	{
		foreach (string StatName in StatisticsListName) {
			StatisticsList.Add (new Statistics (StatName, 0));
		}

	}

	public void ClearDebugUI ()
	{
		DebugUI_TexteComponent.text = "";
	}

	public void ChangePersoName (int Perso_Number)
	{
		CurrentPersonnage = Perso_Name [Perso_Number];
		if (Perso_Number == 0) {
			PersoNameUI_TextComponent.text = "";
		} else {
			PersoNameUI_TextComponent.text = CurrentPersonnage;
			//Debug.Log (CurrentPersonnage);
		}

	}

	public bool CompareValue (string name, string signe, int value)
	{
		if (signe == ">") {
			foreach (Statistics C in StatisticsList) {
				if (C.Name == name) {
					if (C.Value > value) {
						return true;
					} else {
						return false;
					}
				}
			}	
		}
		if (signe == "<") {
			foreach (Statistics C in StatisticsList) {
				if (C.Name == name) {
					if (C.Value < value) {
						return true;
					} else {
						return false;
					}
				}
			}	
		}
		if (signe == "=") {
			foreach (Statistics C in StatisticsList) {
				if (C.Name == name) {
					if (C.Value == value) {
						return true;
					} else {
						return false;
					}
				}
			}	
		}
		Debug.Log ("From StatManager: Signe is not correct ?");
		return false;
	}


	public void ModifyStatValue (string Name, int Value)
	{
		if (Name == "Name") {
			ChangePersoName (Value);
		} else {
			ChangePersoName (0);
			foreach (Statistics T in StatisticsList) {

				if (T.Name == Name) {
					if (Name == "InstantOrc") {
						DebugUI_TexteComponent.text += "INSTANT ORC";
						//Lancer Instant ORC

					} else {
						T.Value += Value;
						DebugUI_TexteComponent.text += (" Statistic Name: " + T.Name + " Statistic Value: " + T.Value);
					}
				}
			}
		}
	}


}
