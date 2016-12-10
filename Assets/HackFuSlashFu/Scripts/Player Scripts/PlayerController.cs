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
	private List<GameObject> _enemiesToAttack = new List<GameObject> ();

	public List<GameObject> EnemiesToAttack
	{
		get
		{
			return _enemiesToAttack;
		}
	}

	[SerializeField]
	private List<GameObject> _enemiesToSpecialAttack = new List<GameObject> ();

	void Update ()
	{
		if (Input.GetKeyDown (AttackInput) && !_isInSpecialAttackMode)
			Attack ();

		if (Input.GetKeyDown (SpecialAttackInput) && (_comboCounter >= NeededCombo))
			StartSpecialAttackMode ();

		if (_comboTimer >= ComboResetTimer && !_hasComboOpportunity)
		{
			_comboCounter = 0;
			Debug.Log ("Combo reset.");
		}
		else
		{
			_comboTimer += Time.deltaTime;
		}
	}

	private void IncreaseCombo ()
	{
		_comboCounter += _enemiesToAttack.Count;
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

		IncreaseCombo ();

		for (int i = 0; i < _enemiesToAttack.Count; i++)
		{
			Debug.Log (_enemiesToAttack [i].name + " killed.");
		}
	}

	private void StartSpecialAttackMode ()
	{
		Debug.Log ("SpecialAttackModeOn");

		_isInSpecialAttackMode = true;
		_comboCounter = 0;

		for (int i = 0; i < _enemiesToSpecialAttack.Count; i++)
		{
			Debug.Log (_enemiesToSpecialAttack [i].name + " killed.");
		}

		Invoke ("EndSpecialAttackMode", SpecialAttackModeDuration);
	}

	private void EndSpecialAttackMode ()
	{
		Debug.Log ("SpecialAttackModeOff");

		_isInSpecialAttackMode = false;
	}

	public void AddEnemyForSpecialAttak (GameObject enemy)
	{
		if (_isInSpecialAttackMode)
			Debug.Log (enemy.name + " killed.");
		else
			_enemiesToSpecialAttack.Add (enemy);
	}

	public void RemoveEnemyForSpecialAttack (GameObject enemy)
	{
		_enemiesToSpecialAttack.Remove (enemy);
	}
}
