using AI;
using Happy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[DisallowMultipleComponent]
public class Enemy : Entity
{
    //TODO Score System
    public Happy.CustomUnityEvent<int> OnEnemyComboBonus = new Happy.CustomUnityEvent<int>();
    public AgentBehaviour Behaviour;
    public CustomUnityEvent OnAttackEvent = new CustomUnityEvent();
    public float KnockBackDuration = 0.6f;

    protected Agent2D _agent;
    protected Rigidbody2D _rb;

    protected override void Awake()
    {
        base.Awake();
        _agent = GetComponent<Agent2D>();
        _rb = GetComponent<Rigidbody2D>();
    }
    public virtual void Start()
    {
        OnEnemyComboBonus.AddListener(delegate (int value) { GameManager.Instance.AddScore(value, GameManager.ScoreBonusType.ComboBonus); });
        OnSetBehaviorTarget(GameManager.Instance.GetPlayer(transform.position));
    }
    protected virtual void OnSetBehaviorTarget(GameObject target)
    {
        Behaviour.target = target;
        Behaviour.enabled = true;
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

    private bool _isKnockbacked = false;
    public void AddKnockBack(Vector3 force)
    {
        if (_isKnockbacked) return;
        _isKnockbacked = true;
        _agent.velocity = Vector3.zero;
        _rb.AddForce(force, ForceMode2D.Impulse);
        /*Steering steering = new Steering();
        steering.linear = force;
        _agent.SetSteering(steering,10);*/
        StartCoroutine(KnockBackPostEffect(Behaviour.enabled));
    }

    protected IEnumerator KnockBackPostEffect(bool behaviorEnabled)
    {
        OnBeforeKnockback(behaviorEnabled);
        yield return new WaitForSeconds(KnockBackDuration);
        OnAfterKnockback(behaviorEnabled);
    }
    protected virtual void OnBeforeKnockback(bool behaviorEnabled)
    {
        if (behaviorEnabled) Behaviour.enabled = false;
    }
    protected virtual void OnAfterKnockback(bool behaviorEnabled)
    {
        _rb.velocity = Vector3.zero;
        Behaviour.enabled = behaviorEnabled;
        _isKnockbacked = false;
    }

}
