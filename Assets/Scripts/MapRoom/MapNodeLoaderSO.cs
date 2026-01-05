using System.Collections;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Managers;
using UnityEngine;

namespace MapRoom
{
    public abstract class MapNodeLoaderSO : ScriptableObject
    {
        [SerializeField] private LevelNodeType levelNodeType;

        public LevelNodeType LevelNodeType => levelNodeType;
        public abstract UniTask LoadAsync(LevelNode node, LevelManager context);
    }
}