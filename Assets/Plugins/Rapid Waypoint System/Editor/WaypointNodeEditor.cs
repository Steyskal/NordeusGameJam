using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(WaypointNode))]
public class WaypointNodeEditor :  Editor{

    void OnSceneGUI()
    {
        Handles.BeginGUI();

        GUILayout.Window(2, new Rect(Screen.width - 260, Screen.height - 130, 250, 100), (id) => {
            if (GUILayout.Button("Add New Node", GUILayout.Height(EditorGUIUtility.singleLineHeight * 5)))
                WaypointManagerEditor.CreateNode();
        }, "Waypoint System");

        Handles.EndGUI();
    }

    public override void OnInspectorGUI()
    {
        GUILayout.Space(EditorGUIUtility.singleLineHeight);
        EditorGUILayout.LabelField("DO NOT DELETE THIS COMPONENT! \nIt is required for the waypoint system to function properly.", GUILayout.MinHeight(EditorGUIUtility.singleLineHeight * 3));
        serializedObject.ApplyModifiedProperties();
    }
}
