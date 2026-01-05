using System;
using UnityEngine;

namespace ShopRoom
{
    public abstract class ShopItemDataSO : ScriptableObject
    {
        public string itemName;
        public int price;
        public float itemModifier;
        public abstract string ItemDescription { get; }
    }
}