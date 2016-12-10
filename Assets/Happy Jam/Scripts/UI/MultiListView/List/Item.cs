using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MultiListView.List
{
    /// <summary>s
    /// Part of row as Visible Element
    /// </summary>
    public class Item : ListComponent
    {
        public bool IsInitialized = false;
        public Data.Item Data = null;

        public void AddData(Data.Item data)
        {
            Data = data;
        }

        //TODO: Delete
        public bool CheckData()
        {
            bool isGoodType = false;
            if (Data != null)
            {
                if (Data.GetItemType() == UI.MultiListView.Data.Item.ItemType.Text)
                {
                    if (this.GetComponent<Text>())
                        isGoodType = true;
                }
                else if (Data.GetItemType() == UI.MultiListView.Data.Item.ItemType.Button)
                {
                    if (this.GetComponent<Button>())
                        isGoodType = true;
                }
                else if (Data.GetItemType() == UI.MultiListView.Data.Item.ItemType.Toggle)
                {
                    if (this.GetComponent<Toggle>())
                        isGoodType = true;
                }
            }
            return isGoodType;
        }

        public override void OnEnable()
        {
            base.OnEnable();
            this.type = IdentityType.ListItem;
        }

        #region Set Item Components
        public void SetText()
        {
            SetText(this.gameObject);
        }
        public void SetText(GameObject obj)
        {
            Helper.UIObjectCreator.AddTextComponent(obj);
            Text text = obj.GetComponent<Text>();
            if (text)
            {
                obj.GetComponent<RectTransform>().localScale = Vector2.one;
                text.text = Data.GetLabel();
            }
        }
        public void SetText(Font font)
        {
            SetText(this.gameObject, font);
        }
        public void SetText(GameObject obj, Font font)
        {
            Text text = obj.GetComponent<Text>();
            if (text)
            {
                text.font = font;
                text.text = Data.GetLabel();
            }
        }
        public void SetButton()
        {
            SetButton(this.gameObject);
        }
        public void SetButton(GameObject obj)
        {
            Helper.UIObjectCreator.AddButtonComponent(obj);
            Button button = obj.GetComponent<Button>();
            if (button)
            {
                obj.GetComponent<RectTransform>().localScale = Vector2.one;
                //Sets Button Label
                Transform child = obj.transform.GetChild(0);
                SetText(child.gameObject);
            }
        }
        /*public void SetEvents(UnityEngine.Events.UnityAction call, Button.ButtonClickedEvent buttonEvent)
        {
            Debug.Log("Sets events");
            if (Data.IsButton())
            {
                Debug.Log("Sets button events");
                Button button = this.GetComponent<Button>();
                button.onClick.RemoveAllListeners();
                button.onClick = buttonEvent;
                button.onClick.AddListener(call);
            }
        }*/
        public void SetFont(Font font)
        {
            Text text = GetComponent<Text>();
            if (text == null) text = GetComponentInChildren<Text>();
            text.font = font;
        }
        public void SetPersonalization(Personalization personalization)
        {
            ItemPersonalization itemPersonalization = personalization.item;
            if (Data.IsButton())
            {
                Button btn = GetComponent<Button>();
                ColorBlock colors = btn.colors;
                colors.normalColor = personalization.button.buttonColor;
                btn.colors = colors;

                itemPersonalization = personalization.button;
            }

            Text text = GetComponent<Text>();
            if (text == null) text = GetComponentInChildren<Text>();
            text.font = itemPersonalization.font;
            text.fontStyle = itemPersonalization.fontStyle;
            text.fontSize = itemPersonalization.fontSize;
            text.color = itemPersonalization.textColor;
            text.alignment = itemPersonalization.alignment;

            LayoutElement le = this.GetComponent<LayoutElement>();
            if (le == null) le = gameObject.AddComponent<LayoutElement>();
            le.minHeight = personalization.rowHeight;
        }

        #endregion

        void OnValidate()
        {
            //IsInitialized = false;
            //Initialize();
        }

        public override bool Initialize()
        {
            LayoutElement le = this.GetComponent<LayoutElement>();
            if (le == null)
            {
                le = gameObject.AddComponent<LayoutElement>();
            }
            le.minHeight = 20f;

            if (Data != null && !IsInitialized)
            {
                if (Data.GetItemType() == UI.MultiListView.Data.Item.ItemType.Text)
                {
                    SetText();
                }
                else if (Data.GetItemType() == UI.MultiListView.Data.Item.ItemType.Button)
                {
                    SetButton();
                }
                else if (Data.GetItemType() == UI.MultiListView.Data.Item.ItemType.Toggle)
                {
                    //TODO
                }
                IsInitialized = true;

            }
            return true;
        }
        public override void Initialize(Personalization personalize)
        {
            Debug.Log("Called initialization of item");
            GetComponent<RectTransform>().localScale = Vector2.one;
            //Initialize() TODO:Prebacit Poziv u sami Row;
            SetPersonalization(personalize);
        }

    }
}
