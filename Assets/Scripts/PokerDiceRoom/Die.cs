using System;
using System.Collections;
using Cysharp.Threading.Tasks.Triggers;
using Extensions;
using Managers;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

namespace PokerDiceRoom
{
    public class Die : MonoBehaviour
    {
        [field: SerializeField] public DieVisual DieVisual { get; private set; }
        
        [SerializeField] private AudioClip rollSound;
        
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

        public void ResetDieVisual()
        {
            DieVisual.ResetDieUITransform();
        }
        
        public void ResetDie(bool isFirstRoll)
        {
            IsHeld = !isFirstRoll;
            DieVisual.SetVisual(IsHeld);
        }

        public int Roll()
        {
            SoundManager.Instance.PlayVFX(rollSound);
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