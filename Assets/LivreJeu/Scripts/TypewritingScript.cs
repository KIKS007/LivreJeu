using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class TypewritingScript : MonoBehaviour 
{
	[Header ("String")]
	[Multiline (10)]
	public string text;
	public string[] textArray;

	[Header ("Typewriting")]
	//Peut être proposer différentes vitesses de lecture ??
	public float letterPause = 0.02f;

	private float letterPauseOriginal;

	[Header ("Wait")]
	public float waitExclamation = 0.5f;
	public float waitQuestion = 0.5f;
	public float waitDots = 0.5f;


	[Header ("Screen Shake")]
	public float shakeDuration = 0.5f;
	public Vector3 shakeStrenth = new Vector3 (1, 1, 0);
	public int shakeVibrato = 100;
	public float shakeRandomness = 45;

	private bool passingDialogue;

	private GameObject canvas;

	// Use this for initialization
	void Awake () 
	{
		DOTween.Init();

		letterPauseOriginal = letterPause;


		//StartCoroutine (Typewriting (text));
	}

	void Start ()
	{
		canvas = transform.parent.gameObject;
	}

	void Update ()
	{
		if(Input.GetKey(KeyCode.Space))
			passingDialogue = true;
		else
			passingDialogue = false;
	}

	public void TypeWrite (string textToType)
	{
		StopAllCoroutines ();
		StartCoroutine(Typewriting (textToType));
	}

	IEnumerator Typewriting (string textToType)
	{
		GetComponent<Text>().text = "";

		textArray = textToType.Split(" " [0]);


		foreach( string array in textArray)
		{
			
			//Add space between words if needed
			if(GetComponent<Text>().text != "")
				GetComponent<Text>().text += " ";

			if(array.Contains("!#"))
			{
				string substring = array.Replace("#", "");
				//Debug.Log(substring);

				foreach (char letter in substring.ToCharArray()) 
				{
					GetComponent<Text>().text += letter;
					yield return new WaitForSeconds (letterPause);
				}

				// If player's not passing dialogue do specific action
				if(!passingDialogue)
				{
					canvas.transform.DOShakePosition(shakeDuration, shakeStrenth, shakeVibrato, shakeRandomness);
					yield return new WaitForSeconds (waitExclamation);
				}

			}

			else if(array.Contains("?#"))
			{
				string substring = array.Replace("#", "");
				//Debug.Log(substring);

				foreach (char letter in substring.ToCharArray()) 
				{
					GetComponent<Text>().text += letter;
					yield return new WaitForSeconds (letterPause);
				}

				if(!passingDialogue)
				{
					canvas.transform.DOShakePosition(shakeDuration, shakeStrenth, shakeVibrato, shakeRandomness);
					yield return new WaitForSeconds (waitQuestion);
				}
			}

			else if(array.Contains(".#"))
			{
				string substring = array.Replace("#", "");
				//Debug.Log(substring);

				foreach (char letter in substring.ToCharArray()) 
				{
					
					GetComponent<Text>().text += letter;

					if(!passingDialogue && letter.ToString() != ".")
						yield return new WaitForSeconds (letterPause);

					if(!passingDialogue && letter.ToString() == ".")
						yield return new WaitForSeconds (waitDots);
				}
			}

			else if(!passingDialogue)
			{
				foreach (char letter in array.ToCharArray()) 
				{
					GetComponent<Text>().text += letter;
					yield return new WaitForSeconds (letterPause);
				}
			}

			else if (passingDialogue)
			{
				GetComponent<Text>().text += array;
				yield return new WaitForSeconds (letterPause);

			}

		}

		yield return null;
	}
}