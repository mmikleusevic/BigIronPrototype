using System;
using System.Collections;
using Cysharp.Threading.Tasks.Triggers;
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
        public bool IsHeld { get; private set; }
        
        public void OnEnable()
        {
            ResetDie(true);
        }

        public void Initialize(RectTransform uiContainer)
        {
            DieVisual.Initialize(uiContainer);
        }
        
        public void ResetDie(bool isFirstRoll)
        {
            IsHeld = !isFirstRoll;
            DieVisual.SetVisual(IsHeld);
        }

        public int Roll()
        {
            Value = Random.Range(1, 7);
            return Value;
        }

        public void ToggleDie()
        {
            IsHeld = !IsHeld;
            DieVisual.SetVisual(IsHeld);
        }
    }
}