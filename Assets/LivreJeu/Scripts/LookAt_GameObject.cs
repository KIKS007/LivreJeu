using UnityEngine;
using System.Collections;

public class LookAt_GameObject : MonoBehaviour
{

	[SerializeField]
	private GameObject
		Object;

	// Update is called once per frame
	void Update ()
	{
		transform.LookAt (2*transform.position - Object.transform.position);

	}
}
