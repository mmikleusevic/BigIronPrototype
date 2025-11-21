using System;
using Cysharp.Threading.Tasks;
using Extensions;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace PokerDiceRoom
{
    public class PokerDiceRoomUI : MonoBehaviour
    {
        [SerializeField] private GameObject pokerDiceRoomPanel;
        [SerializeField] private Slider goldSlider;
        [SerializeField] private TextMeshProUGUI goldText;
        [SerializeField] private Button continueButton;
        [SerializeField] private AssetReference pokerDiceSceneReference;
        
        private void Awake()
        {
            pokerDiceRoomPanel.SetActive(false);
        }

        private void OnEnable()
        {
            goldSlider.onValueChanged.AddListener(SetGoldText);
            continueButton.AddClickAsync(ContinueToPokerDiceRoom);
            PokerDiceRoomManager.Instance.OnPokerDiceRoomPressed += Open;
        }

        private void OnDisable()
        {
            goldSlider.onValueChanged.RemoveListener(SetGoldText);
            continueButton.onClick.RemoveAllListeners();
            PokerDiceRoomManager.Instance.OnPokerDiceRoomPressed -= Open;
        }
        
        private void Open()
        {
            pokerDiceRoomPanel.SetActive(true);
            
            goldSlider.minValue = 0;
            goldSlider.maxValue = GameManager.Instance.PlayerContext.Gold.GoldAmount;
        }

        private void SetGoldText(float gold)
        {
            goldText.text = $"{gold} Gold";
            PokerDiceRoomManager.Instance.SetWager((int)gold);
        }
        
        private async UniTask ContinueToPokerDiceRoom()
        {
            int playerWageredGold = PokerDiceRoomManager.Instance.PlayerGoldToWager;
            GameManager.Instance.PlayerContext.LoseGoldAmount(playerWageredGold);
            
            pokerDiceRoomPanel.SetActive(false);
            await LevelManager.Instance.LoadSceneAsync(pokerDiceSceneReference);
        }
    }
}