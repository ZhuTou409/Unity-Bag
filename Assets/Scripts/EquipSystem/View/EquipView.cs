using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Bag
{
    public class EquipView
    {
        Transform view;
        List<Transform> weaponViewList;
        public EquipView(Transform view)
        {
            this.view = view;
            for (int i = 0; i < this.view.childCount; i++)
                weaponViewList.Add(this.view.GetChild(i));
        }
    }
}

