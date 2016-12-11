using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuBackgroundMusic : MonoBehaviour
{
	public Vector2 ClipInterval;
	public List<AudioClip> AudioClips = new	List<AudioClip> ();

	private AudioSource _audioSource;

	void Awake ()
	{
		_audioSource = GetComponent<AudioSource> ();

		StartCoroutine (PlayClip ());
	}

	IEnumerator PlayClip ()
	{
		while(true)
		{
			yield return new WaitForSeconds (Random.Range (ClipInterval.x, ClipInterval.y));

			_audioSource.PlayOneShot (AudioClips [Random.Range (0, AudioClips.Count)]);
		}
	}
}
