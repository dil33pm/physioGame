using UnityEngine;
using System.Collections;

public class parallax : MonoBehaviour {

	public float offsetSpeed = 0.2f;
	public float offset;
	public Renderer txRend;

	// Use this for initialization
	void Start () {
		try {
		txRend = GetComponent<Renderer>();
		}
		catch (UnityException e)
		{
			Debug.Log ("Cannot find renderer "+e.Message);
		}
	}
	
	// Update is called once per frame
	void Update () {
		offset = Time.time * offsetSpeed;
		txRend.material.SetTextureOffset ("_MainTex", new Vector2(0, offset));
	}
}