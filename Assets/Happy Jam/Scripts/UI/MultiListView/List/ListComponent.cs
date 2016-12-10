using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UI.MultiListView.List
{
    /// <summary>
    /// ListComponent used in Rows and in Items
    /// </summary>
    public class ListComponent : Identity, IMultiList
    {
        [HideInInspector]
        public IdentityType type;

        public virtual void Add(ListComponent listItem) { }

        public int GetId()
        {
            return _id;
        }

        public Transform GetTransform()
        {
            return this.transform;
        }

        public virtual bool Initialize() { return false; }
        public virtual void Initialize(Personalization personalize) { }

        public virtual void OnEnable()
        {
            //Debug.Log("Level: " + this.CheckLevel());
        }

        public virtual void Operation()
        {
            throw new NotImplementedException();
        }

        public void SetId(int id)
        {
            this._id = id;
        }
        private static Item CreateItemFromPrefab(Data.Item.ItemType itemType, GameObject itemTypeText, GameObject itemTypeButton, GameObject itemTypeToggle)
        {
            Item item = null;
            GameObject objItem = Helper.UIObjectCreator.CreateItemFromPrefab(itemType, itemTypeText, itemTypeButton, itemTypeToggle);
            if (objItem != null)
            {
                item = objItem.GetComponent<List.Item>();
                if (item == null) item = objItem.AddComponent<Item>();
            }
            return item;
        }
        public static Item CreateItem(Data.IItem data, Transform rowTransform, bool useItemTypePrefabs, GameObject itemTypeText, GameObject itemTypeButton, GameObject itemTypeToggle)
        {
            Item item = null;
            Data.Item tmpData = data.Copy();

            if (tmpData != null)
            {
                if (useItemTypePrefabs)
                {
                    item = CreateItemFromPrefab(tmpData.GetItemType(), itemTypeText, itemTypeButton, itemTypeToggle);
                }
                if (item == null)
                {
                    GameObject objItem = new GameObject("Item");
                    item = objItem.AddComponent<List.Item>();
                }
                item.transform.SetParent(rowTransform);
                //item.SetIdManager(_idManager);
                item.AddData(tmpData);
                item.Initialize();
                item.SetFont(Font.CreateDynamicFontFromOSFont(Font.GetOSInstalledFontNames()[0], 12));
                //task.Invoke(task.Method.Name);
                //RegisterItemEvents(item, rowTransform.GetComponent<Row>());

            }
            return item;
        }
    }
}
