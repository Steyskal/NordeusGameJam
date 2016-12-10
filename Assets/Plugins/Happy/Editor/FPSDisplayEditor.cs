/*
 *
 *	Happy Framework
 *	by Toni Steyskal, 2016-2017
 * 
 */

using UnityEngine;

using UnityEditor;

namespace Happy
{
	[CustomEditor (typeof(FPSDisplay))]
	public class FPSDisplayEditor : Editor
	{
		private FPSDisplay _fpsDisplay;
		private RectTransform _fpsCanvasGroup;

		void Awake ()
		{
			_fpsDisplay = (FPSDisplay)target;
			_fpsCanvasGroup = _fpsDisplay.GetComponentInChildren<CanvasGroup> ().GetComponent<RectTransform> ();

			//TODO: Somehow add a listener to EditorPrefs change or Scene "anything" change
			UpdateExtremeValues ();
		}

		public override void OnInspectorGUI ()
		{
			EditorGUI.BeginChangeCheck ();

			if (!EditorApplication.isPlaying)
			{
				string buttonString = EditorPrefs.GetBool ("FPSExtremeValues") ? "Hide Extreme Values" : "Show Extreme Values";

				if (GUILayout.Button (buttonString))
				{
					EditorPrefs.SetBool ("FPSExtremeValues", !EditorPrefs.GetBool ("FPSExtremeValues")); 
				}
			}

			if (EditorGUI.EndChangeCheck ())
			{
				Undo.RecordObject (target, "Changed Area Of Effect");

				UpdateExtremeValues ();
			}

			DrawDefaultInspector ();
		}

		private void UpdateExtremeValues()
		{
			_fpsDisplay.HighestFPSText.transform.parent.gameObject.SetActive (EditorPrefs.GetBool ("FPSExtremeValues"));
			_fpsDisplay.LowestFPSText.transform.parent.gameObject.SetActive (EditorPrefs.GetBool ("FPSExtremeValues"));

			Vector2 newSize = new Vector2 ();
			newSize.x = _fpsCanvasGroup.sizeDelta.x;
			newSize.y = EditorPrefs.GetBool ("FPSExtremeValues") ? 60 : 20;

			_fpsCanvasGroup.sizeDelta = newSize;
		}
	}
}
