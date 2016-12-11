using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Happy;
using System;
using UnityEngine.UI;
using DigitalRuby.Tween;

public class TweenShake : MonoBehaviour
{
    public Vector3 Offset1;
    public Vector3 Offset2;
    public Vector3 Offset3;
    public Vector3 Offset4;

    public float DurationPerTween = 0.1f;
    public int _id;
    private Vector3 startingPos;
    private Transform _transform;

    void Awake()
    {
        _transform = transform;
        _id = gameObject.GetInstanceID();
        startingPos = transform.position;
        Offset1 += transform.position;
        Offset2 += transform.position;
        Offset3 += transform.position;
        Offset4 += transform.position;
    }
    void Start()
    {
        GameManager.Instance.OnEntityHit.AddListener(delegate () { Tween(); });
    }

    public void Tween()
    {
        TweenFactory.Tween("Offset1" + _id, startingPos, Offset1, DurationPerTween, TweenScaleFunctions.SineEaseInOut, (t) =>
        {
            _transform.position = t.CurrentValue;
        }, (t) =>
        {
            TweenFactory.Tween("Offset2" + _id, Offset1, Offset2, DurationPerTween, TweenScaleFunctions.SineEaseInOut, (t1) =>
            {
                _transform.position = t1.CurrentValue;
            }, (t1) =>
            {
                TweenFactory.Tween("Offset3" + _id, Offset2, Offset3, DurationPerTween, TweenScaleFunctions.SineEaseInOut, (t2) =>
                {
                    _transform.position = t2.CurrentValue;
                }, (t2) =>
                {
                    TweenFactory.Tween("Offset4" + _id, Offset3, Offset4, DurationPerTween, TweenScaleFunctions.SineEaseInOut, (t3) =>
                    {
                        _transform.position = t3.CurrentValue;
                    }, (t3) =>
                    {
                        TweenFactory.Tween("Offset4" + _id, Offset4, startingPos, DurationPerTween, TweenScaleFunctions.SineEaseInOut, (t4) =>
                        {
                            _transform.position = t4.CurrentValue;
                        }, (t4) =>
                        {
                        });
                    }
           );

                }
            );
            }
            );
        });

    }
}
