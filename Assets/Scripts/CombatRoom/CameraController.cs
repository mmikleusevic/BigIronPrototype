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

        public void SetPlayerCamera(CinemachineCamera playerCamera, Transform playerTransform, Transform overviewTransform)
        {
            this.playerCamera = playerCamera;
            overviewCamera.transform.position = playerTransform.position + overviewTransform.position;
            overviewCamera.transform.rotation = overviewTransform.rotation;
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