using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Happy;

[DisallowMultipleComponent]
public class UIManager : MonoSingleton<UIManager>
{
	public Canvas MainMenuCanvas;

	void Awake()
	{
		if (Instance != this)
			Destroy (gameObject);
	}

	public void PlayGame()
	{
		MainMenuCanvas.enabled = false;

		SceneManagerExtension.LoadNextScene();
	}

	public void QuitGame()
	{
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#endif

		Application.Quit ();
	}
}
