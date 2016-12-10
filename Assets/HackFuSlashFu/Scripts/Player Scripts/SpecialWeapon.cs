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
	private PlayerController _playerController;

	void Awake ()
	{
		_playerController = GetComponentInParent<PlayerController> ();
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.CompareTag (TargetTag))
			_playerController.AddEnemyForSpecialAttak (other.GetComponent<Enemy>());
	}

	void OnTriggerExit2D (Collider2D other)
	{
		if (other.CompareTag (TargetTag))
			_playerController.RemoveEnemyForSpecialAttack (other.GetComponent<Enemy>());
	}

	void OnDrawGizmos ()
	{
		CircleCollider2D circleCollider2D = GetComponent<CircleCollider2D> ();

		GizmosExtension.DrawCircle (transform.position, circleCollider2D.radius, Color.blue, 32);
	}
}
