using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class DynamicText : MonoBehaviour 
{
	public float minWidth;
	public float maxWidth;

	public float adjustedWidth;
	public float adjustedHeight;

	public float timeToInstantiate;

	public GameObject cobayeText;

	public GameObject textPrefab;

	public GameObject canvasParent;

	void Start ()
	{
		CreateText (cobayeText.GetComponent<Text> ().text, new Vector3 (2, 1, 5));
	}

	public void CreateText (string text, Vector3 textPosition)
	{
		//Find Size of the rect
		cobayeText.GetComponent<Text> ().text = "";
		cobayeText.GetComponent<Text> ().text = text;

		cobayeText.GetComponent<RectTransform> ().sizeDelta = new Vector2(Random.Range (minWidth, maxWidth), cobayeText.GetComponent<RectTransform> ().sizeDelta.y) ;

		adjustedWidth = cobayeText.GetComponent<RectTransform> ().rect.width;
		adjustedHeight = cobayeText.GetComponent<Text> ().preferredHeight;

		StartCoroutine (InstantiateText (text, textPosition));
	}

	IEnumerator InstantiateText (string text, Vector3 textPosition)
	{
		GameObject instantiatedText = (Instantiate (textPrefab, textPosition, textPrefab.transform.rotation) as GameObject);
		instantiatedText.transform.SetParent (canvasParent.transform);

		//Debug.Log(instantiatedText.transform.position);

		instantiatedText.GetComponent<RectTransform> ().sizeDelta = new Vector2(0, 0);
		Tween myTween = instantiatedText.GetComponent<RectTransform> ().DOSizeDelta(new Vector2(adjustedWidth, adjustedHeight), timeToInstantiate) ;

		yield return myTween.WaitForCompletion ();

		instantiatedText.GetComponent<TypewritingScript> ().TypeWrite (text);
	}
		
}
