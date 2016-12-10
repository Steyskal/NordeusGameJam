using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Happy;

public class GameManager : MonoSingleton<GameManager>
{
	[Header ("Score System")]
	public int Score = 0;

	[Header ("Checkpoint System")]
	[SerializeField]
	private Checkpoint _currentCheckpoint;

	public Checkpoint CurrentCheckpoint
	{
		get
		{
			return _currentCheckpoint;
		}

		set
		{
			if (_currentCheckpoint)
				_currentCheckpoint.Deactivate ();
			
			_currentCheckpoint = value;
		}
	}

	[Header ("Read-Only")]
	[SerializeField]
	private int _coinsCollected = 0;
	[SerializeField]
	private int _coinScore = 0;

	public int CoinScore
	{
		get
		{
			return _coinScore;
		}

		set
		{
			_coinScore += value;
			_coinsCollected++;
		}
	}
}
