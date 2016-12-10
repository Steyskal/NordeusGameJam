using AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[DisallowMultipleComponent]
public class EnemyShooter : Enemy
{
    public enum States
    {
        Shooting
    }
    
    public float ShotForce = 100f;
    public float ShotDelay = 1f;
    public GameObject BulletPrefab;
    public Face FaceBehavior;


    private States _state = States.Shooting;
    private WaitForSeconds _waitDelay;
    protected override void Awake()
    {
        base.Awake();
        _waitDelay = new WaitForSeconds(ShotDelay);
    }

    public override void Start()
    {
        base.Start();
        Behaviour.enabled = false;
        StartCoroutine(Shot());
    }
    protected override void OnSetBehaviorTarget(GameObject target)
    {
        base.OnSetBehaviorTarget(target);
        FaceBehavior.target = target;
        FaceBehavior.enabled = true;
    }
    IEnumerator Shot()
    {
        GameObject bullet = Instantiate(BulletPrefab);
        bullet.transform.position = transform.position + transform.right;
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(transform.right * ShotForce, ForceMode2D.Impulse);
        
        yield return _waitDelay;
        yield return Shot();
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
