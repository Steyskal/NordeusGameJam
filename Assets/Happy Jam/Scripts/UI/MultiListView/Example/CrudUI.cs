using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UI.MultiListView;
using System;
using UnityEditor;
using UnityEngine.UI;
using UI.MultiListView.List;
//Event Responder
public class CrudUI : EventResponder
{
    public override void OnAddButtonClick(Row row)
    {
        Debug.Log("Clicked ADD BUTTON");

        //SetItems(listRow.Get(0).Data.GetLabel());
        if (row.reference != null)
        {
            ExampleClass reference = (ExampleClass)row.reference;
            Debug.Log("Row Data Ref " + reference.name + " " + reference.value + " " + reference.value2 );
        }
        SetItems(row);
    }
    public override void OnEditButtonClick(Row row)
    {
        //TOD
        Debug.Log("Clicked EDIT BUTTON");
        Row listRow = (Row)row;
        if (listRow)
        {
            SetItems(listRow.Get(0).Data.GetLabel());
        }
    }
    public override void OnDeleteButtonClick(Row row)
    {
        //TOD
    }
    public override void OnCustomButtonClick(Row row)
    {
        //TOD
    }

    public Text textItem;
    public FieldData[] items;

    public void SetItems(string txt)
    {
        if (textItem != null)
        {
            textItem.text = txt;
        }
    }
    public void SetItems(Row row)
    {
        foreach (FieldData d in items)
        {
            if (d.name == "Rbr")
            {
                if (d.label) d.label.text = "Rbr";
                if (d.input) d.input.text = "" + row.GetId();
            }
            else if (d.name == "Name")
            {
                if (d.label) d.label.text = "Name";
                if (d.input) d.input.text = row.Get(0).Data.GetLabel();
            }
            else if (d.name == "Name2")
            {
                if (d.label) d.label.text = "Name2";
                if (d.input) d.input.text = row.Get(1).Data.GetLabel();
            }
            else if (d.name == "Name3")
            {
                if (d.label) d.label.text = "Name3";
                if (d.input) d.input.text = row.Get(2).Data.GetLabel();
            }
        }
    }
    public void Set(IMultiList row)
    {
        return;
    }
}
[System.Serializable]
public class FieldData
{
    [SerializeField]
    public string name;
    [SerializeField]
    public InputField input;
    [SerializeField]
    public Text label;

}
