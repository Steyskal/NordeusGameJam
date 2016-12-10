using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UI.MultiListView.Data;
using UI.MultiListView;
using System;
using UnityEditor;
using UnityEngine.UI;
using UI.MultiListView.List;
using UnityEngine.Events;
//Event Responder
public class EventResponder : MonoBehaviour
{
    public MultiListView MultiListView;

    void OnEnable()
    {
        //deleting old multiple listeners
        MultiListView.OnRowClickedEvent.RemoveAllListeners();
        MultiListView.OnRowClickedEvent.AddListener(OnRowClicked);
    }

    void OnDestroy()
    {
        //MultiListView.OnRowClickedEvent.RemoveListener(OnRowClicked);
    }
    private void OnRowClicked(UI.MultiListView.Data.Item item, Row row)
    {
        OnButtonPress(item.GetButtonType(), row);
    }
    public void OnButtonPress(UI.MultiListView.Data.Item.ButtonType buttonType, Row row)
    {
        Debug.Log("Clicked on row: " + row.GetId());
        if (buttonType == UI.MultiListView.Data.Item.ButtonType.Add)
        {
            OnAddButtonClick(row);
        }
        else if (buttonType == UI.MultiListView.Data.Item.ButtonType.Edit)
        {
            OnEditButtonClick(row);
        }
        else if (buttonType == UI.MultiListView.Data.Item.ButtonType.Remove)
        {
            OnDeleteButtonClick(row);
        }
        else if (buttonType == UI.MultiListView.Data.Item.ButtonType.Event)
        {
            OnCustomButtonClick(row);
        }
    }

    public virtual void OnAddButtonClick(Row row)
    {
        //TODO
    }
    public virtual void OnEditButtonClick(Row row)
    {
        //TOD
    }
    public virtual void OnDeleteButtonClick(Row row)
    {
        //TOD
    }
    public virtual void OnCustomButtonClick(Row row)
    {
        //TOD
    }
    
}
