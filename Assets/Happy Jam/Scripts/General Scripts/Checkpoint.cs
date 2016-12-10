using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Collider2D))]
public class Checkpoint : MonoBehaviour
{
	public string PlayerTag = "Player";

	[Header("Read-Only")]
	[SerializeField]
	private bool _isActive = false;

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag (PlayerTag) && !_isActive)
			Activate ();
	}

	private void Activate()
	{
		_isActive = true;
		GameManager.Instance.CurrentCheckpoint = this;

		Debug.Log (gameObject.name + " activated.");
	}

	public void Deactivate()
	{
		_isActive = false;

		Debug.Log (gameObject.name + " deactivated.");
	}
}
