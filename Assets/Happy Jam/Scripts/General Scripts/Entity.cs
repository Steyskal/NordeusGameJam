using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Entity : MonoBehaviour
{
	public int InitialHealth = 10;
	public int MaxHealth = 10;

	[Header ("Read-Only")]
	[SerializeField]
	private int _currentHealth;

	public int CurrentHealth
	{
		get
		{
			return _currentHealth;
		}

		set
		{
			_currentHealth = value <= MaxHealth ? value : MaxHealth;

			if (_currentHealth <= 0)
				Die ();
		}
	}

	void Awake ()
	{
		CurrentHealth = InitialHealth;
	}

	public void Die ()
	{
		Debug.Log (gameObject.name + " died.");

		Destroy (gameObject);
	}
}
