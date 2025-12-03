using System;
using Cysharp.Threading.Tasks;
using Unity.Cinemachine;
using UnityEngine;

namespace CombatRoom
{
    public class CameraController : MonoBehaviour
    {
        [Header("Cameras")]
        [SerializeField] private CinemachineCamera overviewCamera;
        
        private CinemachineCamera playerCamera;

        public void SetPlayerCamera(CinemachineCamera playerCamera)
        {
            this.playerCamera = playerCamera;
        }
        
        public void SwitchToPlayerCamera()
        {
            playerCamera.Priority = 20;
            overviewCamera.Priority = 10;
        }

        public void SwitchToOverviewCamera()
        {
            overviewCamera.Priority = 20;
            playerCamera.Priority = 10;
        }
    }
}