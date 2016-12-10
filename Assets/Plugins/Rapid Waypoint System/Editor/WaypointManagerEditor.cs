using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.IO;
using System.Collections.Generic;

public enum WaypointAdditionMode
{
    Before,
    After,
    Normal
}

[CanEditMultipleObjects]
[CustomEditor(typeof(WaypointManager))]
public class WaypointManagerEditor : Editor {

    public static Component GetSerializedPropertyRootComponent(SerializedProperty property)
    {
        return (Component)property.serializedObject.targetObject;
    }

    private ReorderableList nodeList;
    private bool refresh = false;

    private void OnEnable()
    {
        nodeList = new ReorderableList(serializedObject,
                                   serializedObject.FindProperty("waypointNodes"),
                                   true, true, true, true);

        nodeList.drawElementCallback =
        (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            var element = nodeList.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;

            Transform t = element.FindPropertyRelative("transform").objectReferenceValue as Transform;
            if (t == null || element == null)
            {
                if(index > 0 && index < (target as WaypointManager).waypointNodes.Count)
                    (target as WaypointManager).waypointNodes.RemoveAt(index);
                (target as WaypointManager).waypointNodes.TrimExcess();
                refresh = true;
                //ReorderableList.defaultBehaviours.pa                
            }

            //GUIContent typeContent = new GUIContent("Node " + index + ":", "");
            //Rect typeRect = new Rect(rect.x, rect.y, 50, EditorGUIUtility.singleLineHeight);
            //EditorGUI.LabelField(typeRect, typeContent);

            //Rect prefabRect = new Rect(rect.x + 55, rect.y, rect.width - 60, EditorGUIUtility.singleLineHeight);
            //EditorGUI.PropertyField(prefabRect, element.FindPropertyRelative("transform"), GUIContent.none);

            GUIContent typeContent = new GUIContent("Node " + index);
            Rect typeRect = new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight);
            GUIStyle style = new GUIStyle();
            style.alignment = TextAnchor.MiddleCenter;
            EditorGUI.LabelField(typeRect, typeContent, style);

        };

        nodeList.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "Nodes");
        };

        nodeList.onSelectCallback = (ReorderableList l) =>
        {
            var transform_ = l.serializedProperty.GetArrayElementAtIndex(l.index).FindPropertyRelative("transform").objectReferenceValue as Transform;
            if (transform_)
            {
                EditorGUIUtility.PingObject(transform_.gameObject);
                //Selection.activeTransform = transform_;
            }
        };

        nodeList.onRemoveCallback = (ReorderableList l) =>
        {
            var transform_ = l.serializedProperty.GetArrayElementAtIndex(l.index).FindPropertyRelative("transform").objectReferenceValue as Transform;
            if (transform_)
                DestroyImmediate(transform_.gameObject);
            ReorderableList.defaultBehaviours.DoRemoveButton(l);
            for (int i = 0; i < l.count; ++i)
            {
                //var transform_ = l.serializedProperty.GetArrayElementAtIndex(i).FindPropertyRelative("transform").objectReferenceValue as Transform;
                if (transform_ != null)
                {
                    transform_.gameObject.name = "Node " + i.ToString();
                }
            }
        };

        nodeList.onAddCallback = (ReorderableList l) =>
        {

            CreateNode();
            for (int i = 0; i < l.count; ++i)
            {
                var transform_ = l.serializedProperty.GetArrayElementAtIndex(i).FindPropertyRelative("transform").objectReferenceValue as Transform;
                if (transform_ != null)
                {
                    transform_.gameObject.name = "Node " + i.ToString();
                }
            }
        };


        nodeList.onChangedCallback = (ReorderableList l) =>
        {
            for (int i = 0; i < l.count; ++i)
            {
                var transform_ = l.serializedProperty.GetArrayElementAtIndex(i).FindPropertyRelative("transform").objectReferenceValue as Transform;
                if (transform_ != null)
                {
                    transform_.gameObject.name = "Node " + i.ToString();
                }
            }
        };

    }

    public static void CreateNode(WaypointAdditionMode mode = WaypointAdditionMode.Normal)
    {
        var nodes = FindObjectOfType<WaypointManager>().waypointNodes;

        Node node = new Node();
        GameObject obj = new GameObject("Node " + nodes.Count);
        obj.transform.parent = FindObjectOfType<WaypointManager>().transform;
        node.transform = obj.transform;

        if (nodes.Count > 0)
        {
            obj.transform.position = FindObjectOfType<WaypointManager>().waypointNodes[FindObjectOfType<WaypointManager>().waypointNodes.Count - 1].transform.position;
            obj.transform.rotation = FindObjectOfType<WaypointManager>().waypointNodes[FindObjectOfType<WaypointManager>().waypointNodes.Count - 1].transform.rotation;
            //obj.transform.rotation = m_previousTransform.rotation;
        }

        var wn = obj.AddComponent<WaypointNode>();
        node.transform = wn.transform;
        
       // int index = nodes.FindIndex(x => x.transform = Selection.activeGameObject.transform);
        switch (mode)
        {
            case WaypointAdditionMode.Before:
                //nodes.Insert(index - 1, node);
                //waypointNode.transform.position = Selection.activeGameObject.transform.position;
                //waypointNode.transform.rotation = Selection.activeGameObject.transform.rotation;
                //Debug.Log(index);
                
                break;
            case WaypointAdditionMode.After:
                //nodes.Insert(index + 1, node);
                //waypointNode.transform.position = Selection.activeGameObject.transform.position;
                //waypointNode.transform.rotation = Selection.activeGameObject.transform.rotation;
                //Debug.Log(index);
                break;
            case WaypointAdditionMode.Normal:
                nodes.Add(node);
                //waypointNode.transform.position = node.transform.position;
                //waypointNode.transform.rotation = node.transform.rotation;
                break;
            default:
                Debug.LogError("Waypoint System: Other modes not supported!");
                break;
        }
        //SerializedProperty element = nl.serializedProperty.GetArrayElementAtIndex(index);
        

        //element.FindPropertyRelative("transform").objectReferenceValue = node.transform;
        //so.ApplyModifiedProperties();

        //for (int i = 0; i < nodes.Count; ++i)
        //{
        //    var transform_ = nodes[i].transform;
        //    if (transform_ != null)
        //    {
        //        transform_.gameObject.name = "Node " + i.ToString();
        //    }
        //}


        Undo.RegisterCreatedObjectUndo(obj, "Created " + obj.name);
        Selection.activeObject = obj;
    }

    public override void OnInspectorGUI()
    {
        WaypointManager waypoints = target as WaypointManager;

        GUIContent looping = new GUIContent("Should Loop", "Should the agents restart from node 0?");
        waypoints.looping = EditorGUILayout.Toggle(looping, waypoints.looping);

        GUIContent updateInterval = new GUIContent("Update interval (/s)", "The amount of times per second the waypoint system should update.");
        waypoints.updateIntervalPerSecond = EditorGUILayout.IntSlider(updateInterval, waypoints.updateIntervalPerSecond, 10, 60);

        GUIContent nodeDistance = new GUIContent("Node Proximity", "The minimum distance from the current node to change to the next.");
        waypoints.nodeProximityDistance = EditorGUILayout.FloatField(nodeDistance, waypoints.nodeProximityDistance);

        EditorGUILayout.Separator();

        //List<WaypointAgent> ra = serializedObject.FindProperty("objectToMove") as List<WaypointAgent>();
        EditorGUILayout.SelectableLabel("Current active objects in system:" + waypoints.AgentQuantity.ToString());

        //GUIContent targetMin = new GUIContent("Target spread", "The amount of randomness to apply to chosing the next point to traverse to.");
        //waypoints.nodeProximityDistance = EditorGUILayout.Slider(targetMin, waypoints.nodeProximityDistance, 0.0f, 10.0f);

        GUIContent rotationMode = new GUIContent("Agent Rotation Mode", "The the agent should look at the next node");
        waypoints.rotationMode = (WaypointRotationMode)EditorGUILayout.EnumPopup(rotationMode, waypoints.rotationMode);

        if (waypoints.rotationMode == WaypointRotationMode.Slerp)
        {
            GUIContent slerpRotationSpeed = new GUIContent("Rotation speed", "The speeed at which the agent looks to face the target");
            waypoints.slerpRotationSpeed = EditorGUILayout.Slider(slerpRotationSpeed, waypoints.slerpRotationSpeed, 1, 100);
        }

        GUILayout.Space(10);
        serializedObject.Update();
        nodeList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
        GUILayout.Space(10);

        if(refresh)
        {
            for (int i = 0; i < nodeList.count; ++i)
            {
                var transform_ = nodeList.serializedProperty.GetArrayElementAtIndex(i).FindPropertyRelative("transform").objectReferenceValue as Transform;
                if (transform_ != null)
                {
                    transform_.gameObject.name = "Node " + i.ToString();
                }
            }
            refresh = false;

        }
    }


    void OnSceneGUI()
    {
        Handles.BeginGUI();

        GUILayout.Window(2, new Rect(Screen.width - 260, Screen.height - 130, 250, 100), (id) => {
            if (GUILayout.Button("Add New Node", GUILayout.Height(EditorGUIUtility.singleLineHeight * 5)))
                CreateNode();
        }, "Waypoint System");

        Handles.EndGUI();
    }

}
