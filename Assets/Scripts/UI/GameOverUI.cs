using System;
using Cysharp.Threading.Tasks;
using Extensions;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace UI
{
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private Button mainMenuButton;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Image gameOverImage;
        [SerializeField] private Sprite loseSprite;
        [SerializeField] private Sprite winSprite;

        private void Awake()
        {
            gameOverPanel.SetActive(false);
        }

        private void OnEnable()
        {
            if (GameManager.Instance) GameManager.Instance.OnGameOver += GameOver;
            mainMenuButton.AddClickAsync(BackToMainMenu);
        }
        
        private void OnDisable()
        {
            if (GameManager.Instance) GameManager.Instance.OnGameOver -= GameOver;
            mainMenuButton.onClick.RemoveAllListeners();
        }
        
        private void GameOver(bool hasWon, bool isGameOver)
        {
            if (GameManager.Instance) GameManager.Instance.TogglePause();
            if (InputManager.Instance) InputManager.Instance.StartingMapsSetup();
            
            mainMenuButton.Select();

            gameOverImage.sprite = hasWon ? winSprite : loseSprite;
            
            gameOverPanel.SetActive(true);
        }
        
        private async UniTask BackToMainMenu()
        {
            if (LevelManager.Instance) await LevelManager.Instance.LoadMainMenu();
        }
    }
}