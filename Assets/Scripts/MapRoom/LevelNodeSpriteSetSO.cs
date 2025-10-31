using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MapRoom
{
    [CreateAssetMenu(fileName = "LevelNodeSpriteSetSO", menuName = "Scriptable Objects/LevelNodeSpriteSetSO")]
    public class LevelNodeSpriteSetSO : ScriptableObject
    {
        [SerializeField] private List<LevelNodeSprite> nodeSprites = new List<LevelNodeSprite>();

        private Dictionary<LevelNodeType, Sprite> spriteLookup;

        private void Initialize()
        {
            if (spriteLookup != null) return;
        
            spriteLookup = nodeSprites.ToDictionary(levelNodeSprite => levelNodeSprite.LevelNodeType, 
                levelNodeSprite => levelNodeSprite.Sprite);
        }

        public Sprite GetSprite(LevelNodeType type)
        {
            Initialize();

            return spriteLookup.GetValueOrDefault(type);
        }
    }
}