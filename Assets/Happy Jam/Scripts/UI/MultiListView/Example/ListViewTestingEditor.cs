using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UI.MultiListView;
using System;
using UnityEditor;

[CustomEditor(typeof(ListViewTesting))]
public class ListViewTestingEditor : Editor
{
    ListViewTesting test;

    public override void OnInspectorGUI()
    {
        test = (ListViewTesting)target;
        test.ShowGUI();
        DrawDefaultInspector();
        if(GUILayout.Button("Add Again"))
        {
            test.AddDataInput1();
            test.AddDataInput();
        }
    }
}
