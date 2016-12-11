using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Happy;

public class GameplayUIManager : MonoSingleton<GameplayUIManager>
{
	[Header ("Combo Properties")]
	public PlayerController Player;
	public Slider ComboSlider;

	[Header ("Pause Properties")]
	public KeyCode PauseKey = KeyCode.Escape;
	public Canvas PauseCanvas;

	[Header("GameOver Properties")]
	public Canvas GameOverCanvas;

	[Header ("Read-Only")]
	[SerializeField]
	private AudioListener _audioListener;

	void Awake ()
	{
		_audioListener = Camera.main.GetComponent<AudioListener> ();

		PauseCanvas.enabled = false;
		GameOverCanvas.enabled = false;

		ComboSlider.minValue = 0;
		ComboSlider.maxValue = Player.NeededCombo;

		Player.OnComboCounterChangedEvent.AddListener (OnComboChangedEventListener);
		GameManager.Instance.OnGameOverEvent.AddListener (OnGameOverEventListener);
	}

	void Update ()
	{
		if (Input.GetKeyDown (PauseKey))
			TogglePause ();
	}

	public void ToggleMute ()
	{
		_audioListener.enabled = !_audioListener.enabled;
	}

	public void TogglePause ()
	{
		Time.timeScale = Time.timeScale == 1.0f ? 0.0f : 1.0f;
		PauseCanvas.enabled = !PauseCanvas.enabled;
	}

	public void OnComboChangedEventListener (int newCombo)
	{
		ComboSlider.value = newCombo;
	}

	public void OnGameOverEventListener()
	{
		GameOverCanvas.enabled = true;
	}

	public void Restart()
	{
		SceneManagerExtension.ReloadScene ();
	}
}
