using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UI.MultiListView.List;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MultiListView.Helper
{
    /// <summary>
    /// Creates numerious objects
    /// </summary>
    public static class UIObjectCreator
    {
        const string ROW_NAME = "Row";
        /*public static GameObject CreateGOWithComponent<T>()
        {
            GameObject obj = new GameObject();
            obj.AddComponent<T>();
            return obj;
        }*/

        //Adds Button Component to empty object
        public static void AddButtonComponent(GameObject obj)
        {
            if (obj.GetComponent<Button>() == null)
            {
                Image img = obj.AddComponent<Image>();
                Button btn = obj.AddComponent<Button>();
                btn.targetGraphic = img;
                GameObject textObj = new GameObject("Text");
                textObj.transform.SetParent(obj.transform);
                Text text = textObj.AddComponent<Text>();
                text.color = UnityEngine.Color.black;
                text.alignment = TextAnchor.MiddleCenter;

                RectTransform rec = textObj.GetComponent<RectTransform>();
                rec.anchorMax = Vector2.one;
                rec.anchorMin = Vector2.zero;
                rec.sizeDelta = Vector2.zero;

            }
        }
        public static void AddTextComponent(GameObject obj)
        {                                   // && <Image>() == null (Only one Graphic component
            if (obj.GetComponent<Text>() == null)
            {
                obj.AddComponent<Text>();
            }
        }
        /// <summary>
        /// Create Item (ListItem) from prefab
        /// TODO: Might be optimized without checkih if itemType have component ON
        /// </summary>
        /// <param name="data"></param>
        /// <param name="itemTypeText"></param>
        /// <param name="itemTypeButton"></param>
        /// <param name="itemTypeToggle"></param>
        /// <returns>Returns GameObject</returns>
        public static GameObject CreateItemFromPrefab(Data.Item.ItemType itemType, GameObject itemTypeText, GameObject itemTypeButton, GameObject itemTypeToggle)
        {
            GameObject objItem = null;
            if (itemType == Data.Item.ItemType.Text)
            {
                if (itemTypeText != null && itemTypeText.GetComponent<Text>())
                {
                    objItem = GameObject.Instantiate(itemTypeText);
                }
                else
                {
                    Debug.LogWarning("No defined prefab for Text Item or prefab does not have Text component");
                }
            }
            else if (itemType == Data.Item.ItemType.Button)
            {
                if (itemTypeButton != null && itemTypeText.GetComponent<Button>())
                {
                    objItem = GameObject.Instantiate(itemTypeButton);
                }
                else
                {
                    Debug.LogWarning("No defined prefab for Button Item or prefab does not have Button component");
                }
            }
            else if (itemType == Data.Item.ItemType.Toggle)
            {
                if (itemTypeToggle != null && itemTypeText.GetComponent<Toggle>())
                {
                    objItem = GameObject.Instantiate(itemTypeToggle);
                }
                else
                {
                    Debug.LogWarning("No defined prefab for Toggle Item or prefab does not have Toggle component");
                }
            }

            return objItem;
        }

        /*public static Row CreateRow(Transform parent)
        {
            return CreateRow(parent, null);
        }*/

        public static Row CreateRow(Transform parent, Personalization personalize)
        {
            GameObject obj = new GameObject(ROW_NAME);
            Row row = obj.AddComponent<Row>();
            /*if (personalize != null)
            {
                row.Initialize(personalize);
            }
            else
            {
                row.Initialize();
            }*/
            obj.transform.SetParent(parent);
            return row;
        }
        /*public static SubList CreateSubList(Transform parent)
        {
            return CreateSubList(parent, null);
        }*/
        public static SubList CreateSubList(Transform parent, Personalization personalize)
        {
            GameObject obj = new GameObject(ROW_NAME);
            SubList sublist = obj.AddComponent<SubList>();
            sublist.Initialize();
            obj.transform.SetParent(parent);
            return sublist;
        }
    }
}
