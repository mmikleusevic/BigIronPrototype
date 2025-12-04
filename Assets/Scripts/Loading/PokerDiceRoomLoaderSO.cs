using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Managers;
using MapRoom;
using PokerDiceRoom;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Loading
{
    [CreateAssetMenu(menuName = "Map Node Loaders/PokerDiceRoomSO")]
    public class PokerDiceRoomLoaderSO : MapNodeLoaderSO
    {
        public override async UniTask LoadAsync(LevelNode node, LevelManager context)
        {
            if (PokerDiceRoomManager.Instance) PokerDiceRoomManager.Instance.DisplayPokerDice();
            
            await UniTask.CompletedTask;
        }
    }
}