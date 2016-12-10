using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;
using UnityEngine.Events;

public class Window : MonoBehaviour
{
    public Canvas Canvas;
    public bool UseGameObjectInstead = false;
    public bool Opened = false;
    public bool CloseFirstTimeWithoutEvents = false;

    [SerializeField]
    public UnityEvent OnOpen = new UnityEvent();
    [SerializeField]
    public UnityEvent OnClose = new UnityEvent();

    protected virtual void Start()
    {
        Open(Opened);
        CloseFirstTimeWithoutEvents = false;
    }

    public void Open(bool open)
    {
        if (!UseGameObjectInstead)
        {
            Canvas.enabled = open;
        }
        else
        {
            this.gameObject.SetActive(open);
        }
        if (open)
        {
            OnWindowOpen();
        }
        else
        {
            OnWindowClose();
        }
        Opened = open;
    }
    public virtual void OnWindowOpen()
    {
        if (!CloseFirstTimeWithoutEvents)
            OnOpen.Invoke();
    }
    public virtual void OnWindowClose()
    {
        if (!CloseFirstTimeWithoutEvents)
            OnClose.Invoke();
    }
}

