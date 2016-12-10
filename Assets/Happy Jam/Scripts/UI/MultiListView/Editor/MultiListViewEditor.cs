using System;
using UnityEngine;
using UnityEditor;

namespace UI.MultiListView.Editor
{
    [CustomEditor(typeof(MultiListView))]
    public class MultiListViewEditor : UnityEditor.Editor
    {
        MultiListView listView;

        public override void OnInspectorGUI()
        {
            listView = (MultiListView)target;

            //EditorGUILayout.IntField("ID", listView._id);
            SerializedProperty prop = serializedObject.FindProperty("m_Script");
            EditorGUILayout.PropertyField(prop, true, new GUILayoutOption[0]);
            serializedObject.ApplyModifiedProperties();


            Transform obj = EditorGUILayout.ObjectField("Items Reference", listView.itemsReference, typeof(Transform), true) as Transform;
            if (obj != null && listView.CheckItemsReference(obj)) listView.itemsReference = obj;

            if (listView.itemsReference != null)
            {
                listView.useHeader = EditorGUILayout.Toggle("Use Main Header", listView.useHeader);
                if (listView.useHeader)
                {
                    listView.headerReference = EditorGUILayout.ObjectField("Header Reference", listView.headerReference, typeof(GameObject), true) as GameObject;
                }

                listView.useElementPrefabs = EditorGUILayout.Toggle("Use Custom Element Prefabs", listView.useElementPrefabs);
                if (listView.useElementPrefabs)
                {
                    listView.headerElementPrefab = (GameObject)EditorGUILayout.ObjectField("Header Element Prefab", listView.headerElementPrefab, typeof(GameObject), true);
                    listView.listElementPrefab = (GameObject)EditorGUILayout.ObjectField("Row Element Prefab", listView.listElementPrefab, typeof(GameObject), true);
                }
                else
                {
                    SerializedProperty spPersonalize = serializedObject.FindProperty("personalize");
                    EditorGUI.BeginChangeCheck();
                    serializedObject.Update();
                    EditorGUILayout.PropertyField(spPersonalize, true);
                    if (EditorGUI.EndChangeCheck())
                        serializedObject.ApplyModifiedProperties();

                    if (GUILayout.Button("Refresh Existing item to style"))
                    {
                        listView.UpdateExistingStyles();
                        EditorUtility.SetDirty(target);
                    }
                    EditorGUILayout.Space();
                }

                listView.useItemTypePrefabs = EditorGUILayout.Toggle("Use Custom Item Type Prefabs", listView.useItemTypePrefabs);
                if (listView.useItemTypePrefabs)
                {
                    listView.itemTypeText = (GameObject)EditorGUILayout.ObjectField("Text Item Type Prefab", listView.itemTypeText, typeof(GameObject), true);
                    listView.itemTypeToggle = (GameObject)EditorGUILayout.ObjectField("Toggle Item Type Prefab", listView.itemTypeToggle, typeof(GameObject), true);
                    listView.itemTypeButtonDefault = (GameObject)EditorGUILayout.ObjectField("Default Button Item Type Prefab", listView.itemTypeButtonDefault, typeof(GameObject), true);
                }
                listView.useCustomButtonEvents = EditorGUILayout.Toggle("Use Custom Events", listView.useCustomButtonEvents);
                if (listView.useCustomButtonEvents)
                {
                    EditorGUILayout.LabelField("Custom Button Events");
                    SerializedProperty eventsAdd = serializedObject.FindProperty("eventsOnAdd");
                    SerializedProperty eventsDel = serializedObject.FindProperty("eventsOnDelete");
                    SerializedProperty eventsEdit = serializedObject.FindProperty("eventsOnEdit");
                    SerializedProperty eventsClick = serializedObject.FindProperty("eventsOnClick");
                    EditorGUI.BeginChangeCheck();
                    //DRAW DEFAULT AS UNITY
                    serializedObject.Update();
                    EditorGUILayout.PropertyField(eventsAdd, true);
                    EditorGUILayout.PropertyField(eventsDel, true);
                    EditorGUILayout.PropertyField(eventsEdit, true);
                    EditorGUILayout.PropertyField(eventsClick, true);

                    //DrawCustomButtonEventGUI(itemsList);

                    if (EditorGUI.EndChangeCheck())
                        serializedObject.ApplyModifiedProperties();

                }
                EditorGUILayout.LabelField("Test functions", EditorStyles.boldLabel);

                if (GUILayout.Button("Add Dummy Sublist"))
                {
                    listView.AddSublist();
                }
                if (GUILayout.Button("Add Dummy Row"))
                {
                    listView.AddDummyRow();
                }
                if (GUILayout.Button("Add Dummy String Row"))
                {
                    listView.AddDummyStringRow();
                }
                if (GUILayout.Button("Clear All"))
                {
                    listView.ClearAll();
                }
            }

        }

    }
}
