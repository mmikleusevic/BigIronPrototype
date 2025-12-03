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
using Weapons;

namespace CombatRoom
{
    public class CombatRoomController : MonoBehaviour
    {
        [field: SerializeField] public BaseStateMachine BaseStateMachine { get; private set; }
        [field: SerializeField] public CombatTargetInputs CombatTargetInputs { get; private set; }
        [field: SerializeField] public CombatInputs CombatInputs { get; private set; }
        [field: SerializeField] public CombatInputRules CombatInputRules { get; private set; }
        [field: SerializeField] public GunUIController GunUIController { get; private set; }
        [field: SerializeField] public PlayerComboSystem PlayerComboSystem { get; private set; }
        [field: SerializeField] public CameraController CameraController { get; private set; }
        
        [SerializeField] private TargetSpawner targetSpawner;
        [SerializeField] private Vector3[] enemyPositions;
        
        private List<Combatant> ActiveCombatants { get; } = new List<Combatant>();
        public Queue<Combatant> TurnQueue { get; private set; } = new Queue<Combatant>();
        public Combatant CurrentCombatant { get; private set; }
        public CombatRoomEvents CombatRoomEvents { get; private set; } = new CombatRoomEvents();
        
        private readonly Dictionary<Combatant, Action> deathActions = new Dictionary<Combatant, Action>();
        private EnemyCombatant selectedEnemy;
        
        private void Start()
        {
            SetupPlayer();
            InitializeSystems();
            InitializeRoom().Forget();
        }
        
        private void InitializeSystems()
        {
            PlayerComboSystem.Initialize(CombatRoomEvents);
            
            PlayerComboSystem.OnComboFinished += HandleComboFinished;

            targetSpawner.Initialize(CombatRoomEvents, PlayerComboSystem, () => selectedEnemy);
        }
        
        private void OnDestroy()
        {
            if (PlayerComboSystem) PlayerComboSystem.OnComboFinished -= HandleComboFinished;
        }

        private async UniTask InitializeRoom()
        {
            await BaseStateMachine.ChangeState(new CombatRoomSetupState(this));
        }

        private void HandleComboFinished(float damageMultiplier)
        {
            if (CurrentCombatant is not PlayerCombatant player || !player.Gun) return;

            int totalDamage = (int)(player.Gun.Damage * damageMultiplier);
            Debug.Log($"Dealt {totalDamage} damage (Mult: {damageMultiplier})");

            if (selectedEnemy) selectedEnemy.Health.TakeDamage(totalDamage);
        }

        public void SpawnEnemies()
        {
            EncounterSO encounterSo = null;

            if (EncounterManager.Instance) encounterSo = EncounterManager.Instance.EncounterSO;
            if (!encounterSo) return;
            
            for (int i = 0; i < encounterSo.enemies.Length; i++)
            {
                if (i >= enemyPositions.Length) break;

                EnemyCombatant enemyPrefab = encounterSo.enemies[i];
                EnemyCombatant enemy = Instantiate(enemyPrefab, enemyPositions[i], Quaternion.identity);
                
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
            selectedEnemy = target;
        }

        private void SetupPlayer()
        {
            GameObject playerGameObject = null;
            if (GameManager.Instance) playerGameObject = GameManager.Instance.PlayerCombatant.gameObject;
            if (!playerGameObject) return;
            
            SceneManager.MoveGameObjectToScene(playerGameObject, gameObject.scene);
            PlayerCombatant playerCombatant = playerGameObject.GetComponent<PlayerCombatant>();
            
            CameraController.SetPlayerCamera(playerCombatant.PlayerCamera);
            GunUIController.SetGun(playerCombatant.Gun);
            
            RegisterPlayer(playerCombatant);
        }
        
        private void RegisterPlayer(PlayerCombatant player)
        {
            ActiveCombatants.Add(player);
            
            //TODO set gun

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