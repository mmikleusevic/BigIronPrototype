using System.Collections;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using EventRoom;
using Managers;
using MapRoom;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Loading
{
    [CreateAssetMenu(menuName = "Map Node Loaders/EventRoomSO")]
    public class EventRoomLoaderSO : MapNodeLoaderSO
    {
        [SerializeField] private EventSO[] possibleEvents;

        public override async UniTask LoadAsync(LevelNode node, LevelManager context)
        {
            int index = Random.Range(0, possibleEvents.Length);
            EventSO chosenEvent = possibleEvents[index];

            EventRoomManager.Instance?.DisplayCurrentEvent(chosenEvent);

            await UniTask.CompletedTask;
        }
    }
}
