/*==============================================================================
Copyright (c) 2016 PTC Inc. All Rights Reserved.
 
Copyright (c) 2012-2015 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
==============================================================================*/
using UnityEngine;
using System.Collections;
using Vuforia;
using System;

/// <summary>
/// This class manages the content displayed on top of cloud reco targets in this sample
/// </summary>
public class ContentManager : MonoBehaviour, ITrackableEventHandler
{
	#region MEMBERS
	/// <summary>
	/// The root gameobject that serves as an augmentation for the image targets created by search results
	/// </summary>
	public GameObject cloudTarget, goVid;
	private string[] nameObjectFile, nameObject;
	public Creature2 jsMeta;
	private GameObject go = null;
	public float scaleX, scaleY, scaleZ, positionX, positionY, positionZ;

	#endregion //MEMBERS

	#region MONOBEHAVIOUR_METHODS
	void Start ()
	{
		TrackableBehaviour trackableBehaviour = cloudTarget.GetComponent<TrackableBehaviour>();
		if (trackableBehaviour)
		{
			trackableBehaviour.RegisterTrackableEventHandler(this);
		}

		ShowObject(false);
	}

	#endregion MONOBEHAVIOUR_METHODS


	#region PUBLIC_METHODS
	/// <summary>
	/// Implementation of the ITrackableEventHandler function called when the
	/// tracking state changes.
	/// </summary>
	public void OnTrackableStateChanged(
		TrackableBehaviour.Status previousStatus,
		TrackableBehaviour.Status newStatus)
	{
		if (newStatus == TrackableBehaviour.Status.DETECTED ||
			newStatus == TrackableBehaviour.Status.TRACKED || 
			newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
		{
			ShowObject(true);
		}
		else
		{
			ShowObject(false);
		}
	}

	public void ShowObject(bool tf)
	{
		if (tf) {
			string url = GameObject.Find ("CloudRecognition").GetComponent<screh>().url;
			jsMeta = JsonUtility.FromJson<Creature2>(url); 
			nameObjectFile = jsMeta.url.Split(new char[] { '/' }); 
			nameObject = nameObjectFile[nameObjectFile.Length - 1].Split(new char[] { '.' }); 
			if (nameObject [1] == "mp4") {
				StartCoroutine (SetUpVideo ());
			} else {
				StartCoroutine(DownloadAndCache()); 
			}
		}
		else {
			if(go != null){
				go.GetComponent<VideoPlaybackBehaviour> ().VideoPlayer.Pause();
				go.GetComponent<VideoPlaybackBehaviour> ().enabled = false;
				go.GetComponent<MeshRenderer> ().enabled = false;
			}
		}
	}
	#endregion //PUBLIC_METHODS
	IEnumerator SetUpVideo(){
		float pos = 0;
		if(go == null){
			go = Instantiate(goVid); 
			go.name = "obj"; 
			go.transform.parent = cloudTarget.transform;
			pos = 0;
		}
		if (go.transform.GetComponent<VideoPlaybackBehaviour> ().m_path == jsMeta.url) {
			pos = go.GetComponent<VideoPlaybackBehaviour> ().VideoPlayer.GetCurrentPosition ();
		} else {
			Destroy (GameObject.Find ("obj"));
			go = Instantiate(goVid); 
			go.name = "obj"; 
			go.transform.parent = cloudTarget.transform;
			go.transform.GetComponent<VideoPlaybackBehaviour> ().m_path = jsMeta.url;
			pos = 0;
		}
		scaleX = Single.Parse (jsMeta.width) * -0.1f;
		scaleY = 0.1f;
		scaleZ = Single.Parse (jsMeta.height) * 0.1f;
		positionX = Single.Parse (jsMeta.left);
		positionY = 0.01f;
		positionZ = Single.Parse (jsMeta.top);
		go.transform.localScale = new Vector3 (Single.Parse (jsMeta.width) * -0.1f, 0.1f, Single.Parse (jsMeta.height) * 0.1f); 
		go.transform.localPosition = new Vector3 (Single.Parse (jsMeta.left), 0.01f, Single.Parse (jsMeta.top));
		go.GetComponent<MeshRenderer> ().enabled = true;
		go.GetComponent<VideoPlaybackBehaviour> ().enabled = true;
		yield return null; // new WaitForSeconds (1);
		//goVid.GetComponent<VideoPlaybackBehaviour> ().VideoPlayer.Play (false, pos);
	}

	IEnumerator DownloadAndCache() 
	{ 
		Destroy (GameObject.Find("obj"));
		while (!Caching.ready) 
			yield return null; 

		using (WWW www = WWW.LoadFromCacheOrDownload(jsMeta.url, 1)) 
		{ 
			yield return www; 

			if (www.error != null) 
				throw new UnityException("WWW Download had an error: " + www.error); 

			AssetBundle bundle = www.assetBundle; 
			Debug.Log(bundle);
			GameObject go = Instantiate(bundle.LoadAsset(nameObject[1]) as GameObject); 
			go.name = "obj"; 
			go.transform.parent = cloudTarget.transform; 
			go.transform.localScale = new Vector3(Single.Parse(jsMeta.width), Single.Parse(jsMeta.height), Single.Parse(jsMeta.height)); 
			go.transform.localPosition = new Vector3(Single.Parse(jsMeta.left), Single.Parse(jsMeta.top), Single.Parse(jsMeta.top)); 
			bundle.Unload(false); 
		} 
	}
}

[System.Serializable]
public class Creature2 
{ 
	public string url; 
	public string width; 
	public string height; 
	public string top; 
	public string left; 
}