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
    [Tooltip("Used only for repel")]
    public float AttackDuration = 1.5f;

    [Header("Knockback Properties")]
    public float KnockbackForce = 100.0f;

	public CustomUnityEvent OnAttackEvent = new CustomUnityEvent();

    [Header("Special Attack Properties")]
    public float SpecialAttackModeDuration = 2.5f;
	public AudioSource SpecialAttackAudioSource;

    [Header("Combo Properties")]
    public int NeededCombo = 200;
    public float ComboResetTimer = 1.0f;
    public float ComboOpportunityTime = 2.0f;

	[Header("Audio Properties")]
	public List<AudioClip> AttackAudioClips = new List<AudioClip>();
	private AudioSource _audioSource;

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
    [HideInInspector]
    public bool IsAttacking = false;

    public List<Enemy> EnemiesToAttack
    {
        get
        {
            return _enemiesToAttack;
        }
    }

    [SerializeField]
    private List<Enemy> _enemiesToSpecialAttack = new List<Enemy>();

    private Transform _transform;

	private Animator _animator;

    void Awake()
    {
        IsAttacking = false;
        _transform = transform;
		_audioSource = GetComponentInChildren<AudioSource> ();
		_animator = GetComponentInChildren<Animator> ();
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
    void SetIsAttacking(float duration)
    {
        StartCoroutine(SetIsAttackingForDuration(duration));
    }
    IEnumerator SetIsAttackingForDuration(float duration)
    {
        IsAttacking = true;
        yield return new WaitForSeconds(duration);
        IsAttacking = false;
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

		GameManager.Instance.ComboCounter = _comboCounter;

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
        SetIsAttacking(AttackDuration);
        _audioSource.PlayOneShot (AttackAudioClips [Random.Range (0, AttackAudioClips.Count)]);

        OnAttackEvent.Invoke();
        //		Debug.Log ("Attack");
        int newComboCount = _enemiesToAttack.Count;

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

		_enemiesToAttack.RemoveAll(enemy => enemy == null);
		_enemiesToSpecialAttack.RemoveAll(enemy => enemy == null);

		if (newComboCount != 0)
		{
			_comboOpportunityTimer = 0.0f;
			IncreaseCombo(newComboCount);
		}
    }

    private void StartSpecialAttackMode()
    {
        //		Debug.Log ("SpecialAttackModeOn");
		SpecialAttackAudioSource.Play();
        SetIsAttacking(SpecialAttackModeDuration);

        _hasComboOpportunity = false;
        _isInSpecialAttackMode = true;
		_animator.SetBool ("IsCombo", _isInSpecialAttackMode);

        List<Enemy> enemiesToRemove = new List<Enemy>();

        for (int i = 0; i < _enemiesToSpecialAttack.Count; i++)
        {
            //			Debug.Log (_enemiesToSpecialAttack [i].name + " killed.");

            KnockbackTarget(_enemiesToSpecialAttack[i]);

            if (_enemiesToSpecialAttack[i].ApplyDamage(999, _comboCounter))
                enemiesToRemove.Add(_enemiesToSpecialAttack[i]);
        }

        for (int i = 0; i < enemiesToRemove.Count; i++)
        {
            _enemiesToAttack.Remove(enemiesToRemove[i]);
            _enemiesToSpecialAttack.Remove(enemiesToRemove[i]);
        }

		_enemiesToSpecialAttack.RemoveAll(enemy => enemy == null);

        Invoke("EndSpecialAttackMode", SpecialAttackModeDuration);
    }

    private void EndSpecialAttackMode()
    {
        //		Debug.Log ("SpecialAttackModeOff");
		SpecialAttackAudioSource.Stop();
        ResetCombo();

        _isInSpecialAttackMode = false;
		_animator.SetBool ("IsCombo", _isInSpecialAttackMode);
    }

    public void AddEnemyForSpecialAttak(Enemy enemy)
    {
        if (_isInSpecialAttackMode)
        {
            //			Debug.Log (enemy.name + " killed.");
			enemy.ApplyDamage(999, _comboCounter);
        }
        else
            _enemiesToSpecialAttack.Add(enemy);
    }

    public void RemoveEnemyForSpecialAttack(Enemy enemy)
    {
        _enemiesToSpecialAttack.Remove(enemy);
    }
}
