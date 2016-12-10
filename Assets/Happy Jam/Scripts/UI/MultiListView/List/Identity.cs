using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UI.MultiListView.List
{
    /// <summary>
    /// Mozda suvišno čak
    /// </summary>
    public class Identity : MonoBehaviour
    {
        public enum IdentityType
        {
            ListRow,
            ListItem
        }
        [SerializeField]
        protected int _id = 0;


        private int _level = 1;
        public int Level
        {
            get
            {
                _level = CheckLevel();
                return _level;
            }
            set
            {
                _level = value;
            }
        }
        public int CheckLevel()
        {
            if(transform.parent == null || this.transform.parent.GetComponent<Identity>() == null)
            {
                return 0;
            }

            Identity parentId = this.transform.parent.GetComponent<Identity>();
            if (parentId._id == -1)
                return 1;
            else
                return 1 + parentId.CheckLevel();
        }
    }
}
