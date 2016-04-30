using UnityEngine;
using System.Collections;
using System;

public class Statistics : IComparable<Statistics> {

	public string Name;
	public int Value;

	public Statistics(string newName, int newValue){
		Name = newName;
		Value = newValue;
	}

	public int CompareTo(Statistics other){
	
		if (other == null) {
			return 1;
		}

		return Value - other.Value;

	}

}
