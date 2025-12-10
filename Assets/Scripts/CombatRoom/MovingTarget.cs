using System;
using UnityEngine;

namespace CombatRoom
{
    public class MovingTarget : BaseTarget
    {
        private Vector3 startPosition;
        private Vector3 endPosition;
        private MovementAxis movementAxis;
        private float halfLifetime;

        protected override void OnInitialize(TargetSpawnContext ctx)
        {
            startPosition = ctx.origin;
            halfLifetime = profile.lifetime * 0.5f;
            movementAxis = ctx.movementAxis;
            endPosition = startPosition;

            switch (movementAxis)
            {
                case MovementAxis.X:
                    endPosition.x = -startPosition.x; 
                    break;

                case MovementAxis.Y:
                    startPosition.y = Mathf.Max(startPosition.y, MIN_Y_HEIGHT);
            
                    endPosition.y = Math.Abs(startPosition.y - MAX_Y_HEIGHT) < 0.01f ? MIN_Y_HEIGHT : MAX_Y_HEIGHT;
            
                    transform.position = startPosition; 
                    break;
            }
            
            rb.linearVelocity = (endPosition - startPosition) / halfLifetime;
        }

        protected override void TickBehavior()
        {
            if (lifetimeTimer >= halfLifetime && lifetimeTimer - Time.deltaTime < halfLifetime)
            {
                rb.linearVelocity = -rb.linearVelocity;
            }

            if (lifetimeTimer >= profile.lifetime)
            {
                rb.linearVelocity = Vector3.zero;
            }
        }
    }
}