using System;
using UnityEngine;

namespace CombatRoom
{
    public interface ICombatTargetInputSource
    {
        public event Action OnShootSelected;
        public event Action<Vector2> OnMove;
        public event Action OnConfirm;
        public event Action OnCancel;
    }
}