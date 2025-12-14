using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CombatRoom
{
    public class OrbitingTarget : BaseTarget
    {
        private float currentAngle;
        private Vector3 initialOffset;

        protected override void OnInitialize(TargetSpawnContext ctx)
        {
            float minAngle = Mathf.PI * 0.25f;
            float maxAngle = Mathf.PI * 1.75f;
            currentAngle = Random.Range(minAngle, maxAngle);
        }

        protected override void TickBehavior()
        {
            currentAngle += profile.speed * Time.fixedDeltaTime;
            
            float x = Mathf.Cos(currentAngle) * profile.orbitRadius;
            float z = Mathf.Sin(currentAngle) * profile.orbitRadius;
            
            float yOffset = Mathf.Sin(currentAngle * profile.speed) * 0.5f;
            
            Vector3 center = context.centerTransform ? context.centerTransform.position : context.origin;
            Vector3 targetPos = center + new Vector3(x, 1f + yOffset, z);
            
            rb.MovePosition(targetPos);
            rb.MoveRotation(Quaternion.LookRotation(center - transform.position));
        }
    }
}