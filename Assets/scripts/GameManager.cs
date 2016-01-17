using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public GoogleAnalyticsV3 GA;
    Text score;
    int distance = 1;

	// Use this for initialization
	void Start () {
        score = GameObject.Find("score").GetComponent<Text>();
        InvokeRepeating("addDistance", 3f, 5f);
        GA.LogScreen("Secondary Menu");/*
        GA.LogTiming("Session", 50L, "Play screen", "isPlaying");
        GA.LogEvent(new EventHitBuilder()
.SetEventCategory("Achievement")
.SetEventAction("Max Distance")
.SetEventLabel("Highest score")
.SetEventValue(distance));*/

    }

    // Update is called once per frame
    void Update () {
       score.text = "Distance " + distance.ToString()+ " Time "+ Time.time.ToString("F0") + " Sec";
    }

    void addDistance ()
    {
        distance += 2;
    }
}
