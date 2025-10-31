using System.Threading.Tasks;
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
        [SerializeField] private AssetReference sceneReference;

        public override async UniTask LoadAsync(LevelNode node, LevelManager context)
        {
             await context.LoadSceneAsync(sceneReference);
        }
    }
}