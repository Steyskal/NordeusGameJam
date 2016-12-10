using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UI.MultiListView.Data;
using UI.MultiListView;
using System;
using UnityEditor;

public class ListViewTesting : MonoBehaviour
{
    public List<string> dataInput1;
    public List<ListViewData> data;
    public MultiListView _listView;

    void Start()
    {
        _listView = FindObjectOfType<MultiListView>();
        AddDataInput1();
        AddDataInput();
    }
    public void AddDataInput1()
    {
        _listView.AddRow(dataInput1);
        Debug.Log("Adding to List " + _listView);
    }
    public void AddDataInput()
    {
        List<Item> items = new List<Item>();
        foreach (ListViewData d in data)
        {
            items.Add(d.GetItem());
        }
        //_listView.AddRow(items);
        _listView.AddRowWithReference<ExampleClass>(items, new ExampleClass(data[0].item.GetLabel() + "22", 1, 2f));
    }

    public void ShowGUI()
    {
        //data.ShowGUI();
    }
}
public class ExampleClass
{
    public string name;
    public int value;
    public float value2;

    public ExampleClass(string name, int value, float value2)
    {
        this.name = name;
        this.value = value;
        this.value2 = value2;
    }
}

[Serializable]
public class ListViewData
{
    [SerializeField]
    public Item.ItemType type;
    [SerializeField]
    public Item item;

    public void ShowGUI()
    {
        /*EditorGUILayout.LabelField("Save game settings", EditorStyles.boldLabel);
        type = (Item.ItemType)EditorGUILayout.EnumPopup("Item Type:", type);
        if (type == Item.ItemType.Text)
        {

        }
        else if(type == Item.ItemType.Button)
        {

        }*/
    }
    public Item GetItem()
    {
        return item;
    }
}
