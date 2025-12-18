using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Managers
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance { get; private set; }
        
        [SerializeField] private string[] mapsToDisableAtStart;
        [SerializeField] private InputActionAsset inputActionAsset;
        
        private List<string> enabledMapsBeforePause;
        
        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            StartingMapsSetup();
            
            GameManager.Instance.OnGameOver += OnGameOver;
        }

        private void OnDisable()
        {
            GameManager.Instance.OnGameOver -= OnGameOver;
        }

        public void StartingMapsSetup()
        {
            foreach (string mapName in mapsToDisableAtStart)
            {
                InputActionMap map = inputActionAsset.FindActionMap(mapName);
                map?.Disable();
            }
        }
        
        public void EnableOnlyUIMap()
        {
            enabledMapsBeforePause = new List<string>();
            
            foreach (InputActionMap map in inputActionAsset.actionMaps)
            {
                if (map.enabled) enabledMapsBeforePause.Add(map.name);
                
                map.Disable();
            }
            
            inputActionAsset.FindActionMap(GameStrings.UI).Enable();
        }
        
        public void RestoreMaps()
        {
            inputActionAsset.FindActionMap(GameStrings.UI).Disable();
            
            foreach (var mapName in enabledMapsBeforePause)
            {
                InputActionMap map = inputActionAsset.FindActionMap(mapName);
                map?.Enable();
            }

            enabledMapsBeforePause.Clear();
        }

        private void OnGameOver(bool hasWon, bool isGameOver)
        {
            EnableOnlyUIMap();
        }
    }
}