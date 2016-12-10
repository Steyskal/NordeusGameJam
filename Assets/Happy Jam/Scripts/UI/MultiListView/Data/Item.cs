using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UI.MultiListView.Data
{
    [Serializable]
    public class Item : IItem
    {
        public enum ItemType
        {
            Empty,
            Text,
            Button, 
            Toggle
        }

        [SerializeField]
        protected string label;

        [SerializeField]
        protected ItemType type;


        public enum ButtonType
        {
            None,
            Add,
            Edit,
            Remove,
            Event
        }
        [SerializeField]
        protected ButtonType buttonType;


        public Item() { }

        public Item(string label)
        {
            this.label = label;
            type = ItemType.Text;
        }
        public Item(ItemType type)
        {
            this.type = type;
        }
        public string GetLabel()
        {
            return label;
        }
        public ItemType GetItemType()
        {
            return type;
        }
        public bool IsButton()
        {
            return type == ItemType.Button;
        }

        public static Item Copy(Item item)
        {
            Item newItem = null;
            if (item.IsButton())
                newItem = new Item(item.GetLabel(), item.GetButtonType());
            else
                newItem = new Item(item.GetLabel());
            return newItem;
        }

        public Item Copy()
        {
            return Item.Copy(this);
        }


        /// <summary>
        /// Create ButtonItem
        /// </summary>
        /// <param name="label"></param>
        /// <param name="type"></param>
        public Item(string label, ButtonType type)
        {
            buttonType = type;
            this.label = label;
            this.type = ItemType.Button;
        }

        public ButtonType GetButtonType()
        {
            return buttonType;
        }
    }
}
