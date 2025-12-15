using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CombatRoom
{
    public class OrbitingTarget : BaseTarget
    {
        private Transform pivotTransform;
        private float verticalSpeed;
        private float initialLocalY;
        private Camera mainCamera;

        private void Awake()
        {
            mainCamera = Camera.main;
        }

        protected override void OnInitialize(TargetSpawnContext targetSpawnContext)
        {
            GameObject pivot = new GameObject("OrbitPivot");
            pivotTransform = pivot.transform;
            
            Vector3 center = targetSpawnContext.centerTransform
                ? targetSpawnContext.centerTransform.position
                : targetSpawnContext.origin;
            
            pivotTransform.position = center;
            
            transform.SetParent(pivotTransform);
            
            initialLocalY = pivotTransform.localPosition.y;
            
            verticalSpeed = Random.Range(-0.5f, 0.5f);
        }

        protected override void TickBehavior()
        {
            transform.forward = -mainCamera.transform.forward;
            pivotTransform.Rotate(Vector3.up, profile.speed * Time.fixedDeltaTime * Mathf.Rad2Deg);
            
            Vector3 localPivotPosition = pivotTransform.localPosition;
            localPivotPosition.y += verticalSpeed * Time.fixedDeltaTime;
            
            localPivotPosition.y = Mathf.Clamp(localPivotPosition.y, initialLocalY - 2f, initialLocalY + 2f);
            
            if (localPivotPosition.y <= initialLocalY - 2f || localPivotPosition.y >= initialLocalY + 2f)
            {
                verticalSpeed = -verticalSpeed;
            }
            
            pivotTransform.localPosition = localPivotPosition;
        }
    }
}