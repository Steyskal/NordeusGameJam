using AI;
using Happy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[DisallowMultipleComponent]
public class EnemyMelee : Enemy
{
    public enum States
    {
        Pursue,
        Attack
    }
    public float SpawnDelay = 0.5f;
    public float AttackDamageCheckDelay = 0.1f;
    public float AttackDuration = 1f;
    public float AttackDistance = 2f;
    //public OnCollider2DEvents ColliderEvents;
    public FaceForward FaceForward;

    private States _state = States.Pursue;
    private WaitForSeconds _waitAttack;
    private WaitForSeconds _waitCheckAttack;
    private WaitForSeconds _waitStartDelay;

    private List<Transform> _toAttack = new List<Transform>();

    protected override void Awake()
    {
        base.Awake();
        _waitAttack = new WaitForSeconds(AttackDuration);
        _waitCheckAttack = new WaitForSeconds(AttackDamageCheckDelay);
        _waitStartDelay = new WaitForSeconds(SpawnDelay);
        Debug.Log(transform.eulerAngles);
    }

    public override void Start()
    {
        base.Start();
        /*ColliderEvents.OnTriggerEnterTransformEvent.AddListener(OnTargetEnter);
        ColliderEvents.OnTriggerExitTransformEvent.AddListener(OnTargetExit);
        ColliderEvents.OnTriggerStayEvent.AddListener(OnTriggerStay);*/
        StartCoroutine(StartPursue());

    }
    protected override void OnSetBehaviorTarget(GameObject target)
    {
        Behaviour.target = target;
        FaceForward.target = target;
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.transform.tag == "Player")
        {
            Entity entity = col.gameObject.GetComponentInChildren<Entity>();

            if (entity)
                entity.ApplyDamage(1);
        }
    }
    protected override void OnPlayerDie()
    {
        base.OnPlayerDie();
        FaceForward.enabled = false;
    }

    #region Not used - For Weapon
    void OnTriggerStay()
    {
        StartCoroutine(Attack());
    }

    void OnTargetEnter(Transform target)
    {
        if (!_toAttack.Contains(target))
            _toAttack.Add(target);
    }
    void OnTargetExit(Transform target)
    {
        if (_toAttack.Contains(target))
            _toAttack.Remove(target);
    }
    /// <summary>
    /// Attacks Player (if he is in PolygonCollider range)
    /// Killes player if he is also inside distance
    /// </summary>
    /// <returns></returns>
    IEnumerator Attack()
    {
        if (_state != States.Attack)
        {
            if (_toAttack.Count > 0)
            {
                _state = States.Attack;
                Behaviour.enabled = false;
                FaceForward.enabled = false;
                yield return _waitCheckAttack;

                float distance = Vector3.Distance(Behaviour.target.transform.position, transform.position);
                if (distance > AttackDistance)
                {
                    foreach (Transform t in _toAttack)
                    {
                        Entity e = t.GetComponent<Entity>();
                        if (e)
                        {
                            e.ApplyDamage(1);
                            Debug.Log("Attacked " + e);
                        }
                    }
                    Debug.Log("Attacked");
                }
                OnAttackEvent.Invoke();

                yield return _waitAttack;
            }
            yield return Pursue();
        }
        yield break;
    }

    #endregion
    IEnumerator StartPursue()
    {
        yield return _waitStartDelay;
        yield return Pursue();
    }
    IEnumerator Pursue()
    {
        _state = States.Pursue;
        Behaviour.enabled = true;
        FaceForward.enabled = true;
        yield break;
    }
    protected override void OnAfterKnockback(bool behaviorEnabled)
    {
        base.OnAfterKnockback(behaviorEnabled);
        Behaviour.enabled = false;
        FaceForward.enabled = false;
    }
    protected override void OnBeforeKnockback(bool behaviorEnabled)
    {
        base.OnBeforeKnockback(behaviorEnabled);
        Behaviour.enabled = true;
        FaceForward.enabled = true;
    }

}
