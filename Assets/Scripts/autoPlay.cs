using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class autoPlay : MonoBehaviour {

	GameObject cm;
	// Use this for initialization
	void Start () {
		cm = GameObject.Find ("ContentManager");
	}
	
	// Update is called once per frame
	void Update () {
		if (gameObject.GetComponent<VideoPlaybackBehaviour> ().CurrentState == VideoPlayerHelper.MediaState.READY
		    && gameObject.GetComponent<VideoPlaybackBehaviour> ().CurrentState != VideoPlayerHelper.MediaState.PLAYING
		    && gameObject.GetComponent<VideoPlaybackBehaviour> ().CurrentState != VideoPlayerHelper.MediaState.PAUSED) {
			transform.localScale = new Vector3 (cm.GetComponent<ContentManager> ().scaleX, cm.GetComponent<ContentManager> ().scaleY, cm.GetComponent<ContentManager> ().scaleZ);
			transform.localPosition = new Vector3 (cm.GetComponent<ContentManager> ().positionX, cm.GetComponent<ContentManager> ().positionY, cm.GetComponent<ContentManager> ().positionZ);
			gameObject.GetComponent<VideoPlaybackBehaviour> ().enabled = true;
			gameObject.GetComponent<VideoPlaybackBehaviour> ().VideoPlayer.Play (false, 0);
		}
	}
}
