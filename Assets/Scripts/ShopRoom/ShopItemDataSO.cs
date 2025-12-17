using System;
using UnityEngine;

namespace ShopRoom
{
    public abstract class ShopItemDataSO : ScriptableObject
    {
        public string itemName;
        [NonSerialized] 
        public string itemDescription;
        public int price;
        public float itemModifier;

        protected abstract void OnValidate();
    }
}