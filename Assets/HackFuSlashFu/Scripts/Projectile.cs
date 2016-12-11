using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	public int Damage = 1;

	void OnCollisionEnter2D(Collision2D other)
	{
		Entity entity = other.gameObject.GetComponentInChildren<Entity> ();

		if (entity)
			entity.ApplyDamage (1);

		Destroy (gameObject);
	}
}
