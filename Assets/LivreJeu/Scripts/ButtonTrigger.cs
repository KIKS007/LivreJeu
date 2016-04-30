using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ButtonTrigger : MonoBehaviour
{
	public GameObject TextManager;

	string NextChoice;

	public void ReadChoice ()
	{
		TextManager.GetComponent<TwineParser> ().ReadNode (NextChoice);

	}

	public void SetNextChoice (string Key)
	{
		NextChoice = Key;
	}



}
