using System.Threading.Tasks;
using CombatRoom;
using Cysharp.Threading.Tasks;
using Managers;
using MapRoom;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Loading
{
    [CreateAssetMenu(menuName = "Map Node Loaders/CombatRoomSO")]
    public class CombatRoomLoaderSO : MapNodeLoaderSO
    {
        [SerializeField] private EncounterTableSO combatTableSO;
        [SerializeField] private AssetReference sceneReference;

        public override async UniTask LoadAsync(LevelNode node, LevelManager context)
        {
            int randomIndex = Random.Range(0, combatTableSO.encounters.Count);
            EncounterSO randomEncounter = combatTableSO.encounters[randomIndex];
            
            EncounterManager.Instance.SetNextEncounter(randomEncounter);
            
            await context.LoadSceneAsync(sceneReference);
        }
    }
}