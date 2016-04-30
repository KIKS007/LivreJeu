using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class TextRead : MonoBehaviour
{

	[SerializeField]
	private bool
		Animated = true;

	bool RandomInProgress = true;

	string charsetLowerCase = "abcdefghijklmnopqrstuvwxyz";
	string Word;
	string Letter;
	string inp_ln;

	Text TexteUI;



	public TextAsset FileToRead;
	int TextLength;

	private TypewritingScript typewritingScript;

	// Use this for initialization
	void Awake ()
	{
		TexteUI = this.GetComponent<Text> ();
		//ReadText ();
		if (Animated) {
			StartCoroutine ("AnimatedText");
		}

		typewritingScript = GetComponent<TypewritingScript>();
	}
	


	public void ReadText ()
	{

		/*
		StreamReader inp_stm = new StreamReader (file);
		
		while (!inp_stm.EndOfStream) {
			inp_ln = inp_stm.ReadLine ();

			TextLength = inp_ln.Length;

			//ShowText (inp_ln);

		}
		inp_stm.Close ();  
		*/

		inp_ln = FileToRead.text;
		TextLength = inp_ln.Length;
		//ShowText ();

	}

	public void ShowText ()
	{
		RandomInProgress = false;
		TexteUI.text = inp_ln;

	}

	public void ShowText_External (string Texte)
	{
		RandomInProgress = false;
		TexteUI.text = Texte;
	}

	public void StopShowText ()
	{
		RandomInProgress = true;
	}


	public string GetRandomWord (int length)
	{
		Word = "";
		for (int i = 0; i < length; i++) {
			Letter = (charsetLowerCase [(int)Random.Range (0, 26)]).ToString ();
			Word = Word + Letter;
		}
		return Word;
	}

	IEnumerator AnimatedText ()
	{
		while (true) {
			if (RandomInProgress == true) {
				string TexteTemp = GetRandomWord (TextLength);
				TexteUI.text = TexteTemp;
			}

			yield return new WaitForSeconds (.1f);
		}
	}
}