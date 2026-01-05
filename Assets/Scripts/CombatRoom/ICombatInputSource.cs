using System;
using UnityEngine;

namespace CombatRoom
{
    public interface ICombatInputSource
    {
        public event Action OnShoot;
        public event Action OnReload;
        public event Action OnEnd;
    }
}