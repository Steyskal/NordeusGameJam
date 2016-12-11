using UnityEngine;
using System;
using System.Collections;

public class CameraAspectEnforcer : MonoBehaviour
{
	[Serializable]
	public struct AspectRatio
	{
		public float widthAspect;
		public float heightAspect;
	}
	
	public AspectRatio aspectRatio;
	
	void Awake()
	{
		float targetAspect = aspectRatio.widthAspect / aspectRatio.heightAspect;
		float windowAspect = (float)Screen.width / (float)Screen.height;
		float scaleHeight = windowAspect / targetAspect;
		Camera camera = GetComponent<Camera>();
		
		if (scaleHeight < 1.0f)
		{
			camera.orthographicSize = camera.orthographicSize / scaleHeight;
		}
	}
}
