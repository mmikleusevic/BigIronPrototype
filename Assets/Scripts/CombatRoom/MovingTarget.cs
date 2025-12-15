using System;
using UnityEngine;

namespace CombatRoom
{
    public class MovingTarget : BaseTarget
    {
        private Vector3 startPosition;
        private Vector3 moveDirection;
        private float effectiveTravelDistance;
        private float speed;

        protected override void OnInitialize(TargetSpawnContext targetSpawnContext)
        {
            startPosition = targetSpawnContext.origin;
            effectiveTravelDistance = profile.travelDistance;
            
            switch (targetSpawnContext.movementAxis)
            {
                case MovementAxis.X:
                    moveDirection = Vector3.right * Mathf.Sign(-startPosition.x);
                    break;
                case MovementAxis.Y:
                    moveDirection = startPosition.y < (MAX_Y_HEIGHT + MIN_Y_HEIGHT) * 0.5f ? Vector3.up : Vector3.down;
                    if (moveDirection == Vector3.up)
                    {
                        float maxUpDistance = MAX_Y_HEIGHT - startPosition.y;
                        effectiveTravelDistance = Mathf.Min(profile.travelDistance, maxUpDistance);
                    }
                    else
                    {
                        float maxDownDistance = startPosition.y - MIN_Y_HEIGHT;
                        effectiveTravelDistance = Mathf.Min(profile.travelDistance, maxDownDistance);
                        
                        moveDirection = Vector3.up;
                    }
                    break;
            }
            transform.position = startPosition; 
            
            float halfLife = profile.lifetime * 0.5f;
            speed = effectiveTravelDistance / halfLife;
        }

        protected override void TickBehavior()
        {
            float currentDist = Mathf.PingPong(lifetimeTimer * speed, effectiveTravelDistance);
            
            Vector3 nextPos = startPosition + (moveDirection * currentDist);
            
            rb.MovePosition(nextPos);
        }
    }
}