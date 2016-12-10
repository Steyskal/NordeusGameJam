using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System;

[Serializable]
public class TransformEvent: UnityEvent<Transform> { }

[RequireComponent(typeof(Collider2D))]
public class OnCollider2DEvents : MonoBehaviour
{
    [SerializeField]
    public UnityEvent OnTriggerEnterEvent = new UnityEvent();
    [SerializeField]
    public TransformEvent OnTriggerEnterTransformEvent = new TransformEvent();
    [SerializeField]
    public UnityEvent OnTriggerStayEvent = new UnityEvent();
    [SerializeField]
    public UnityEvent OnTriggerExitEvent = new UnityEvent();
    [SerializeField]
    public TransformEvent OnTriggerExitTransformEvent = new TransformEvent();
    [SerializeField]
    public UnityEvent OnCollisionEnterEvent = new UnityEvent();
    [SerializeField]
    public UnityEvent OnCollisionStayEvent = new UnityEvent();
    [SerializeField]
    public UnityEvent OnCollisionExitEvent = new UnityEvent();

    public LayerMask LayerMask;
    public float TriggerStayFrequency = float.PositiveInfinity;
    public float DelayOnExit = 3f;


    private float _lastWrittenTimeCol = 0f;
    private float _lastWrittenTimeTrig = 0f;

    #region Trigger Events
    void OnTriggerEnter2D(Collider2D col)
    {
        if (IsInLayerMask(col))
        {
            //Debug.Log("OnTriggerEnter");
            OnTriggerEnterEvent.Invoke();
            OnTriggerEnterTransformEvent.Invoke(col.transform);
        }
    }
    void OnTriggerStay2D(Collider2D col)
    {
        if (IsInLayerMask(col))
        {
            if (_lastWrittenTimeTrig < 1 || Time.time - _lastWrittenTimeTrig > TriggerStayFrequency)
            {
                //Debug.Log("OnTriggerStay");
                OnTriggerStayEvent.Invoke();
                _lastWrittenTimeTrig = Time.time;
            }
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (IsInLayerMask(col))
        {
            Invoke("InvokedTriggerExit", DelayOnExit);
            OnTriggerExitTransformEvent.Invoke(col.transform);
        }
    }
    void InvokedTriggerExit()
    {
        Debug.Log("OnTriggerExit");
        OnTriggerExitEvent.Invoke();
        _lastWrittenTimeTrig = 0;
    }
    #endregion

    #region Collision Events
    void OnCollisionEnter2D(Collision2D col)
    {
        if (IsInLayerMask(col.collider))
        {
            Debug.Log("OnCollisionEnter");
            OnCollisionEnterEvent.Invoke();
        }
    }
    void OnCollisionStay2D(Collision2D col)
    {
        if (IsInLayerMask(col.collider))
        {
            if (_lastWrittenTimeCol < 1 || Time.time - _lastWrittenTimeCol > TriggerStayFrequency)
            {
                OnCollisionStayEvent.Invoke();
                _lastWrittenTimeCol = Time.time;
            }
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (IsInLayerMask(col.collider))
        {
            Invoke("InvokedCollisionExit", DelayOnExit);
        }
    }

    void InvokedCollisionExit()
    {
        Debug.Log("OnCollisionExit");
        OnCollisionExitEvent.Invoke();
        _lastWrittenTimeCol = 0;
    }

    #endregion

    bool IsInLayerMask(Collider2D col)
    {
        return LayerMask == (LayerMask | (1 << col.gameObject.layer));
    }

}
