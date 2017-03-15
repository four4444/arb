using System.Collections;
using System;
using UnityEngine;

namespace Vuforia 
{
	public class screh : MonoBehaviour, ICloudRecoEventHandler 
	{ 
		private CloudRecoBehaviour mCloudRecoBehaviour; 
		private ObjectTracker mObjectTracker; 
		public string url; 

		void Start() 
		{ 
			CloudRecoBehaviour cloudRecoBehaviour = GetComponent<CloudRecoBehaviour>(); 
			if(cloudRecoBehaviour) 
			{ 
				cloudRecoBehaviour.RegisterEventHandler(this); 
			} 
			mCloudRecoBehaviour = cloudRecoBehaviour; 
		} 

		public void OnInitError(TargetFinder.InitState initError) 
		{ 
		} 
 
		public void OnInitialized() 
		{ 
			mObjectTracker = (ObjectTracker)TrackerManager.Instance.GetTracker<ObjectTracker>(); 
		} 

		public void OnNewSearchResult(TargetFinder.TargetSearchResult targetSearchResult) 
		{ 
			url = targetSearchResult.MetaData; 
		} 

		public void OnStateChanged(bool scanning) 
		{ 
		} 

		public void OnUpdateError(TargetFinder.UpdateState updateError) 
		{ 
		}
	}
}