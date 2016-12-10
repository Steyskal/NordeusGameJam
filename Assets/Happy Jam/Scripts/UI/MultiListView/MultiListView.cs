using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UI.MultiListView.List;
using UI.MultiListView.Data;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

namespace UI.MultiListView
{
    [System.Serializable]
    public class RowEvent : UnityEvent<Data.Item, Row> { }

    [RequireComponent(typeof(MultiListIdentityManager))]
    public class MultiListView : AnimationFields
    {
        public RowEvent OnRowClickedEvent = new RowEvent();

        public bool useCustomButtonEvents;
        public UnityEvent eventsOnAdd;
        public UnityEvent eventsOnDelete;
        public UnityEvent eventsOnEdit;
        public UnityEvent eventsOnClick;

        private MultiListIdentityManager _idManager;

        void Reset()
        {
            _id = -1;
            Level = 0;
            _idManager = this.GetComponent<MultiListIdentityManager>();
        }
        void Awake()
        {
            if (_idManager == null)
            {
                _idManager = this.GetComponent<MultiListIdentityManager>();
            }
            if (itemsReference.childCount > 0)
            {
                SetExistingItems();
            }
        }

        //Check if animations are on and attach them
        void SetExistingItems()
        {
            if (_idManager != null)
            {
                foreach (Row row in itemsReference.GetComponentsInChildren<Row>())
                {
                    _idManager.RegisterListObject(row);
                    foreach (List.Item i in row.children)
                    {
                        RegisterItemEvents(i, row);
                    }
                }
                Debug.Log("Existing items ID's are altered");
            }
        }
        public void UpdateExistingStyles()
        {
            foreach (SubList sub in itemsReference.GetComponentsInChildren<SubList>())
            {
                sub.Initialize(personalize);
                foreach (Row row in sub.GetComponentsInChildren<Row>())
                {
                    row.Initialize(personalize);
                }
            }
        }



        #region Add to List
        public SubList GetLastSublist()
        {
            SubList[] sublists = itemsReference.GetComponentsInChildren<SubList>();
            if (sublists != null)
                return sublists[sublists.Length - 1];
            return null;
        }
        /// <summary>
        /// Adds List of Strings as new Row of data
        /// New Row will be added at last Sublist
        /// </summary>
        /// <param name="data"></param>
        public void AddRow(List<string> data)
        {
            List<Data.Item> dataItems = new List<Data.Item>();
            foreach (string d in data)
            {
                dataItems.Add(new Data.Item(d));
            }
            SubList parent = GetLastSublist();
            if (parent != null)
            {
                AddRow(dataItems, parent);
            }
            else
            {
                Debug.LogWarning("There is no Sublist Component attached in last child in Content");
            }
        }

        public Row AddRow(List<Data.Item> dataItems)
        {
            SubList parent = GetLastSublist();
            if (parent == null)
            {
                parent = Helper.UIObjectCreator.CreateSubList(itemsReference, personalize);
            }
            List.Row row = Helper.UIObjectCreator.CreateRow(parent.GetTransform(), personalize);
            parent.Add(row);
            AddItemsToParent(dataItems, row);
            return row;
        }
        /// <summary>
        /// Row by this definition will be inside (at least) Sublist (IMultiList type)
        /// </summary>
        /// <param name="dataItems"></param>
        /// <param name="parent"></param>
        public Row AddRow(List<Data.Item> dataItems, SubList parent)
        {
            List.Row row = Helper.UIObjectCreator.CreateRow(parent.GetTransform(), personalize);
            AddItemsToParent(dataItems, row);
            return row;
        }
        /// <summary>
        /// Adding items to parent and registering events for buttons in row
        /// </summary>
        /// <param name="dataItems"></param>
        /// <param name="parent">Parent can either be Sublist (header) or Row</param>
        public void AddItemsToParent(List<Data.Item> dataItems, ListComponent parent)
        {
            if (_idManager)
            {
                _idManager.RegisterListObject(parent);
            }
            foreach (Data.Item data in dataItems)
            {
                List.Item item = CreateItem(data, parent.GetTransform());
                if (parent.GetType() == typeof(Row))
                {
                    RegisterItemEvents(item, (Row)parent);
                }
                parent.Add(item);
            }
            if (!useItemTypePrefabs)
                parent.Initialize(personalize);
            else
                parent.Initialize();
        }

        public void AddRowWithReference<T>(List<Data.Item> dataItems, T objReference)
        {
            Row row = AddRow(dataItems);
            row.SetReference<T>(objReference);
        }
        #endregion

        #region Test Editor features
        public void AddDummyStringRow()
        {
            List<string> row = new List<string>();
            row.Add("Item1");
            row.Add("Item2");
            row.Add("Item3");
            AddRow(row);
        }
        public void AddSublist()
        {
            GameObject obj = new GameObject("Sublist");
            SubList list = obj.AddComponent<SubList>();
            list.Initialize();
            list.transform.SetParent(itemsReference);
            //TODO možda:
            //Sublist.Initialize();  ->  IMultiList.Initialize(); -> AddComponentWithParameters

            Data.Item data = new Data.Item("HEADER");
            List.Item item1 = CreateItem(data, obj.transform);
            item1.transform.SetParent(obj.transform);
            list.Add(item1);

        }

        public void AddDummyRow()
        {
            GameObject obj = new GameObject("Row");
            List.Row list = obj.AddComponent<List.Row>();
            list.Initialize();
            obj.transform.SetParent(itemsReference.GetChild(itemsReference.childCount - 1));

            Data.Item data = new Data.Item("Text1");
            Data.Item data2 = new Data.Item("Text2");
            Data.Item data3 = new Data.Item("Text3", Data.Item.ButtonType.Add);
            List<Data.Item> datas = new List<Data.Item>();
            datas.Add(data);
            datas.Add(data2);
            datas.Add(data3);
            AddItemsToParent(datas, list);

            /*List.Item item1 = CreateItem(data, obj.transform);
            item1.transform.SetParent(obj.transform);
            List.Item item2 = CreateItem(data2, obj.transform);
            item2.transform.SetParent(obj.transform);
            List.Item item3 = CreateItem(data3, obj.transform);
            item3.transform.SetParent(obj.transform);

            list.Add(item1);
            list.Add(item2);
            list.Add(item3);*/

        }
        public void ClearAll()
        {
            for (int i = 0; i < itemsReference.childCount; i++)
            {
                DestroyImmediate(itemsReference.GetChild(i).gameObject);
            }
        }

        #endregion

        #region Creating Items
        /// <summary>
        /// Creates Item
        /// Uses ItemPrefab if useItemTypePrefabs is ON
        /// Otherwise creates default styled itemPrefab (faster)
        /// </summary>
        /// <param name="data"></param>
        /// <returns>List Item</returns>
        public List.Item CreateItem(Data.Item data, Transform rowTrans)
        {
            return ListComponent.CreateItem(data, rowTrans, useItemTypePrefabs, itemTypeText, itemTypeButtonDefault, itemTypeToggle);
        }
        #endregion
        #region Register Events
        public void RegisterItemEvents(List.Item item, Row row)
        {
            if (item.Data.IsButton() && row != null)
            {
                Data.Item data = item.Data.Copy();
                //Adding Events
                Data.Item.ButtonType type = data.GetButtonType();
                int rowId = row.GetId();

                Button button = item.GetComponent<Button>();
                if (useCustomButtonEvents)
                {
                    if (data.GetButtonType() == Data.Item.ButtonType.Add)
                    {
                        button.onClick.AddListener(delegate () { OnClick(eventsOnAdd, rowId); });
                    }
                    else if (data.GetButtonType() == Data.Item.ButtonType.Edit)
                    {
                        button.onClick.AddListener(delegate () { OnClick(eventsOnEdit, rowId); });
                    }
                    else if (data.GetButtonType() == Data.Item.ButtonType.Remove)
                    {
                        button.onClick.AddListener(delegate () { OnClick(eventsOnDelete, rowId); });
                    }
                    else if (data.GetButtonType() == Data.Item.ButtonType.Event)
                    {
                        button.onClick.AddListener(delegate () { OnClick(eventsOnClick, rowId); });
                    }
                }

                button.onClick.AddListener(delegate () { OnClick(data, rowId); });
            }

        }

        public void OnClick(UnityEvent call, int rowId)
        {
            //Debug.Log("UnityEvent CAll on Row id: " + rowId);
            call.Invoke();
        }

        public void OnClick(Data.Item item, int rowId)
        {
            Row row = null;
            if (_idManager)
            {
                row = (Row)_idManager.GetListObject(rowId);
            }

            if (OnRowClickedEvent != null && row != null && item != null)
            {
                Debug.Log("Invoked Row id: " + rowId);
                OnRowClickedEvent.Invoke(item, row);
            }
        }
        #endregion
    }

}
