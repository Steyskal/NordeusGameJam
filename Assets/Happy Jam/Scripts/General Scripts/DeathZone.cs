using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Collider2D))]
public class DeathZone : MonoBehaviour
{
	void OnTriggerEnter2D(Collider2D other)
	{
		Entity entity = other.GetComponent<Entity> ();

		if (entity)
			entity.Die ();
	}
}
