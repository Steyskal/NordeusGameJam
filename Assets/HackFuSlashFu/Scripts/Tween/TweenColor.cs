using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Happy;
using System;
using UnityEngine.UI;
using DigitalRuby.Tween;

public class TweenColor : MonoBehaviour
{
    public SpriteRenderer Sprite;

    public Color NormalColor;
    public float NormalTargetDuration = 0.5f;
    public Color TargetColor;
    public float TargetNormalDuration = 0.5f;
    private int _id;

    void Awake()
    {
        _id = gameObject.GetInstanceID();
    }

    public void Tween()
    {
        TweenFactory.Tween("ColorDown" + _id, NormalColor, TargetColor, NormalTargetDuration, TweenScaleFunctions.SineEaseInOut, (t) =>
        {
            Sprite.color = t.CurrentValue;
        }, (t) =>
        {
            TweenFactory.Tween("ColorDown" + _id, TargetColor, NormalColor, TargetNormalDuration, TweenScaleFunctions.SineEaseInOut, (t1) =>
             {
                 Sprite.color = t1.CurrentValue;
             }, (t1) => { }
            );
        });

    }

    void OnDestroy()
    {
        TweenFactory.RemoveTweenKey("ColorDown" + _id, TweenStopBehavior.DoNotModify);
        TweenFactory.RemoveTweenKey("ColorDown" + _id, TweenStopBehavior.DoNotModify);
    }
}
