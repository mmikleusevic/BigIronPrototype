using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CombatRoom
{
    public class OrbitingTarget : BaseTarget
    {
        private Vector3 enemyPos;
        private float orbitAngle;
        private float initialAngle;

        protected override void OnInitialize(TargetSpawnContext ctx)
        {
            enemyPos = ctx.origin;

            float minAngle = Mathf.PI * 0.25f;
            float maxAngle = Mathf.PI * 1.75f;

            initialAngle = Random.Range(minAngle, maxAngle);

            // Set first orbit position
            float x = Mathf.Cos(initialAngle) * profile.orbitRadius;
            float z = Mathf.Sin(initialAngle) * profile.orbitRadius;
            float y = Mathf.Sin(initialAngle * profile.speed) * 0.5f;

            Vector3 startPos = enemyPos + new Vector3(x, y, z);

            // Move rigidbody instantly to orbit start
            rb.position = startPos;

            // Start orbit from that angle
            orbitAngle = initialAngle;
        }

        protected override void TickBehavior()
        {
            if (Time.deltaTime <= 0) return;
            
            orbitAngle += profile.speed * Time.deltaTime;

            float x = Mathf.Cos(orbitAngle) * profile.orbitRadius;
            float z = Mathf.Sin(orbitAngle) * profile.orbitRadius;
            float y = Mathf.Sin(orbitAngle * profile.speed) * 0.5f;

            Vector3 targetPosition = enemyPos + new Vector3(x, y, z);

            Vector3 velocity = (targetPosition - rb.position) / Time.deltaTime;

            rb.linearVelocity = velocity;
        }
    }
}