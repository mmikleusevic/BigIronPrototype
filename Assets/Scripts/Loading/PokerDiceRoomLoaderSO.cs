using System.Threading.Tasks;
using Managers;
using MapRoom;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Loading
{
    [CreateAssetMenu(menuName = "Map Node Loaders/PokerDiceRoom")]
    public class PokerDiceRoomLoaderSO : MapNodeLoaderSO
    {
        [SerializeField] private AssetReference sceneReference;

        public override async Task LoadAsync(LevelNode node, LevelManager context)
        {
            await context.LoadSceneAsync(sceneReference);
        }
    }
}