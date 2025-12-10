using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Enemies;
using Managers;
using Player;
using StateMachine;
using StateMachine.CombatStateMachine;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.AddressableAssets;
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
        [field: SerializeField] public AssetReference CombatRoomAssetReference { get; private set; }
        [field: SerializeField] public AssetReference GameAssetReference { get; private set; }
        
        [SerializeField] private Transform[] enemyTransforms;
        [SerializeField] private Transform playerTransforms;
        [SerializeField] private Transform overviewCameraTransform;
        
        private List<Combatant> ActiveCombatants { get; } = new List<Combatant>();
        public Queue<Combatant> TurnQueue { get; } = new Queue<Combatant>();
        public Combatant CurrentCombatant { get; private set; }
        public CombatRoomEvents CombatRoomEvents { get; } = new CombatRoomEvents();
        public EnemyCombatant SelectedEnemy { get; private set; }
        
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

            int totalDamage = (int)(player.Data.damage * damageMultiplier);
            Debug.Log($"Dealt {totalDamage} damage (Mult: {damageMultiplier})");

            if (SelectedEnemy) SelectedEnemy.TakeDamage(player, SelectedEnemy, totalDamage);
        }

        public void SpawnEnemies()
        {
            EncounterSO encounterSo = null;

            if (EncounterManager.Instance) encounterSo = EncounterManager.Instance.EncounterSO;
            if (!encounterSo) return;
            
            for (int i = 0; i < encounterSo.enemies.Length; i++)
            {
                if (i >= enemyTransforms.Length) break;

                EnemyCombatant enemyPrefab = encounterSo.enemies[i];
                EnemyCombatant enemy = Instantiate(enemyPrefab, enemyTransforms[i].position, enemyTransforms[i].rotation);
                enemy.InitializeTargetSpawner(PlayerComboSystem);
                
                SceneManager.MoveGameObjectToScene(enemy.gameObject, gameObject.scene);
                
                ActiveCombatants.Add(enemy);
            }
        }
        
        public List<EnemyCombatant> GetAliveEnemies()
        {
            return ActiveCombatants
                .Where(c => c.Data.combatantType == CombatantType.Enemy && !c.IsDead)
                .Cast<EnemyCombatant>()
                .ToList();
        }

        public void HandleTargetChosen(EnemyCombatant target)
        {
            SelectedEnemy = target;
        }

        private void SetupPlayer()
        {
            GameObject playerGameObject = null;
            if (GameManager.Instance) playerGameObject = GameManager.Instance.PlayerCombatant.gameObject;
            if (!playerGameObject) return;

            playerGameObject.transform.position = playerTransforms.position;
            playerGameObject.transform.rotation = playerTransforms.rotation;
            
            PlayerCombatant playerCombatant = null;
            if (GameManager.Instance) playerCombatant = GameManager.Instance.PlayerCombatant;
            if (!playerCombatant) return;
            
            CameraController.SetPlayerCamera(playerCombatant.PlayerCamera, playerTransforms, overviewCameraTransform);
            GunUIController.SetGun(playerCombatant.Gun);
            
            RegisterPlayer(playerCombatant);
        }
        
        private void RegisterPlayer(PlayerCombatant player)
        {
            ActiveCombatants.Add(player);
        }
        
        public void CalculateTurnOrder()
        {
            List<Combatant> sortedList = ActiveCombatants
                .Where(c => !c.IsDead)
                .OrderByDescending(c => c.Data.speed)
                .ToList();
            
            TurnQueue.Clear();
            foreach (Combatant combatant in sortedList)
            {
                TurnQueue.Enqueue(combatant);
            }
    
            Debug.Log("Turn Order: " + string.Join(" -> ", TurnQueue.Select(c => c.Data.name)));
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

        public void ToggleEnemyVisibility(bool isVisible)
        {
            foreach (Combatant combatant in ActiveCombatants)
            {
                if (combatant == CurrentCombatant) continue;

                combatant?.ToggleVisibility(isVisible);
            }
        }
        
        public bool CheckWinCondition()
        {
            return ActiveCombatants.Where(c => c.Data.combatantType == CombatantType.Enemy).All(e => e.IsDead);
        }

        public bool CheckLossCondition()
        {
            return ActiveCombatants.Where(c => c.Data.combatantType == CombatantType.Player).All(p => p.IsDead);
        }
    }
}