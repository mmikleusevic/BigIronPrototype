using System.Collections.Generic;
using EventRoom;
using Managers;
using MapRoom;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class EventRoomUI : MonoBehaviour
    {
        [SerializeField] private GameObject eventRoomPanel;
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private ChoiceButton choicePrefab;
        [SerializeField] private ContinueButton continueButton;
        [SerializeField] private Transform choiceParent;

        private Selectable firstSelectable;
        
        private void Awake()
        {
            eventRoomPanel.SetActive(false);
            continueButton.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            if (!EventRoomManager.Instance) return;
            
            EventRoomManager.Instance.OnEventLoaded += Initialize;
            EventRoomManager.Instance.OnChoiceResult += ShowChoiceResult;
            EventRoomManager.Instance.OnEventEnded += Hide;
        }

        private void OnDisable()
        {
            if (!EventRoomManager.Instance) return;
            
            EventRoomManager.Instance.OnEventLoaded -= Initialize;
            EventRoomManager.Instance.OnChoiceResult -= ShowChoiceResult;
            EventRoomManager.Instance.OnEventEnded -= Hide;
        }
        
        private void Initialize(EventSO eventSo)
        {
            titleText.text = eventSo.Title;
            descriptionText.text = eventSo.Description;

            DestroyButtons();
            firstSelectable = null;
            
            List<Selectable> choices = new List<Selectable>();
            
            foreach (EventChoice eventChoice in eventSo.Choices)
            {
                ChoiceButton choiceButton = Instantiate(choicePrefab, choicePrefab.transform.position, choicePrefab.transform.rotation, choiceParent);
                choiceButton.Initialize(eventChoice);
                
                choices.Add(choiceButton);

                if (!firstSelectable) firstSelectable = choiceButton;
            }
            
            int count = choices.Count;
            for (int i = 0; i < count; i++)
            {
                Navigation nav = new Navigation
                {
                    mode = Navigation.Mode.Explicit,
                    selectOnDown = choices[(i + 1) % count],
                    selectOnUp = choices[(i - 1 + count) % count],
                    selectOnLeft = null,
                    selectOnRight = null
                };
                
                choices[i].navigation = nav;
            }
            
            eventRoomPanel.SetActive(true);

            if (!firstSelectable) return;

            if (UIFocusManager.Instance) UIFocusManager.Instance.PushFocus(firstSelectable);
            firstSelectable.Select();
        }
        
        private void Hide()
        {
            continueButton.gameObject.SetActive(false);
            eventRoomPanel.SetActive(false);
            
            if (UIFocusManager.Instance) UIFocusManager.Instance.PopFocus();
            firstSelectable = null;
        }
        
        private void ShowChoiceResult(EventChoice choice, bool conditionsMet)
        {
            DestroyButtons();
            
            SetDescriptionText(choice, conditionsMet);
            RegisterContinueButton(choice);
        }

        private void RegisterContinueButton(EventChoice eventChoice)
        {
            if (UIFocusManager.Instance) UIFocusManager.Instance.PopFocus();
            continueButton.Initialize(eventChoice);
            continueButton.Select();
            if (UIFocusManager.Instance) UIFocusManager.Instance.PushFocus(continueButton);
        }

        private void SetDescriptionText(EventChoice choice, bool conditionsMet)
        {
            if (conditionsMet)
            {
                descriptionText.text = string.IsNullOrEmpty(choice.SuccessDescription)
                    ? choice.GeneratedResultDescription
                    : choice.SuccessDescription;
            }
            else
            {
                descriptionText.text = choice.FailDescription;
            }
        }

        private void DestroyButtons()
        {
            for (int i = 0; i < choiceParent.childCount; i++)
            {
                if (i < 2) continue;
                
                Transform child = choiceParent.GetChild(i);
                Destroy(child.gameObject);
            }
        }
    }
}
