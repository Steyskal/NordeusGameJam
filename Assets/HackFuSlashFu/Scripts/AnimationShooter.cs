using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationShooter : MonoBehaviour
{
	[Header("Read-Only")]
	[SerializeField]
	private EnemyShooter _enemyShooter;

	void Awake()
	{
		_enemyShooter = GetComponentInParent<EnemyShooter> ();
	}

	public void Shoot()
	{
		_enemyShooter.Shoot();
	}
}
