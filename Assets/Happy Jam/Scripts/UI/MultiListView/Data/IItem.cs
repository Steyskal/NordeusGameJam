using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UI.MultiListView.Data
{
    public interface IItem
    {
        Item.ItemType GetItemType();
        bool IsButton();
        string GetLabel();
        Data.Item Copy();
    }
}
