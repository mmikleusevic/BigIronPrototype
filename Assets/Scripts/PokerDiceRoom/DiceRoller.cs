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
        public static event Action<int, int> OnNumberOfRollsChanged;
        
        [Header("Game Settings")]
        [SerializeField] private Die diePrefab;

        [SerializeField] private float rollDuration;
        [field: SerializeField] public int MaxRolls { get; private set; }
        
        [SerializeField] private int numberOfDice;
        [SerializeField] private RectTransform uiDiceContainerPrefab;
        
        public int CurrentRollNumber { get; set; }
        
        public Dictionary<PokerPlayer, List<Die>> PlayerDice { get; } = new Dictionary<PokerPlayer, List<Die>>();

        private Dictionary<PokerPlayer, int> PlayerNumberOfRolls { get; } = new Dictionary<PokerPlayer, int>();

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

        public void Initialize(PokerPlayer[] players)
        {
            InitializePlayerRolls(players);
            InitializeDice(players);
        }

        private void InitializePlayerRolls(PokerPlayer[] pokerPlayers)
        {
            foreach (PokerPlayer pokerPlayer in pokerPlayers)
            {
                PlayerNumberOfRolls.Add(pokerPlayer, 0);
            }
        }

        private void InitializeDice(PokerPlayer[] pokerPlayers)
        {
            int xPosition = 0;
            foreach (PokerPlayer pokerPlayer in pokerPlayers)
            {
                RectTransform container = uiDiceContainerPrefab.GetPooledObject<RectTransform>(uiDiceContainerPrefab.parent);
                
                container.gameObject.SetActive(true);
                container.name = pokerPlayer.PlayerName;
                
                if (pokerPlayer.PlayerName == "Player")
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
                
                PlayerDice.Add(pokerPlayer, dice);
            }
        }
        
        public void ResetDiceHolds(PokerPlayer player)
        {
            List<Die> playerDice = PlayerDice[player];

            bool isFirstRoll = GetNumberOfPlayerRolls(player) == 0;
            
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

        public void RollDice(PokerPlayer player, Action<List<int>> onComplete)
        {
            StartCoroutine(RollDiceCoroutine(player, onComplete));
        }

        private IEnumerator RollDiceCoroutine(PokerPlayer player, Action<List<int>> onComplete)
        {
            List<int> rolls = new List<int>();
            List<Coroutine> runningCoroutines = new List<Coroutine>();
    
            List<Die> playerDice = PlayerDice[player];
            
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

        public void SetPlayerRolls(PokerPlayer player)
        {
            if (!PlayerNumberOfRolls.TryAdd(player, 0))
            {
                PlayerNumberOfRolls[player]++;
            }

            int numberOfPlayerRolls = GetNumberOfPlayerRolls(player);
            
            OnNumberOfRollsChanged?.Invoke(numberOfPlayerRolls, MaxRolls);
            
            Debug.Log($"Player {player} has rolled, number of rolls is: {PlayerNumberOfRolls[player]}");
        }
        
        public bool HaveAllPlayersRolled() => PlayerNumberOfRolls.Values.Min() == PlayerNumberOfRolls.Values.Max();

        public void TryAdvanceRollPhase()
        {
            if (HaveAllPlayersRolled()) CurrentRollNumber++;
        }

        public int GetNumberOfPlayerRolls(PokerPlayer player)
        {
            return PlayerNumberOfRolls[player];
        }

        public int ReturnNumberOfDice(PokerPlayer player)
        {
            return PlayerDice[player].Count;
        }
    }
}