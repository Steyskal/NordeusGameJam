using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MultiListView.List
{
    /// <summary>
    /// Leaf in Composite
    /// </summary>
    public class Row : ListComponent
    {
        public List<Item> children;
        public bool isHeader = false;
        public bool VerticalChildren = false;
        public string firstChildString = "";
        public object reference;

        //private Data.Row _row = null;

        private bool _selected;
        public bool Selected
        {
            get
            {
                return _selected;
            }
            set
            {
                //TODO: getComponent<Toogle>().selected = value;
                _selected = value;
            }
        }

        public void Add(Item listItem)
        {
            if (children == null)
            {
                children = new List<Item>();
            }
            children.Add(listItem);
            if (children.Count < 2)
                firstChildString = listItem.Data.GetLabel();
        }

        public void Remove(Item listItem)
        {
            children.Remove(listItem);
        }
        public Item Get(int child)
        {
            Item item = null;
            if (children != null && children.Count > child)
            {
                item = children[child];
            }
            return item;
        }
        public Item GetById(int identity)
        {
            Item item = null;
            foreach (Item i in children)
                if (i.GetId() == identity)
                {
                    item = i;
                    break;
                }
            return item;
        }
        public override void OnEnable()
        {
            base.OnEnable();
            this.type = IdentityType.ListRow;
        }

        public override bool Initialize()
        {
            if (VerticalChildren && this.GetComponent<VerticalLayoutGroup>() == null) {
                VerticalLayoutGroup vlg = this.gameObject.AddComponent<VerticalLayoutGroup>();
                vlg.childControlHeight = true;
                vlg.childControlWidth = true;
            }
            else if (!VerticalChildren && this.GetComponent<HorizontalLayoutGroup>() == null) {
                HorizontalLayoutGroup hlg = this.gameObject.AddComponent<HorizontalLayoutGroup>();
                hlg.childControlHeight = true;
                hlg.childControlWidth = true;
            }
            GetComponent<RectTransform>().localScale = Vector2.one;

            return true;
        }
        public override void Initialize(Personalization personalize)
        {
            Initialize();
            if (personalize.useRowColor)
            {
                Image img = GetComponent<Image>();
                if (img == null) img = this.gameObject.AddComponent<Image>();
                img.color = personalize.rowColor;
            }

            //TODO On Play rewriteaju se djeca
            //Debug.Log("Global: Children count " + children.Count);
#if UNITY_EDITOR
            if (UnityEditor.EditorApplication.isPlaying)
            {
                Debug.Log("App.IsPlaying Called initialization of item");
                if (children == null)
                {
                    Debug.Log("Children are null");
                    children = GetComponentsInChildren<Item>().ToList<Item>();
                    Debug.Log("Children " + (children == null ? "null" : "true"));
                    Debug.Log("Children count " + children.Count);
                }
                if (children != null && children.Count > 0)
                    foreach (Item i in children)
                    {
                        i.Initialize(personalize);
                    }
            }
            else
            {
                if (children == null)
                    foreach (Item i in this.GetComponentsInChildren<Item>())
                    {
                        Add(i);
                    }
                if (children != null && children.Count > 0)
                    foreach (Item i in children)
                    {
                        i.Initialize(personalize);
                    }
            }
#else
            if(children !=null){
                Debug.LogError("Children is null");
                foreach (Item i in children)
                {
                    i.Initialize(personalize);
                }
            }
#endif
        }

        public override void Add(ListComponent listItem)
        {
            if (listItem.GetType() == typeof(Item))
                Add((Item)listItem);
            /**/
        }
        /*public void GenerateItems(ListComponent listItem, )
        {
            if (listItem.GetType() == typeof(Item))
            {
                foreach (Data.Item data in dataItems)
                {
                    List.Item item = CreateItem(data, parent.GetTransform());
                    Add((Item)listItem);
                }
            }

        }*/
        public void SetReference<T>(T reference)
        {
            this.reference = reference;
        }
        public T GetReference<T>()
        {
            if (this.reference.GetType() == typeof(T))
                return (T)this.reference;
            else
                return default(T);
        }

        public void Hide()
        {
            Animator anim = this.GetComponent<Animator>();
            if (anim != null)
            {
                anim.SetTrigger("Hide");
            }
        }
        public void Show()
        {
            gameObject.SetActive(true);
            Animator anim = this.GetComponent<Animator>();
            if (anim != null)
            {
                anim.SetTrigger("Show");
            }
        }
        public void OnHide()
        {
            gameObject.SetActive(false);
        }
    }
}
