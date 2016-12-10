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
	private PlayerWeaponController _playerWeaponController;

	void Awake ()
	{
		_playerWeaponController = GetComponentInParent<PlayerWeaponController> ();
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.CompareTag (TargetTag))
			_playerWeaponController.EnemiesToAttack.Add (other.gameObject);
	}

	void OnTriggerExit2D (Collider2D other)
	{
		if (other.CompareTag (TargetTag))
			_playerWeaponController.EnemiesToAttack.Remove (other.gameObject);
	}
}
