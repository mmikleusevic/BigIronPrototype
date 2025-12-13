using System;
using System.Collections;
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
        [SerializeField] private float rotationSpeed = 5f;
        
        public override Health Health => playerHealth;
        public override Gold Gold => gold;

        private Quaternion initialGunRotation;
        private Quaternion initialCameraRotation;
        
        private float xRotation;
        private bool isAimingActive; 

        private void Awake()
        {
            initialGunRotation = Gun.transform.localRotation;
            initialCameraRotation = PlayerCamera.transform.localRotation;
        }

        public void HandleLook(Vector2 input)
        {
            xRotation -= input.y;
            xRotation = Mathf.Clamp(xRotation, -60f, 60f);

            PlayerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            transform.Rotate(Vector3.up * input.x);
            
            Vector3 lookDir = PlayerCamera.transform.forward;
            Quaternion targetRot = Quaternion.LookRotation(lookDir);
            Gun.transform.rotation = Quaternion.Slerp(
                Gun.transform.rotation,
                targetRot,
                10f * Time.deltaTime
            );
        }
        
        public void ExecuteShoot()
        {
            if (!Gun || !PlayerCamera) return;
            
            Vector3 rayOrigin = PlayerCamera.transform.position; 
            Vector3 rayDirection = PlayerCamera.transform.forward; 
            
            Gun.Shoot(rayOrigin, rayDirection);
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
            if (Gun) Gun.transform.rotation = initialGunRotation;
            if (PlayerCamera) PlayerCamera.transform.rotation = initialCameraRotation;
        }
        
        public void PlayAnimation(string state)
        {
            CombatantAnimator.Play(state, 0, 0f);
            CombatantAnimator.speed = 1f;
        }

        public void RotateBy(float degrees)
        {
            StartCoroutine(Rotate(degrees));
        }

        private IEnumerator Rotate(float degrees)
        {
            Quaternion targetRotation = Quaternion.Euler(0, degrees, 0);

            while (Quaternion.Angle(transform.rotation, targetRotation) > 1f)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                yield return null;
            }

            transform.rotation = targetRotation;
        }
    }
}