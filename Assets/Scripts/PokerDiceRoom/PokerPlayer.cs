using System;
using UnityEngine;

namespace PokerDiceRoom
{
    [Serializable]
    public class PokerPlayer
    {
        [field: SerializeField] public string PlayerName { get; private set; }
        [field: SerializeField] public bool IsAI { get; private set; }
    }
}