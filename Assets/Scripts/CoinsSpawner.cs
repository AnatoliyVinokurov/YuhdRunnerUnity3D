using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsSpawner : MonoBehaviour
{

	public GameObject CoinsObj;
	public int CoinChance;

	// Use this for initialization
	void Start ()
	{
		CoinsObj.SetActive (Random.Range (0, 101) <= CoinChance);
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
