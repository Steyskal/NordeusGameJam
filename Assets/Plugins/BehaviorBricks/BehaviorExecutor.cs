using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Behavior executor component. Add it to your game objects
/// to execute BehaviorBrick's behaviors.
/// </summary>
[UnityEngine.AddComponentMenu("Behavior Bricks/Behavior executor component")]
public class BehaviorExecutor : BBUnity.InternalBehaviorExecutor
{
    protected override void Awake()
    {
        setDebugMode();
        base.Awake();
    }
    

    /// <summary>
    /// In editor mode, 
    /// </summary>
    new void Update()
    {
        bool prev = this.requestTickExecution;
        base.Update();
        if (prev != this.requestTickExecution)
            // Force inspector repaint in editor mode to reactivate
            // Tick button.
            EditorUtility.SetDirty(this);
    }

}
