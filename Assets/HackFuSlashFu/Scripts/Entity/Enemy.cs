﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[DisallowMultipleComponent]
public class Enemy : Entity
{
    //TODO Score System

    public Happy.CustomUnityEvent<int> OnEnemyComboBonus = new Happy.CustomUnityEvent<int>();


    public virtual void Start()
    {
        OnEnemyComboBonus.AddListener(delegate (int value) { GameManager.Instance.AddScore(value, GameManager.ScoreBonusType.ComboBonus); });
    }
    /// <summary>
    /// Applies damage on player and returns true if player will die
    /// Also Invokes event for OnComboBonus and for OnEntityDie
    /// </summary>
    /// <param name="damage"> Damage dealt to enemy</param>
    /// <param name="comboBonus">Score bonus gained from combo</param>
    /// <returns></returns>
    public override bool ApplyDamage(int damage, int comboBonus = 0)
    {
        if (comboBonus > 0)
            OnEnemyComboBonus.Invoke(comboBonus);

        bool enemyWillDie = base.ApplyDamage(damage);
        if (enemyWillDie) GameManager.Instance.AddScore(1, GameManager.ScoreBonusType.EnemyKill);
        return enemyWillDie;
    }

}