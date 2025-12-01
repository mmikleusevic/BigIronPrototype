using System;
using UnityEngine;

namespace CombatRoom
{
    public interface ICombatInputSource
    {
        public event Action OnShoot;
        public event Action<Vector2> OnAim;
        public event Action OnReload;
    }
}