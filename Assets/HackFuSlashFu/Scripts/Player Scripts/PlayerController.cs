using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Happy;

[DisallowMultipleComponent]
public class PlayerController : MonoBehaviour
{
    [Header("Input Axis Properties")]
    public KeyCode AttackInput = KeyCode.Mouse0;
    public KeyCode SpecialAttackInput = KeyCode.Mouse1;

    [Header("Knockback Properties")]
    public float KnockbackForce = 100.0f;

    [Header("Special Attack Properties")]
    public float SpecialAttackModeDuration = 2.5f;

    [Header("Combo Properties")]
    public int NeededCombo = 200;
    public float ComboResetTimer = 1.0f;
    public float ComboOpportunityTime = 2.0f;

    public OnCollider2DEvents BulletTriggerEvents;

	public CustomUnityEvent OnAttackEvent = new CustomUnityEvent();

    [Header("Read-Only")]
    [SerializeField]
    private int _comboCounter = 0;

	[HideInInspector]
    public CustomUnityEvent<int> OnComboCounterChangedEvent = new CustomUnityEvent<int>();

    [SerializeField]
    private float _comboTimer = 0.0f;
    [SerializeField]
    private float _comboOpportunityTimer = 0.0f;

    [SerializeField]
    private bool _hasComboOpportunity = false;
    [SerializeField]
    private bool _isInSpecialAttackMode = false;
    [SerializeField]
    private List<Enemy> _enemiesToAttack = new List<Enemy>();
    private List<Bullet> _bullets = new List<Bullet>();

    public List<Enemy> EnemiesToAttack
    {
        get
        {
            return _enemiesToAttack;
        }
    }
    public List<Bullet> Bullets
    {
        get
        {
            return _bullets;
        }
    }

    [SerializeField]
    private List<Enemy> _enemiesToSpecialAttack = new List<Enemy>();

    private Transform _transform;

    void Awake()
    {
        _transform = transform;
    }
    void Start()
    {
        //BulletTriggerEvents.On
    }

    void Update()
    {
        if (Input.GetKeyDown(AttackInput) && !_isInSpecialAttackMode)
            Attack();

        if (Input.GetKeyDown(SpecialAttackInput) && (_comboCounter >= NeededCombo))
            StartSpecialAttackMode();

        if (_comboTimer >= ComboResetTimer && !_hasComboOpportunity)
        {
            ResetCombo();
            //			Debug.Log ("Combo reset.");
        }
        else if (!_isInSpecialAttackMode)
        {
            _comboTimer += Time.deltaTime;
        }

        if (_hasComboOpportunity && !_isInSpecialAttackMode)
        {
            _comboOpportunityTimer += Time.deltaTime;

            if (_comboOpportunityTimer >= ComboOpportunityTime)
                _hasComboOpportunity = false;
        }
    }

    private void ResetCombo()
    {
        _comboCounter = 0;

        OnComboCounterChangedEvent.Invoke(_comboCounter);
    }

    private void IncreaseCombo(int amount)
    {
        _comboCounter += amount;
        _comboTimer = 0;

        OnComboCounterChangedEvent.Invoke(_comboCounter);

        //		Debug.Log ("Combo " + _comboCounter);

        if (_comboCounter >= NeededCombo)
            _hasComboOpportunity = true;
    }

    private void KnockbackTarget(Enemy enemy)
    {
        if (enemy != null)
        {
            Vector2 direction = enemy.transform.position - _transform.position;
            enemy.AddKnockBack(direction * KnockbackForce);
        }
    }

    private void Attack()
    {
        OnAttackEvent.Invoke();
        List < Bullet> _bulletsTmp = _bullets;
        for(int i = 0; i < _bulletsTmp.Count; i++)
        {
            if (_bulletsTmp[i] != null)
            {
                Rigidbody2D rb = _bulletsTmp[i].GetComponent<Rigidbody2D>();
                _bulletsTmp[i].ExtendDuration();
                rb.AddForce(transform.right * 10, ForceMode2D.Impulse);
            }
            else
            {
                _bullets.Remove(_bulletsTmp[i]);
            }
        }


        //		Debug.Log ("Attack");
        int newComboCount = _enemiesToAttack.Count;

        if (newComboCount != 0)
            _comboOpportunityTimer = 0.0f;

        List<Enemy> enemiesToRemove = new List<Enemy>();

        for (int i = 0; i < _enemiesToAttack.Count; i++)
        {
            //			Debug.Log (_enemiesToAttack [i].name + " killed.");

            KnockbackTarget(_enemiesToAttack[i]);

            if (_enemiesToAttack[i].ApplyDamage(1, _comboCounter))
                enemiesToRemove.Add(_enemiesToAttack[i]);
        }

        for (int i = 0; i < enemiesToRemove.Count; i++)
        {
            _enemiesToAttack.Remove(enemiesToRemove[i]);
            _enemiesToSpecialAttack.Remove(enemiesToRemove[i]);
        }

        IncreaseCombo(newComboCount);
    }

    private void StartSpecialAttackMode()
    {
        //		Debug.Log ("SpecialAttackModeOn");

        _hasComboOpportunity = false;
        _isInSpecialAttackMode = true;

        List<Enemy> enemiesToRemove = new List<Enemy>();

        for (int i = 0; i < _enemiesToSpecialAttack.Count; i++)
        {
            //			Debug.Log (_enemiesToSpecialAttack [i].name + " killed.");

            KnockbackTarget(_enemiesToSpecialAttack[i]);

            if (_enemiesToSpecialAttack[i].ApplyDamage(1, _comboCounter))
                enemiesToRemove.Add(_enemiesToSpecialAttack[i]);
        }

        for (int i = 0; i < enemiesToRemove.Count; i++)
        {
            _enemiesToAttack.Remove(enemiesToRemove[i]);
            _enemiesToSpecialAttack.Remove(enemiesToRemove[i]);
        }

        Invoke("EndSpecialAttackMode", SpecialAttackModeDuration);
    }

    private void EndSpecialAttackMode()
    {
        //		Debug.Log ("SpecialAttackModeOff");

        ResetCombo();

        _isInSpecialAttackMode = false;
    }

    public void AddEnemyForSpecialAttak(Enemy enemy)
    {
        if (_isInSpecialAttackMode)
        {
            //			Debug.Log (enemy.name + " killed.");
            enemy.ApplyDamage(1, _comboCounter);
        }
        else
            _enemiesToSpecialAttack.Add(enemy);
    }

    public void RemoveEnemyForSpecialAttack(Enemy enemy)
    {
        _enemiesToSpecialAttack.Remove(enemy);
    }
}
