using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MultiListView.List
{
    /// <summary>
    /// Composite (nonLeaf) in Composite
    /// </summary>
    public class SubList : ListComponent
    {
        public List<ListComponent> children;
        public bool VerticalChildren = true;

        private bool _childrenCollapsed = false;

        public override void Add(ListComponent listItem)
        {
            if (children == null)
            {
                children = new List<ListComponent>();
            }
            children.Add(listItem);
        }

        public void Remove(ListComponent listItem)
        {
            children.Remove(listItem);
        }
        public IMultiList Get(int child)
        {
            IMultiList item = null;
            if (children != null && children.Count > child)
            {
                item = children[child];
            }
            return item;
        }
        public IMultiList GetById(int identity)
        {
            IMultiList item = null;
            foreach (IMultiList i in children)
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
            if (VerticalChildren && this.GetComponent<VerticalLayoutGroup>() == null)
                this.gameObject.AddComponent<VerticalLayoutGroup>();
            else if (!VerticalChildren && this.GetComponent<HorizontalLayoutGroup>() == null)
                this.gameObject.AddComponent<HorizontalLayoutGroup>();

            return true;
        }

        public override void Initialize(Personalization personalize)
        {
            Initialize();
            if (VerticalChildren)
            {
                VerticalLayoutGroup vlg = this.GetComponent<VerticalLayoutGroup>();
                if (vlg)
                    vlg.spacing = personalize.spaceBetweenRows;
            }
            else
            {
                HorizontalLayoutGroup hlg = this.GetComponent<HorizontalLayoutGroup>();
                if (hlg)
                    hlg.spacing = personalize.spaceBetweenRows;
            }



#if UNITY_EDITOR
            if (UnityEditor.EditorApplication.isPlaying)
            {
                Debug.Log("App.IsPlaying Called initialization of item");
                if (children == null)
                {
                    Debug.Log("Children are null");
                    children = GetComponentsInChildren<Item>().ToList<ListComponent>();
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
#endif
        }
        /// <summary>
        /// Collapses everything except header row
        /// </summary>
        /// <param name="collapse"></param>
        public void CollapseChildren(bool collapse)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                Row row = child.GetComponent<Row>();
                if (row != null && !row.isHeader)
                {
                    if (collapse)
                        row.Show();
                    else
                        row.Hide();
                }
            }
        }
        public void CollapseChildren()
        {
            CollapseChildren(_childrenCollapsed);
            _childrenCollapsed = !_childrenCollapsed;
        }
    }
}
