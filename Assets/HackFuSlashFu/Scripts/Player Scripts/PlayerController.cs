using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerController : MonoBehaviour
{
	[Header ("Input Axis Properties")]
	public KeyCode AttackInput = KeyCode.Mouse0;
	public KeyCode SpecialAttackInput = KeyCode.Mouse1;

	[Header ("Special Attack Properties")]
	public float SpecialAttackModeDuration = 5.0f;

	[Header ("Combo Properties")]
	public int NeededCombo = 10;
	public float ComboResetTimer = 1.0f;
	public float ComboOpportunityTime = 2.0f;

	[Header ("Read-Only")]
	[SerializeField]
	private int _comboCounter = 0;
	[SerializeField]
	private float _comboTimer = 0.0f;
	[SerializeField]
	private bool _hasComboOpportunity = false;
	[SerializeField]
	private bool _isInSpecialAttackMode = false;
	[SerializeField]
	private List<Enemy> _enemiesToAttack = new List<Enemy> ();

	public List<Enemy> EnemiesToAttack
	{
		get
		{
			return _enemiesToAttack;
		}
	}

	[SerializeField]
	private List<Enemy> _enemiesToSpecialAttack = new List<Enemy> ();

	void Update ()
	{
		if (Input.GetKeyDown (AttackInput) && !_isInSpecialAttackMode)
			Attack ();

		if (Input.GetKeyDown (SpecialAttackInput) && (_comboCounter >= NeededCombo))
			StartSpecialAttackMode ();

		if (_comboTimer >= ComboResetTimer && !_hasComboOpportunity)
		{
			_comboCounter = 0;
//			Debug.Log ("Combo reset.");
		}
		else
		{
			_comboTimer += Time.deltaTime;
		}
	}

	private void IncreaseCombo (int amount)
	{
		_comboCounter += amount;
		_comboTimer = 0;

		Debug.Log ("Combo " + _comboCounter);

		if (_comboCounter >= NeededCombo)
		{
			_hasComboOpportunity = true;

			Invoke ("RemoveComboOpportunity", ComboOpportunityTime);

			Debug.Log ("Has combo opportunity.");
		}
	}

	private void RemoveComboOpportunity ()
	{
		_hasComboOpportunity = false;
	}

	private void Attack ()
	{
		Debug.Log ("Attack");

		int newComboCount = _enemiesToAttack.Count;

		for (int i = 0; i < _enemiesToAttack.Count; i++)
		{
			Debug.Log (_enemiesToAttack [i].name + " killed.");
			_enemiesToAttack [i].ApplyDamage (1, _comboCounter);
		}

		IncreaseCombo (newComboCount);
	}

	private void StartSpecialAttackMode ()
	{
		Debug.Log ("SpecialAttackModeOn");

		_isInSpecialAttackMode = true;
		_comboCounter = 0;

		for (int i = 0; i < _enemiesToSpecialAttack.Count; i++)
		{
			Debug.Log (_enemiesToSpecialAttack [i].name + " killed.");
			_enemiesToSpecialAttack [i].ApplyDamage (1, _comboCounter);
		}

		Invoke ("EndSpecialAttackMode", SpecialAttackModeDuration);
	}

	private void EndSpecialAttackMode ()
	{
		Debug.Log ("SpecialAttackModeOff");

		_isInSpecialAttackMode = false;
	}

	public void AddEnemyForSpecialAttak (Enemy enemy)
	{
		if (_isInSpecialAttackMode)
		{
			Debug.Log (enemy.name + " killed.");
			enemy.ApplyDamage (1, _comboCounter);
		}
		else
			_enemiesToSpecialAttack.Add (enemy);
	}

	public void RemoveEnemyForSpecialAttack (Enemy enemy)
	{
		_enemiesToSpecialAttack.Remove (enemy);
	}
}
