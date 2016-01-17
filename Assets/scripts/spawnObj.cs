using UnityEngine;
using System.Collections;

public class spawnObj : MonoBehaviour {

	private Vector3 startPos;
	private float newXpos;
	public float moveSpeed = 1.0f;
	public float moveDist = 3.0f;
	public GameObject[] randomBlocks;

	//Time for spawns
	public float nextSpawnInterval = 4.0f;
	public float startTime = 0.0f;
	public float timerLeft = 0.0f;

	void Start () {
		startPos = transform.position;
	}

	void spawnMe() {
		int whichBlock = Random.Range (0, randomBlocks.Length);
		GameObject newObj = Instantiate (randomBlocks[whichBlock]) as GameObject;
		newObj.transform.position = transform.position;
	}


	// Update is called once per frame
	void Update () {
		newXpos = Mathf.PingPong (Time.time * moveSpeed, moveDist) - (moveDist / 2.0f);
		transform.position = new Vector3 (newXpos, startPos.y, startPos.z);

		//can we spawn more?
		timerLeft = Time.time - startTime;
		if (timerLeft > nextSpawnInterval) {
			startTime = Time.time;
			timerLeft = 0.0f;
			GameObject[] numberOfBlocks = GameObject.FindGameObjectsWithTag ("powerup");
			int platCount = numberOfBlocks.Length;
			numberOfBlocks = GameObject.FindGameObjectsWithTag ("enemy");
			platCount += numberOfBlocks.Length;
		//	Debug.Log ("Total items on screen now: "+platCount);
			if(platCount<3) {
				spawnMe();
				platCount = 0;
			}

		}
	
	}
}
