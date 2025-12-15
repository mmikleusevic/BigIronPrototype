using System;
using Managers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Button))]
    public class ButtonSound : MonoBehaviour
    {
        [SerializeField] private AudioClip clickSound;
    
        private Button button;

        private void Awake()
        {
            button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            button.onClick.AddListener(PlaySound);
        }

        private void OnDisable()
        {
            button.onClick.RemoveListener(PlaySound);
        }

        private void PlaySound()
        {
            SoundManager.Instance.PlayVFX(clickSound); 
        }
    }
}