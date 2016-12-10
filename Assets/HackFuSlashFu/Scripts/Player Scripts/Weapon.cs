using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent (typeof(Collider2D))]
public class Weapon : MonoBehaviour
{
	public string TargetTag = "Enemy";

	[Header ("Read-Only")]
	[SerializeField]
	private PlayerController _playerController;

	void Awake ()
	{
		_playerController = GetComponentInParent<PlayerController> ();
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.CompareTag (TargetTag))
			_playerController.EnemiesToAttack.Add (other.GetComponent<Enemy>());
	}

	void OnTriggerExit2D (Collider2D other)
	{
		if (other.CompareTag (TargetTag))
			_playerController.EnemiesToAttack.Remove (other.GetComponent<Enemy>());
	}
}
