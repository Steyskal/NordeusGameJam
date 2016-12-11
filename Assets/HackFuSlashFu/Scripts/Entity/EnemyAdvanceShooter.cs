using AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[DisallowMultipleComponent]
public class EnemyAdvanceShooter : Enemy
{
    public enum States
    {
        Shooting,
        Pursue
    }

    public float ShotForce = 100f;
    public float FirstShootDelay = 1f;
    public float ShotDelay = 1f;
    public GameObject BulletPrefab;
    public Face FaceBehavior;

    private States _state = States.Pursue;
    protected WaitForSeconds _waitDelay;
    protected WaitForSeconds _waitFirstDelay;
    protected bool _enableShoot = false;
    protected GameObject _target;
    protected override void Awake()
    {
        base.Awake();
        _waitDelay = new WaitForSeconds(ShotDelay);
        _waitFirstDelay = new WaitForSeconds(FirstShootDelay);
    }

    public override void Start()
    {
        base.Start();
        Behaviour.enabled = false;
    }
    protected override void OnSetBehaviorTarget(GameObject target)
    {
        base.OnSetBehaviorTarget(target);
        FaceBehavior.target = target;
        Behaviour.target = target;
        FaceBehavior.enabled = true;
        _target = target;
    }
    private bool _shooting = false;
    void Update()
    {
        if (Vector3.Distance(_target.transform.position, transform.position) > 1)
        {
            FaceBehavior.enabled = true;
            Behaviour.enabled = true;
        }
        else if (!_shooting)
        {
            StartCoroutine(Shot());
        }
    }
    IEnumerator Shot()
    {
        _shooting = true;
        yield return _waitDelay;
        Shoot();
        yield return Shot();

    }

    protected override void OnPlayerDie()
    {
        base.OnPlayerDie();
        FaceBehavior.enabled = false;
        _enableShoot = false;
        Debug.Log("Player dead " + this);
    }

    public void Shoot()
    {
        GameObject bullet = Instantiate(BulletPrefab);
        bullet.transform.position = transform.position + transform.right;
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(transform.right * ShotForce, ForceMode2D.Impulse);
    }

    protected override void OnAfterKnockback(bool behaviorEnabled)
    {
        base.OnAfterKnockback(behaviorEnabled);
        FaceBehavior.enabled = true;
    }
    protected override void OnBeforeKnockback(bool behaviorEnabled)
    {
        base.OnBeforeKnockback(behaviorEnabled);
        FaceBehavior.enabled = false;
    }

}
