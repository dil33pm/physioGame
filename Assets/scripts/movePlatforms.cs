using UnityEngine;
using System.Collections;

public class movePlatforms : MonoBehaviour {

	private float moveSpeed;
    float minSpeed = 1.0f;
    float maxSpeed = 3.0f;
    parallax cloudSpeed;

    // Use this for initialization
    void Start () {
		moveSpeed = Random.Range(minSpeed, maxSpeed);
        cloudSpeed = GameObject.Find("clouds").GetComponent<parallax>();
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate (0, -(moveSpeed * Time.deltaTime), 0);
		if (transform.position.y < -4.5f) {
			Destroy (gameObject);
		}
	}

	void OnBecameInvisible(){
		Destroy (gameObject);
	}

    void OnTriggerEnter2D (Collider2D col)
    {
        if(col.name == "player" && gameObject.tag == "enemy")
        {
            Destroy(gameObject);
            Debug.Log("Deleted " + gameObject.name);
            if (cloudSpeed.offsetSpeed > 0)
                cloudSpeed.offsetSpeed -= 0.05f;
            else
                cloudSpeed.offsetSpeed = 0f;

            maxSpeed -= 0.5f;
            if(maxSpeed <= minSpeed)
            {
                maxSpeed = minSpeed = 0f;
                cloudSpeed.offsetSpeed = 0f;
                Time.timeScale = 0f;
            }


        }

        if (col.name == "player" && gameObject.tag == "powerup")
        {
            Debug.Log("Deleted " + gameObject.name);
            Destroy(gameObject);
            minSpeed = 1.0f;
            maxSpeed = 3.0f;
            cloudSpeed.offsetSpeed = 0.2f;
        }
    }
}
