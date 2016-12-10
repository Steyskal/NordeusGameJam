using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UI.MultiListView.List
{
    public interface IMultiList
    {
        void Operation();
        void Add(ListComponent listItem);
        bool Initialize();
        int GetId();
        void SetId(int id);
        Transform GetTransform();
    }
}
