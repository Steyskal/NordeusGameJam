using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Happy;

[DisallowMultipleComponent]
[RequireComponent (typeof(Collider2D))]
public class SpecialWeapon : MonoBehaviour
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
			_playerWeaponController.AddEnemyForSpecialAttak (other.gameObject);
	}

	void OnTriggerExit2D (Collider2D other)
	{
		if (other.CompareTag (TargetTag))
			_playerWeaponController.RemoveEnemyForSpecialAttack (other.gameObject);
	}

	void OnDrawGizmos ()
	{
		CircleCollider2D circleCollider2D = GetComponent<CircleCollider2D> ();

		GizmosExtension.DrawCircle (transform.position, circleCollider2D.radius, Color.blue, 32);
	}
}
