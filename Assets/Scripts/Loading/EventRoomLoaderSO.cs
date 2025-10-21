using System.Collections;
using System.Threading.Tasks;
using EventRoom;
using Managers;
using MapRoom;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Loading
{
    [CreateAssetMenu(menuName = "Map Node Loaders/EventRoom")]
    public class EventRoomLoaderSO : MapNodeLoaderSO
    {
        [SerializeField] private EventDataSO[] possibleEvents;

        public override async Task LoadAsync(LevelNode node, LevelManager context)
        {
            int index = Random.Range(0, possibleEvents.Length);
            EventDataSO chosenEvent = possibleEvents[index];

            EventRoomManager.Instance.DisplayCurrentEvent(chosenEvent);

            await Task.CompletedTask;
        }
    }
}
