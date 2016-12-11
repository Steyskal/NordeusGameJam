using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Happy;

public class GameplayUIManager : MonoSingleton<GameplayUIManager>
{
	[Header ("Tutorial Properites")]
	public GameObject IntroMouse;
	public GameObject ComboMouse;

	[Header ("Combo Properties")]
	public PlayerController Player;
	public Slider ComboSlider;
	public Animator SigilAnimator;
	public Animator FillAnimator;

	[Header ("Pause Properties")]
	public KeyCode PauseKey = KeyCode.Escape;
	public Canvas PauseCanvas;

	[Header ("GameOver Properties")]
	public Canvas GameOverCanvas;
	public Text ScoreText;
	public Text ComboText;
	public Text EnemyText;

	[Header ("Read-Only")]
	[SerializeField]
	private AudioListener _audioListener;

	private int ShouldDisplayTutorial = 1;

	void Awake ()
	{
//		ShouldDisplayTutorial = PlayerPrefs.GetInt ("ShouldDisplayTutorial", 1);
		ShouldDisplayTutorial = 1;
		IntroMouse.SetActive (ShouldDisplayTutorial == 1 ? true : false);

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
		if (Input.GetKeyDown (KeyCode.Mouse0))
			IntroMouse.SetActive (false);

		if (Input.GetKeyDown (PauseKey))
			TogglePause ();

		if (Input.GetKeyDown (KeyCode.Mouse1) && ComboMouse.activeSelf)
		{
			ComboMouse.SetActive (false);
			PlayerPrefs.SetInt ("ShouldDisplayTutorial", 0);
		}
			
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

		if (ComboSlider.value == 0)
		{
			SigilAnimator.SetBool ("ComboOpportunity", false);
			FillAnimator.SetBool ("ComboOpportunity", false);
		}
			

		if (ComboSlider.value == ComboSlider.maxValue)
		{
			SigilAnimator.SetBool ("ComboOpportunity", true);
			FillAnimator.SetBool ("ComboOpportunity", true);

			if (ShouldDisplayTutorial == 1)
				ComboMouse.SetActive (true);
		}
			
	}

	public void OnGameOverEventListener ()
	{
		Invoke ("GameOver", 1.0f);
	}

	private void GameOver()
	{
		GameOverCanvas.enabled = true;

		ScoreText.text = GameManager.Instance.Score.ToString ();
		ComboText.text = GameManager.Instance.ComboCounter.ToString ();
		EnemyText.text = GameManager.Instance.EnemyKillCount.ToString ();
	}

	public void Restart ()
	{
		SceneManagerExtension.ReloadScene ();
	}

	public void LoadMainMenu ()
	{
		Time.timeScale = 1.0f;
		SceneManager.LoadScene (0);
	}
}
