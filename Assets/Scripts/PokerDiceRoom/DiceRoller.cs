using System;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using Managers;
using UnityEngine;

namespace PokerDiceRoom
{
    public class DiceRoller : MonoBehaviour, IClearable
    {
        [Header("Game Settings")]
        [SerializeField] private Die diePrefab;
        [field: SerializeField] public int MaxRolls { get; private set; }
        
        [SerializeField] private int numberOfDice;
        
        public int CurrentRollNumber { get; set; }
        
        public List<Die> Dice { get; } = new List<Die>();

        private Dictionary<string, int> PlayerNumberOfRolls { get; } = new Dictionary<string, int>();
        
        private void Awake()
        {
            //TODO remove comment GameManager.Instance.Clearables.Add(this);
            InitializeDice();
        }

        public void Initialize(string[] players)
        {
            InitializePlayerRolls(players);
        }

        private void InitializePlayerRolls(string[] players)
        {
            foreach (string player in players)
            {
                PlayerNumberOfRolls.Add(player, 0);
            }
        }

        private void InitializeDice()
        {
            for (int i = 0; i < numberOfDice; i++)
            {
                Die die = diePrefab.GetPooledObject<Die>();
                Dice.Add(die);
            }
        }
        
        public void ResetDiceHolds()
        {
            foreach (Die die in Dice) die.ResetDie();
        }
        
        public void ToggleDieHold(int dieIndex)
        {
            if (dieIndex >= 0 && dieIndex < Dice.Count)
            {
                Dice[dieIndex].ToggleDie();
            }
        }
        
        public void ReturnToPool()
        {
            Dice.ReturnAllToPool();
        }

        public List<int> RollDice()
        {
            List<int> rolls = new List<int>();
            
            foreach (Die die in Dice)
            {
                int value = die.Roll();
                rolls.Add(value);   
            }

            return rolls;
        }

        public void SetPlayerRolls(string player)
        {
            if (!PlayerNumberOfRolls.TryAdd(player, 0))
            {
                PlayerNumberOfRolls[player]++;
            }
            
            Debug.Log($"Player {player} has rolled, number of rolls is: {PlayerNumberOfRolls[player]}");
        }
        
        public bool AllPlayersRolled()
        {
            bool haveAllRolled = PlayerNumberOfRolls.Values.Min() == PlayerNumberOfRolls.Values.Max();

            if (haveAllRolled) CurrentRollNumber++;
            
            return haveAllRolled;
        }

        public int GetPlayerRolls(string player)
        {
            return PlayerNumberOfRolls[player];
        }
    }
}