using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Enemies;
using Managers;
using Player;
using StateMachine;
using StateMachine.CombatStateMachine;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CombatRoom
{
    public class CombatRoomManager : MonoBehaviour
    {
        [field: SerializeField] public BaseStateMachine BaseStateMachine { get; private set; }
        [field: SerializeField] public CombatTargetInputs CombatTargetInputs { get; private set; }
        [field: SerializeField] public CombatInputs CombatInputs { get; private set; }
        [field: SerializeField] public CombatInputRules CombatInputRules { get; private set; }
        
        [SerializeField] private Vector3[] EnemyPositions;
        private List<Combatant> ActiveCombatants { get; } = new List<Combatant>();
        public Queue<Combatant> TurnQueue { get; private set; } = new Queue<Combatant>();
        public Combatant CurrentCombatant { get; private set; }
        public CombatRoomEvents CombatRoomEvents { get; private set; } = new CombatRoomEvents();
        public EnemyCombatant SelectedEnemy { get; private set; }
        
        private readonly Dictionary<Combatant, Action> deathActions = new Dictionary<Combatant, Action>();
        private void Start()
        {
            MovePlayerToCombatScene();
            
            InitializeRoom().Forget();
        }

        private async UniTask InitializeRoom()
        {
            await BaseStateMachine.ChangeState(new CombatRoomSetupState(this));
        }
        
        public void SpawnEnemies()
        {
            EncounterSO encounterSO = EncounterManager.Instance.EncounterSO;
            
            for (int i = 0; i < encounterSO.enemies.Length; i++)
            {
                if (i >= EnemyPositions.Length) break;

                EnemyCombatant enemyPrefab = encounterSO.enemies[i];
                EnemyCombatant enemy = Instantiate(enemyPrefab, EnemyPositions[i], Quaternion.identity);
                
                SceneManager.MoveGameObjectToScene(enemy.gameObject, gameObject.scene);
                
                ActiveCombatants.Add(enemy);
            }
        }
        
        public List<EnemyCombatant> GetAliveEnemies()
        {
            return ActiveCombatants
                .Where(c => c.Type == CombatantType.Enemy && !c.IsDead)
                .Cast<EnemyCombatant>()
                .ToList();
        }

        public void HandleTargetChosen(EnemyCombatant target)
        {
            SelectedEnemy = target;
        }

        private void MovePlayerToCombatScene()
        {
            GameObject playerGameObject = GameManager.Instance.PlayerCombatant.gameObject;
            SceneManager.MoveGameObjectToScene(playerGameObject, gameObject.scene);
            PlayerCombatant playerCombatant = playerGameObject.GetComponent<PlayerCombatant>();
            
            RegisterPlayer(playerCombatant);
        }
        
        private void RegisterPlayer(PlayerCombatant player)
        {
            ActiveCombatants.Add(player);

            SubscribeToDeath(player);
        }
        
        public void CalculateTurnOrder()
        {
            List<Combatant> sortedList = ActiveCombatants
                .Where(c => !c.IsDead)
                .OrderByDescending(c => c.Speed)
                .ToList();
            
            TurnQueue.Clear();
            foreach (Combatant combatant in sortedList)
            {
                TurnQueue.Enqueue(combatant);
            }
    
            Debug.Log("Turn Order: " + string.Join(" -> ", TurnQueue.Select(c => c.Name)));
        }
        
        public void HandleDeathCleanup(Combatant combatant)
        {
            ActiveCombatants.Remove(combatant);
        }
        
        private void SubscribeToDeath(Combatant combatant)
        {
            if (deathActions.ContainsKey(combatant)) return;
            
            DeathEventHandler handler = new DeathEventHandler(combatant, this);
            
            deathActions.Add(combatant, handler.DeathDelegate);
            
            combatant.Health.OnDied += handler.DeathDelegate;
        }

        public void GetNextAliveCombatant()
        {
            CurrentCombatant = null;

            while (!CurrentCombatant && TurnQueue.Count > 0)
            {
                Combatant next = TurnQueue.Dequeue();

                if (next.IsDead) continue;
                
                CurrentCombatant = next;
            }
        }
        
        public bool CheckWinCondition()
        {
            return ActiveCombatants.Where(c => c.Type == CombatantType.Enemy).All(e => e.IsDead);
        }

        public bool CheckLossCondition()
        {
            return ActiveCombatants.Where(c => c.Type == CombatantType.Player).All(p => p.IsDead);
        }
        
        public void UnsubscribeFromDeath(Combatant combatant)
        {
            deathActions.Remove(combatant);
        }
    }
}