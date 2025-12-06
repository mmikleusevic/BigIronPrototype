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
        [SerializeField] private TextMeshProUGUI gameOverText;
        [SerializeField] private Button mainMenuButton;

        private void Awake()
        {
            gameOverPanel.SetActive(false);
        }

        private void OnEnable()
        {
            GameManager.Instance.OnGameOver += GameOver;
            mainMenuButton.AddClickAsync(BackToMainMenu);
        }
        
        private void OnDisable()
        {
            GameManager.Instance.OnGameOver -= GameOver;
            mainMenuButton.onClick.RemoveAllListeners();
        }
        
        private void GameOver(bool hasWon)
        {
            if (GameManager.Instance) GameManager.Instance.TogglePause();
            if (InputManager.Instance) InputManager.Instance.StartingMapsSetup();
            
            mainMenuButton.Select();
            gameOverPanel.SetActive(true);

            gameOverText.text = hasWon ? "You won!" : "You lost!";
        }
        
        private async UniTask BackToMainMenu()
        {
            if (LevelManager.Instance) await LevelManager.Instance.LoadMainMenu();
        }
    }
}