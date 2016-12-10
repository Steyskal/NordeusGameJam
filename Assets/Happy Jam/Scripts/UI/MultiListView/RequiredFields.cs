using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UI.MultiListView.List;
using UnityEngine.UI;
using System;

namespace UI.MultiListView
{
    //TODO možda uzet od ListView od Golmana
    public class RequiredFields : Identity
    {
        [Tooltip("Content must have Vertical Layout Group and Content Size Fitter")]
        public Transform itemsReference;

        public bool useHeader = false;
        public GameObject headerReference;

        #region Prefabs
        public bool useElementPrefabs = false;
        public GameObject headerElementPrefab;
        public GameObject listElementPrefab;
        public Personalization personalize;
        #endregion

        public bool useItemTypePrefabs = false;
        public GameObject itemTypeText;
        public GameObject itemTypeToggle;
        public GameObject itemTypeButtonDefault;
        //public CustomizableFields[] visualProperties;

        public bool CheckItemsReference(Transform itemsRef)
        {
            bool condition = false;
            if (itemsRef.GetComponent<ContentSizeFitter>())
                condition = true;
            else
            {
                Debug.LogWarning("Items Reference must have Content Size Fitter");
            }
            return condition;
        }
    }

    [Serializable]
    public class CustomizableFields
    {
        public Identity.IdentityType type;
        public Font font;
        public int fontSize = 12;
    }

    [Serializable]
    public class Personalization
    {
        [Header("Item Properties")]
        [SerializeField]
        public ItemPersonalization item;
        
        [Header("Button Item Properties")]
        [SerializeField]
        public ButtonItemPersonalization button;

        [Header("Row Properties")]
        public bool useRowColor = false;
        public UnityEngine.Color rowColor;
        public float rowHeight = 20f;
        public float spaceBetweenRows = 0f;

    }
    [Serializable]
    public class ItemPersonalization
    {
        public TextAnchor alignment = TextAnchor.MiddleCenter;
        public Font font;
        public FontStyle fontStyle = FontStyle.Normal;
        public int fontSize = 12;
        public UnityEngine.Color textColor;
    }

    [Serializable]
    public class ButtonItemPersonalization : ItemPersonalization
    {
        public UnityEngine.Color buttonColor;
    }
}
