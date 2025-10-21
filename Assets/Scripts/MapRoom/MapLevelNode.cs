using System;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace MapRoom
{
    public class MapLevelNode : MonoBehaviour
    {
        [SerializeField] private Button nodeButton;
        [SerializeField] private Image nodeImage;
        [SerializeField] private RectTransform rectTransform;
    
        public LevelNode LevelNode { get; private set; }
    
        private Action<MapLevelNode> onClickCallback;

        private void OnDisable()
        {
            RemoveListener();
        }

        public void Initialize(Sprite nodeSprite, LevelNode levelNode, Action<MapLevelNode> callback)
        {
            SetInteractable(false);
            AddListener(callback);
            nodeImage.sprite = nodeSprite;
            nodeImage.preserveAspect = true;
            LevelNode = levelNode;
        }
    
        private void AddListener(Action<MapLevelNode> callback)
        {
            onClickCallback = callback;
            nodeButton.onClick.AddListener(OnButtonClicked);
        }
    
        private void RemoveListener()
        {
            if (onClickCallback == null) return;
        
            nodeButton.onClick.RemoveListener(OnButtonClicked);
            onClickCallback = null;
        }
    
        private async void OnButtonClicked()
        {
            try
            {
                await LevelManager.Instance.LoadNode(LevelNode);
                onClickCallback?.Invoke(this);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to start playing game: {ex}");
            }
        }
    
        public void SetInteractable(bool value) => nodeButton.interactable = value;
    
        public void Highlight(bool value)
        {
            nodeImage.color = value ? Color.yellow : Color.white;
        }
    }
}