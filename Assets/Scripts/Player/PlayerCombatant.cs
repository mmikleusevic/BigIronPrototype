using System;
using System.Collections;
using System.Collections.Generic;
using CombatRoom;
using Enemies;
using Managers;
using Unity.Cinemachine;
using UnityEngine;
using Weapons;

namespace Player
{
    public class PlayerCombatant : Combatant
    {
        [field: SerializeField] public Gun Gun { get; private set; }
        [field: SerializeField] public CinemachineCamera PlayerCamera { get; private set; }
        
        [SerializeField] private PlayerHealth playerHealth;
        [SerializeField] private Gold gold;
        [SerializeField] LayerMask targetLayerMask;
        [SerializeField] private float gunRotationSpeed = 10f;
        [SerializeField] private float maxAimDistance = 1000f;
        [SerializeField] private float maxHorizontalAngle = 45f;
        [SerializeField] private float maxVerticalAngle = 30f;
        
        public override Health Health => playerHealth;
        public override Gold Gold => gold;

        public HashSet<string> OwnedItemIds { get; } = new HashSet<string>();

        private Camera mainCamera;
        
        private Quaternion initialGunRotation;
        private Quaternion initialCameraRotation;
        private Quaternion initialPlayerRotation;
        
        private float xRotation;
        private float yRotation; 
        private bool isAiming;

        private void Awake()
        {
            initialGunRotation = Gun.transform.localRotation;
            initialCameraRotation = PlayerCamera.transform.localRotation;
            initialPlayerRotation = transform.localRotation;
        }

        private void Start()
        {
            if (EventRoomManager.Instance) EventRoomManager.Instance.UnlockInteractions();
        }

        private void OnEnable()
        {
            Gun.OnReloadStarted += StopAim;
            Gun.OnReloadFinished += StartAiming;
        }

        private void OnDisable()
        {
            Gun.OnReloadStarted -= StopAim;
            Gun.OnReloadFinished -= StartAiming;
        }

        private void LateUpdate()
        {
            if (!isAiming) return;
            
            AimGunAtCrosshair();
        }

        public void StartAiming()
        {
            if (!combatantAnimator) return;
            
            isAiming = true;
            
            combatantAnimator.Play(GameStrings.AIM);
            combatantAnimator.SetLayerWeight(1, 1f);
        }

        private void StopAim()
        {
            isAiming = false;
            
            if (Gun) Gun.transform.localRotation = initialGunRotation;
            if (PlayerCamera) PlayerCamera.transform.localRotation = initialCameraRotation;
        }

        private void StopAimingGoIdle()
        {
            if (!combatantAnimator) return;
            
            isAiming = false;
            
            combatantAnimator.SetLayerWeight(1, 0f);
            combatantAnimator.Play(GameStrings.IDLE);
        }
        
        public void HandleLook(Vector2 input)
        {
            if (!isAiming) return;
            
            xRotation -= input.y;
            xRotation = Mathf.Clamp(xRotation, -maxVerticalAngle, maxVerticalAngle);
            
            yRotation += input.x;
            yRotation = Mathf.Clamp(yRotation, -maxHorizontalAngle, maxHorizontalAngle);
            
            transform.localRotation = initialPlayerRotation * Quaternion.Euler(0f, yRotation, 0f);
            PlayerCamera.transform.localRotation = initialCameraRotation * Quaternion.Euler(xRotation, 0f, 0f);
        }
        
        private void AimGunAtCrosshair()
        {
            if (!Gun || !mainCamera || !isAiming) return;
            
            Vector3 targetPoint = GetCrosshairWorldPosition(); 
            Vector3 directionToTarget = (targetPoint - Gun.transform.position).normalized; 
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

            Gun.transform.rotation =
                Quaternion.Slerp(Gun.transform.rotation, targetRotation, gunRotationSpeed * Time.deltaTime);
        }
        
        private Vector3 GetCrosshairWorldPosition()
        {
            Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
            Ray ray = mainCamera.ScreenPointToRay(screenCenter);
            
            if (Physics.Raycast(ray, out RaycastHit hit, maxAimDistance, targetLayerMask))
            {
                return hit.point;
            }
            
            return ray.origin + ray.direction * maxAimDistance;
        }
        
        public void ExecuteShoot()
        {
            if (!Gun || !mainCamera) return;
            
            Vector3 targetPoint = GetCrosshairWorldPosition();
            Vector3 shootDirection = (targetPoint - Gun.ShootPoint.position).normalized;
            
            Gun.Shoot(Gun.ShootPoint.position, shootDirection);
        }
        
        public void RefreshState()
        {
            Health.RefreshState();
            Gold.RefreshState();
        }

        public void ResetAim()
        {
            Cursor.lockState = CursorLockMode.None;
            
            xRotation = 0f;
            yRotation = 0f;
            
            StopAimingGoIdle();
            
            if (Gun) Gun.transform.localRotation = initialGunRotation;
            if (PlayerCamera) PlayerCamera.transform.localRotation = initialCameraRotation;
            if (transform) transform.localRotation = initialPlayerRotation;
        }

        public void SetMainCamera()
        {
            mainCamera = Camera.main;
        }
    }
}