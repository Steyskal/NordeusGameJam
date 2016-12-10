using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UI.MultiListView.List;
using UnityEngine.UI;
using System;

namespace UI.MultiListView
{
    public class AnimationFields : RequiredFields
    {

        public bool useAnimations = false;
        public AnimatorControllerParameter animatorController;

        //TODO:
        //1.Attach AnimationController on rows
        //      (Required component = RectTransform)
        //2.Use Animation on Collapse or without animation
        //3.Check speed

        /*
             
         * */


        #region Rows and Sublist Actions

        public void CollapseSublist(int index)
        {
            SubList[] sublists = itemsReference.GetComponentsInChildren<SubList>();
            if (sublists != null && sublists.Length > index)
            {
                sublists[index].CollapseChildren(true);
            }
        }

        #endregion
    }

}
