using System;
using Managers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MapRoom
{
    public class MapLevelNode : MonoBehaviour, ISelectHandler
    {
        public event Action<MapLevelNode> OnNodeSelected;
        
        [SerializeField] private Button nodeButton;
        [SerializeField] private Image nodeImage;
        [SerializeField] private RectTransform rectTransform;
    
        public Button NodeButton => nodeButton;
        
        public LevelNode LevelNode { get; private set; }
    
        private Action<MapLevelNode> onClickCallback;

        private void OnDisable()
        {
            SetInteractable(false);
            Highlight(false);
        }

        private void OnDestroy()
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
                if (!LevelManager.Instance) return;
                await LevelManager.Instance.LoadNode(LevelNode);
                onClickCallback?.Invoke(this);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to start playing game: {ex}");
            }
        }

        public void SetInteractable(bool isInteractable)
        {
            nodeButton.interactable = isInteractable;

            if (isInteractable) return;
            
            Navigation navigation = nodeButton.navigation;
            navigation.mode = Navigation.Mode.None;
            nodeButton.navigation = navigation;
        }
        
        public void SetNavigation(Button leftNeighbor, Button rightNeighbor)
        {
            Navigation navigation = new Navigation();
            navigation.mode = Navigation.Mode.Explicit;
            
            navigation.selectOnLeft = leftNeighbor;
            navigation.selectOnRight = rightNeighbor;

            nodeButton.navigation = navigation;
        }
        
        public void Highlight(bool value) => nodeImage.color = value ? Color.yellow : Color.white;
        public void OnSelect(BaseEventData eventData)
        {
            OnNodeSelected?.Invoke(this);
        }
    }
}