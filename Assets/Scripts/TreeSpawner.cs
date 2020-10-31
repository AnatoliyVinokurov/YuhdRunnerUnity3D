using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSpawner : MonoBehaviour
{

	public GameObject Tree1Obj;
	public GameObject Tree2Obj;
	public int TreeChance;

	// Use this for initialization
	void Start ()
	{
		if (Random.Range (0, 101) <= TreeChance) {
			Tree1Obj.SetActive (true);
			Tree2Obj.SetActive (false);
		} else {
			Tree1Obj.SetActive (false);
			Tree2Obj.SetActive (true);
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
