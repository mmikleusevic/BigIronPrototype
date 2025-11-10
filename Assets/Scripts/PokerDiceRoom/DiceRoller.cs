using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PokerDiceRoom
{
    public class DiceRoller : MonoBehaviour, IClearable
    {
        [Header("Game Settings")]
        [SerializeField] private Die diePrefab;

        [SerializeField] private float rollDuration;
        [field: SerializeField] public int MaxRolls { get; private set; }
        
        [SerializeField] private int numberOfDice;
        [SerializeField] private RectTransform uiDiceContainerPrefab;
        
        public int CurrentRollNumber { get; set; }
        
        public Dictionary<string, List<Die>> PlayerDice { get; } = new Dictionary<string, List<Die>>();

        private Dictionary<string, int> PlayerNumberOfRolls { get; } = new Dictionary<string, int>();

        private readonly Quaternion[] sideRotations = new Quaternion[6]
        {
            Quaternion.Euler(90, 0, 0),     // 1
            Quaternion.Euler(0, 0, 0),      // 2 
            Quaternion.Euler(0, 90, 0),     // 3 
            Quaternion.Euler(0, 270, 0),    // 4 
            Quaternion.Euler(0, 180, 0),    // 5 
            Quaternion.Euler(360, -90, 90)  // 6
        };
        
        private void Awake()
        {
            //TODO remove comment GameManager.Instance.Clearables.Add(this);
        }

        public void Initialize(string[] players)
        {
            InitializePlayerRolls(players);
            InitializeDice(players);
        }

        private void InitializePlayerRolls(string[] players)
        {
            foreach (string player in players)
            {
                PlayerNumberOfRolls.Add(player, 0);
            }
        }

        private void InitializeDice(string[] players)
        {
            int xPosition = 0;
            foreach (string player in players)
            {
                RectTransform container = uiDiceContainerPrefab.GetPooledObject<RectTransform>(uiDiceContainerPrefab.parent);
                
                container.gameObject.SetActive(true);
                container.name = player;
                
                if (player == "Player")
                {
                    container.transform.SetAsLastSibling();
                }
                else
                {
                    container.transform.SetAsFirstSibling();
                }

                List<Die> dice = new List<Die>();
                for (int i = 0; i < numberOfDice; i++)
                {
                    Die die = diePrefab.GetPooledObject<Die>();
                    die.Initialize(container);
                    dice.Add(die);
                    
                    die.transform.position = new Vector3(xPosition, 0, 0);
                    xPosition += 10;
                }
                
                PlayerDice.Add(player, dice);
            }
        }
        
        public void ResetDiceHolds(string playerName)
        {
            List<Die> playerDice = PlayerDice[playerName];

            bool isFirstRoll = CurrentRollNumber == 0;
            
            foreach (Die die in playerDice)
            {
                die.ResetDie(isFirstRoll);
            }
        }
        
        public void ReturnToPool()
        {
            foreach (List<Die> playerDice in PlayerDice.Values)
            {
                playerDice.ReturnAllToPool();
            }
        }

        public void RollDice(string playerName, Action<List<int>> onComplete)
        {
            StartCoroutine(RollDiceCoroutine(playerName, onComplete));
        }

        private IEnumerator RollDiceCoroutine(string playerName, Action<List<int>> onComplete)
        {
            List<int> rolls = new List<int>();
            List<Coroutine> runningCoroutines = new List<Coroutine>();
    
            List<Die> playerDice = PlayerDice[playerName];
            
            foreach (Die die in playerDice)
            {
                if (die.IsHeld)
                {
                    rolls.Add(die.Value);
                    continue;
                }
                
                Coroutine coroutine = StartCoroutine(RollCoroutine(die, rolls));
                runningCoroutines.Add(coroutine);
            }

            foreach (Coroutine coroutine in runningCoroutines)
            {
                yield return coroutine;
            }

            onComplete?.Invoke(rolls);
        }

        private IEnumerator RollCoroutine(Die die, List<int> rolls)
        {
            int result = die.Roll();
            Quaternion targetRotation = sideRotations[result - 1];

            die.DieVisual.SetCamera(true);
            
            float elapsed = 0f;
            
            Vector3 spinAxis = Random.onUnitSphere.normalized;
            float spinSpeed = Random.Range(720f, 1080f);
            
            Quaternion startRotation = Random.rotation;
            die.DieVisual.transform.rotation = startRotation;
            
            while (elapsed < rollDuration * 0.7f)
            {
                die.DieVisual.transform.Rotate(spinAxis, spinSpeed * Time.deltaTime, Space.Self);
                elapsed += Time.deltaTime;
                yield return null;
            }
            
            elapsed = 0f;
            Quaternion currentRotation = die.transform.rotation;
    
            while (elapsed < rollDuration * 0.3f)
            {
                die.DieVisual.transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, elapsed / (rollDuration * 0.3f));
                elapsed += Time.deltaTime;
                yield return null;
            }

            die.DieVisual.transform.rotation = targetRotation;
            rolls.Add(result);
    
            yield return new WaitForSeconds(0.2f);
            die.DieVisual.SetCamera(false);
        }

        public void SetPlayerRolls(string playerName)
        {
            if (!PlayerNumberOfRolls.TryAdd(playerName, 0))
            {
                PlayerNumberOfRolls[playerName]++;
            }
            
            Debug.Log($"Player {playerName} has rolled, number of rolls is: {PlayerNumberOfRolls[playerName]}");
        }
        
        public bool AllPlayersRolled()
        {
            bool haveAllRolled = PlayerNumberOfRolls.Values.Min() == PlayerNumberOfRolls.Values.Max();

            if (haveAllRolled) CurrentRollNumber++;
            
            return haveAllRolled;
        }

        public int GetPlayerRolls(string playerName)
        {
            return PlayerNumberOfRolls[playerName];
        }

        public int ReturnNumberOfDice(string playerName)
        {
            return PlayerDice[playerName].Count;
        }
    }
}