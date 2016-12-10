using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Happy;
using System;
using UnityEngine.UI;

[RequireComponent(typeof(AI.Agent))]
[RequireComponent(typeof(Animator))]
public class AnimationController : MonoBehaviour
{
    public string SpeedFloatVar = "Speed";
    public string AttackBoolVar = "IsAttacking";
    public string ComboBoolVar = "IsCombo";

    private AI.Agent _agent;
    private Animator _animator;
    void Awake()
    {
        _agent = GetComponent<AI.Agent>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        _animator.SetFloat(SpeedFloatVar, _agent.GetVelocity().magnitude);
    }

    public void SetAttack(bool attack)
    {
        _animator.SetBool(AttackBoolVar, attack);
    }
    public void SetCombo(bool combo)
    {
        _animator.SetBool(ComboBoolVar, combo);
    }
    
}
