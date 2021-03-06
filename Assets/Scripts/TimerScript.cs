﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimerScript : MonoBehaviour {

	public float timeLeft;
	
	[HideInInspector]
	public int minutes;
	[HideInInspector]
	public int seconds;
	public Text timeShown;

    private GameObject endScreen;
	private float nextInstensify; 
	private const float delaybetweenIntesifies = 30; 

	private bool musicIntensified = false;

	void Start() {
		minutes = Mathf.FloorToInt(timeLeft/60);
		seconds = Mathf.CeilToInt(timeLeft%60);
		string countdownString = seconds < 10 ?  ":0" : ":";
		timeShown.text =  minutes + countdownString + seconds;
        endScreen = GameObject.FindGameObjectWithTag("EndGameScreen");
        endScreen.SetActive(false);
		nextInstensify = timeLeft - delaybetweenIntesifies;
	}
	
	// Update is called once per frame
	void Update () {
		timeLeft -= Time.deltaTime;

		minutes = Mathf.FloorToInt(timeLeft/60);
		seconds = Mathf.CeilToInt(timeLeft%60);
		string countdownString = seconds < 10 ?  ":0" : ":";
		timeShown.text =  minutes + countdownString + seconds;
		
		if (timeLeft < 0f) {
			timeShown.text = "0:00";
            // endScreen.SetActive(true);
            endScreen.SetActive(true);
            endScreen.GetComponent<EndGame>().loseGame();
		}

		
		if (timeLeft <= 30f && !musicIntensified) {
			FindObjectOfType<AudioManager> ().IntensifyGameThemeByTimer ();
			musicIntensified = true;
		} else if (timeLeft <= nextInstensify) {
			nextInstensify = timeLeft - delaybetweenIntesifies;
			FindObjectOfType<AudioManager> ().IntensifyGameThemeByTrap ();
		}
	}

}
