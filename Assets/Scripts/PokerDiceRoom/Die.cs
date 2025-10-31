using System;
using Extensions;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

namespace PokerDiceRoom
{
    public class Die : MonoBehaviour
    {
        [field: SerializeField] public DieVisual DieVisual { get; private set; }
        
        public int Value { get; private set; }
        private bool IsHeld { get; set; }
        
        public void OnEnable()
        {
            ResetDie();
        }
        
        public void ResetDie()
        {
            Value = 1;
            IsHeld = false;
            DieVisual.SetVisual(IsHeld);
        }

        public int Roll()
        {
            if (!IsHeld) Value = Random.Range(1, 7);
            return Value;
        }

        public void ToggleDie()
        {
            IsHeld = !IsHeld;
            DieVisual.SetVisual(IsHeld);
        }
    }
}