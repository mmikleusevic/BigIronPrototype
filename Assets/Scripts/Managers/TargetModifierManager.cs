using System;
using System.Collections.Generic;
using CombatRoom;
using Targets;
using UnityEngine;

namespace Managers
{
    public class TargetModifierManager : MonoBehaviour
    {
        public static TargetModifierManager Instance { get; private set; }

        private readonly List<ITargetModifier> modifiers = new  List<ITargetModifier>();

        private void Awake()
        {
            Instance = this;
        }

        public void AddModifier(ITargetModifier modifier)
        {
            modifiers.Add(modifier);
        }

        public void RemoveModifier(ITargetModifier modifier)
        {
            modifiers.Remove(modifier);
        }

        public void ApplyModifiers(ref TargetStats stats)
        {
            foreach (ITargetModifier modifier in modifiers)
            {
                modifier.Apply(ref stats);
            }
        }

        public void RemoveAllModifiers()
        {
            modifiers.Clear();
        }
    }
}