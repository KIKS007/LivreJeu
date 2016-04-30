using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node
{
	private string _choiceText;
	private string _content;
	private string[] _Tags;

	private List<Node> next = new List<Node> ();

	public Node (Node nCopied)
	{
		this._choiceText = nCopied.choiceText;
		this._content = nCopied.content;
		this._Tags = nCopied.Tags;
		this.next = nCopied.next;
	}

	public Node ()
	{

	}


	public void ConnectTo (Node n)
	{
		if (next.IndexOf (n) >= 0)
			return;
		else 
			next.Add (n);
		return;
	}

	public void DisconnectTo (Node n)
	{
		if (next.IndexOf (n) >= 0)
			return;
		else 
			next.Remove (n);
	}

	public string GetChoiceText ()
	{
		return _choiceText;
	}

	public Dictionary<string, Node> GetNext ()
	{
		Dictionary<string, Node> r = new Dictionary<string, Node> ();
		for (int i = 0; i <= next.Count-1; i++) {
			r.Add (next [i].GetChoiceText (), next [i]);
		}

		return r;
	}

	public string content {
		get { return _content; }
		set { _content = value; }
	}

	public string choiceText {
		get { return _choiceText; }
		set { _choiceText = value; }
	}

	public string[] Tags {
		get { return _Tags; }
		set { _Tags = value; }
	}

}
