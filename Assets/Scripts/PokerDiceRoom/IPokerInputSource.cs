using System;
using UnityEngine;


namespace PokerDiceRoom
{
    public interface IPokerInputSource
    {
        public event Action OnRoll;
        public event Action OnHold;
        public event Action<Vector2> OnMove;
        public event Action OnSelect;
    }
}