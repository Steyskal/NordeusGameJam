using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Collider2D))]
public class Coin : MonoBehaviour
{
	public string CollectorTag = "Player";
	public int Value = 5;

	[Header ("Audio Properties")]
	public AudioClip PickupSFX;
	[Range (0.0f, 1.0f)]
	public float AudioVolume = 0.25f;

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.CompareTag (CollectorTag))
		{
			GameManager.Instance.CoinScore += Value;
			AudioSource.PlayClipAtPoint (PickupSFX, transform.position, AudioVolume);

			Destroy (gameObject);
		}
	}
}
