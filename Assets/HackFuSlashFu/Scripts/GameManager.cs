using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Happy;
using System;
[Serializable]
public class CustomIntEvent : Happy.CustomUnityEvent<int> { }
public class GameManager : MonoSingleton<GameManager>
{
    [Header("Score System")]
    public int Score = 0;

    public enum ScoreBonusType
    {
        EnemyKill,
        ComboBonus
    }

    [Header("Score Contribution")]
    [Tooltip("Define one for each ScoreBonusType")]
    public ScoreValuesInformation[] ScoreValues = new ScoreValuesInformation[2];

    [Header("Checkpoint System")]
    [SerializeField]
    private Checkpoint _currentCheckpoint;
    private Dictionary<ScoreBonusType, ScoreValuesInformation> _scoreValuesDict = new Dictionary<ScoreBonusType, ScoreValuesInformation>();

    public CustomIntEvent OnScoreChange = new CustomIntEvent();

    public Checkpoint CurrentCheckpoint
    {
        get
        {
            return _currentCheckpoint;
        }

        set
        {
            if (_currentCheckpoint)
                _currentCheckpoint.Deactivate();

            _currentCheckpoint = value;
        }
    }

    #region Coins
    [Header("Read-Only")]
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
    #endregion

    void Reset()
    {
        ScoreValues[0] = new ScoreValuesInformation(ScoreBonusType.EnemyKill, 1);
        ScoreValues[1] = new ScoreValuesInformation(ScoreBonusType.ComboBonus, 1);
    }
    void Awake()
    {
        foreach (ScoreValuesInformation s in ScoreValues)
        {
            _scoreValuesDict.Add(s.Type, s);
        }
    }

    public void AddScore(int value, ScoreBonusType type)
    {
        Score += _scoreValuesDict[type].Value * value;
        _scoreValuesDict[type].CurrentCount++;
        OnScoreChange.Invoke(Score);
    }

    [Serializable]
    public class ScoreValuesInformation
    {
        public ScoreBonusType Type;
        [Tooltip("Multiplier for value")]
        public int Value;
        [HideInInspector]
        public int CurrentCount = 0;
        public ScoreValuesInformation(ScoreBonusType type, int value)
        {
            Type = type;
            Value = value;
        }
    }
}
