using System;
using UnityEngine;

namespace Managers
{
    public class CameraManager : MonoBehaviour
    {
        public static CameraManager Instance { get; private set; }

        private float aimSensitivity;
        
        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            float aimSensitivity = PlayerPrefs.GetFloat(GameStrings.AIM_SENSITIVITY);
            this.aimSensitivity = aimSensitivity;
        }
        
        public void SetAimSensitivity(float aimSensitivity)
        {
            this.aimSensitivity = aimSensitivity;   
            PlayerPrefs.SetFloat(GameStrings.AIM_SENSITIVITY, this.aimSensitivity);
        }

        public float GetAimSensitivity()
        {
            return aimSensitivity;
        }
    }
}