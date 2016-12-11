using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[DisallowMultipleComponent]
public class Entity : MonoBehaviour
{
    [Header("Health")]
    public int InitialHealth = 10;
    public int MaxHealth = 10;

    [Header("Destroying Object")]
    public float DestroyDelay = 0.5f;
    public bool DestroyParent;
    public GameObject EffectPrefabOnDestroy;
    public bool IsPlayerEntity = false;

    [Header("Read-Only")]
    [SerializeField]
    private int _currentHealth;

    [SerializeField]
    public UnityEvent OnEntityDie = new UnityEvent();
    [SerializeField]
    public UnityEvent OnEntityHit = new UnityEvent();

    protected WaitForSeconds _waitDestroy;
    bool _die = false;

    public int CurrentHealth
    {
        get
        {
            return _currentHealth;
        }

        set
        {
            _currentHealth = value <= MaxHealth ? value : MaxHealth;

        }
    }

    protected virtual void Awake()
    {
        CurrentHealth = InitialHealth;
        _waitDestroy = new WaitForSeconds(DestroyDelay);
    }
    /// <summary>
    /// Applies damage on player and returns true if player will die
    /// </summary>
    /// <param name="damage"> Damage dealt to enemy</param>
    /// <param name="comboBonus">Score bonus gained from combo</param>
    /// <returns></returns>
    public virtual bool ApplyDamage(int damage, int comboBonus = 0)
    {
        OnEntityHit.Invoke();
           CurrentHealth -= Mathf.Abs(damage);
        if (_currentHealth <= 0 && !_die)
        {
            if (IsPlayerEntity)
            {
                GameManager.Instance.IsPlayersDead = true;
            }
            OnEntityDie.Invoke();
            Die();
            return true;
        }
        return false;
    }

    public virtual void Die()
    {
        if (EffectPrefabOnDestroy)
        {
            GameObject go = Instantiate(EffectPrefabOnDestroy);
            go.transform.position = transform.position;
        }
        _die = true;
        StartCoroutine(DoDie());
    }
    public virtual IEnumerator DoDie()
    {
        yield return _waitDestroy;
        Debug.Log(gameObject.name + " died.");
        if (DestroyParent)
            Destroy(transform.parent.gameObject);
        else
            Destroy(gameObject);
    }
}
