using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class RoadSpawner : MonoBehaviour
{

	public GameObject[] RoadBlockPrefabs;
	public GameObject StartBlock;
	
	float startBlockZPos = 0, 
		currentBlockZPos = 0;
	int blocksCount = 7;
	float blockLength = 0;
	int safeZone = 280;

	
	public Transform PlayerTranf;
	List<GameObject> CurrentBlocks = new List<GameObject> ();

	Vector3 startPlayerPos;

	void Start ()
	{
		startBlockZPos = StartBlock.transform.position.z;
		blockLength = StartBlock.GetComponent<BoxCollider> ().bounds.size.z;
		startPlayerPos = PlayerTranf.position;


		StartTheGame ();
	}

	public void StartTheGame ()
	{
		currentBlockZPos = startBlockZPos;
		PlayerTranf.position = startPlayerPos;
		foreach (var go in CurrentBlocks)
			Destroy (go);
		CurrentBlocks.Clear ();

		//CurrentBlocks.Add (StartBlock);
		for (int i = 0; i < blocksCount; i++) {
			SpawnBlock ();
		}
	}

	void Update ()
	{
		CheckForSpawn ();
	}

	void CheckForSpawn ()
	{
		if (PlayerTranf.position.z - safeZone > (currentBlockZPos - blocksCount * blockLength)) {
			SpawnBlock ();
			DestroyBlock ();
		}
			
	}

	void SpawnBlock ()
	{
		GameObject block = Instantiate (RoadBlockPrefabs [Random.Range (0, RoadBlockPrefabs.Length)], transform);
		
		currentBlockZPos += blockLength;
		
		block.transform.position = new Vector3 (0, 0, currentBlockZPos);
		
		CurrentBlocks.Add (block);
	}

	void DestroyBlock ()
	{
		Destroy (CurrentBlocks [0].gameObject);
		CurrentBlocks.RemoveAt (0);
	}
}
