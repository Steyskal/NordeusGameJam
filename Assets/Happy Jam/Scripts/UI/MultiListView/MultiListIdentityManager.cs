using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UI.MultiListView.List;
using UnityEngine.UI;
using System;

namespace UI.MultiListView
{
    public class MultiListIdentityManager : MonoBehaviour
    {
        private int _lastID = 0;
        private Dictionary<int, ListComponent> objects = new Dictionary<int, ListComponent>();

        public int GetLastID()
        {
            return _lastID;
        }
        public int GetNextID()
        {
            return _lastID++;
        }
        public void RegisterListObject(ListComponent listComponent)
        {
            int ID = GetNextID();
            objects.Add(ID, listComponent);
            listComponent.SetId(ID);
            if (listComponent.GetType() == typeof(Row))
            {
                ((Row)listComponent).SetId(ID);
                //Debug.Log("Row: " + ((Row)listComponent).Get(0).Data.GetLabel());
            }else if (listComponent.GetType() == typeof(SubList))
            {
                ((SubList)listComponent).SetId(ID);
            }
            //Debug.Log("Current Id: " + ID + "("+listComponent.GetId()+")"+ " Total objects: " + objects.Count);
        }

        public ListComponent GetListObject(int Id)
        {
            ListComponent listComponent = null;
            if (objects.ContainsKey(Id))
            {
                listComponent = objects[Id];
            }
            return listComponent;
        }
    }
}
