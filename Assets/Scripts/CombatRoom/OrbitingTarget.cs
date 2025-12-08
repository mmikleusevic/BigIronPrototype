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

        protected override void OnInitialize(TargetSpawnContext ctx)
        {
            enemyPos = ctx.origin;
            orbitAngle = ctx.initialAngle;
        }

        protected override void TickBehavior()
        {
            orbitAngle += profile.speed * Time.deltaTime;

            float x = Mathf.Cos(orbitAngle) * profile.orbitRadius;
            float z = Mathf.Sin(orbitAngle) * profile.orbitRadius;
            float y = Mathf.Sin(orbitAngle * profile.speed) * 0.5f;

            transform.position = enemyPos + new Vector3(x, y, z);
        }
    }
}