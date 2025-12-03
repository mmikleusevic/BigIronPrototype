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
            if (PokerDiceRoomManager.Instance) PokerDiceRoomManager.Instance.OnPokerDiceRoomPressed += Open;
        }

        private void OnDisable()
        {
            goldSlider.onValueChanged.RemoveListener(SetGoldText);
            continueButton.onClick.RemoveAllListeners();
            if (PokerDiceRoomManager.Instance) PokerDiceRoomManager.Instance.OnPokerDiceRoomPressed -= Open;
        }
        
        private void Open()
        {
            pokerDiceRoomPanel.SetActive(true);
            
            goldSlider.minValue = 0;

            if (!GameManager.Instance) return;
            goldSlider.maxValue = GameManager.Instance.PlayerCombatant.Gold.GoldAmount;
        }

        private void SetGoldText(float gold)
        {
            goldText.text = $"{gold} Gold";
            if (PokerDiceRoomManager.Instance) PokerDiceRoomManager.Instance.SetWager((int)gold);
        }
        
        private async UniTask ContinueToPokerDiceRoom()
        {
            if (!PokerDiceRoomManager.Instance) return;
            
            int playerWageredGold = PokerDiceRoomManager.Instance.PlayerGoldToWager;
            GameManager.Instance.PlayerCombatant.LoseGoldAmount(playerWageredGold);
            
            await LevelManager.Instance.LoadSceneAsync(pokerDiceSceneReference);
            
            pokerDiceRoomPanel.SetActive(false);
        }
    }
}