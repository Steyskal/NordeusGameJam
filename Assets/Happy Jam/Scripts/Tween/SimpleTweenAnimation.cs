using UnityEngine;
using System.Collections;
using DigitalRuby.Tween;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class SimpleTweenAnimation : MonoBehaviour
{
    public Transform ObjectToAnimate;
    public bool KeepZ = true;
    public bool OneAtSameTime = false;

    [Header("Effect")]
    [Tooltip("Range min-max")]
    public Vector2 TopAnimationTime;
    [Tooltip("Range min-max")]
    public Vector2 TopBottomAnimationTime;
    public Vector3 PositionTop;
    public Vector3 PositionBottom;
    private Vector3 startingPosition;
    private string _id;
    private bool active = false;

    [Header("Static")]
    [Tooltip("Only if One At Same Time enabled")]
    public int NumOfAnimationsBeforeNext = 2;

    [Header("Preset")]
    public PresetType PresetToLoad;
    public enum PresetType
    {
        UpDown,
        Up
    }
    void Reset()
    {
        ObjectToAnimate = transform;
    }

    void Awake()
    {
        if (OneAtSameTime)
        {
            TweenAnimationsManager.Add(this);
            TweenAnimationsManager.SetMaxCounter(NumOfAnimationsBeforeNext);
            SetActive(TweenAnimationsManager.IsActive(this));
        }
        startingPosition = ObjectToAnimate.position;
        PositionTop += startingPosition;
        PositionBottom += startingPosition;
        _id = gameObject.GetInstanceID().ToString();
    }

    void Start()
    {
        TweenStart();
    }
    void TweenStart()
    {
        if (OneAtSameTime && !this.active)
        {
            Invoke("TweenStart", Time.fixedDeltaTime);
            return;
        }

        // Animation
        Vector3 position = ObjectToAnimate.position;
        TweenFactory.Tween("Up" + _id, position, PositionTop, GetFloat(TopAnimationTime), TweenScaleFunctions.SineEaseInOut, (t) =>
           {
               // progress
               ObjectToAnimate.position = GetPosition(t.CurrentValue);
           }, (t) =>
           {
               TweenFactory.Tween("Down" + _id, PositionTop, PositionBottom, GetFloat(TopBottomAnimationTime), TweenScaleFunctions.SineEaseInOut, (t1) =>
               {
                   // progress
                   ObjectToAnimate.position = GetPosition(t1.CurrentValue);
               }, (t1) =>
               {
                   if (OneAtSameTime)
                   {
                       TweenAnimationsManager.SetActiveNext();
                   }
                   Invoke("TweenStart", Time.fixedDeltaTime);

               });

           });
    }
    Vector3 GetPosition(Vector3 positon)
    {
        if (KeepZ)
        {
            return new Vector3(positon.x, positon.y, startingPosition.z);
        }
        return positon;
    }
    float GetFloat(Vector2 range)
    {
        return Random.Range(range.x, range.y);
    }

    void OnDestroy()
    {
        TweenAnimationsManager.Remove(this);
        if (this.active)
        {
            TweenFactory.RemoveTweenKey("Up" + _id, TweenStopBehavior.DoNotModify);
            TweenFactory.RemoveTweenKey("Down" + _id, TweenStopBehavior.DoNotModify);
            TweenAnimationsManager.SetActiveNext();
        }
    }
    public void SetActive(bool active)
    {
        this.active = active;
    }
    public void SetPreset()
    {
        TopAnimationTime = new Vector2(0.5f, 0.6f);
        TopBottomAnimationTime = new Vector2(1, 1);
        if (PresetToLoad == PresetType.UpDown)
        {
            TopBottomAnimationTime = new Vector2(0.5f, 0.6f);
            PositionTop = new Vector3(0, 1, 0);
            PositionBottom = new Vector3(0, -1, 0);
        }
        else if (PresetToLoad == PresetType.Up)
        {
            PositionTop = new Vector3(0, 1, 0);
            PositionBottom = new Vector3(0, 0, 0);
        }
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(SimpleTweenAnimation))]
public class SimpleTweenAnimationEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        SimpleTweenAnimation myAnimation = (SimpleTweenAnimation)target;
        if (Application.isEditor && !Application.isPlaying && GUILayout.Button("Set Preset"))
        {
            myAnimation.SetPreset();
        }

    }
}
#endif
