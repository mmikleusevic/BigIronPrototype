using System;
using System.Collections;
using UnityEngine;

namespace CombatRoom
{
    public abstract class BaseTarget : MonoBehaviour
    {
        protected const float MIN_Y_HEIGHT = 1f; 
        protected const float MAX_Y_HEIGHT = 5.0f;

        [SerializeField] protected Rigidbody rb;
        
        public event Action<BaseTarget> OnTargetHit;
        public event Action<BaseTarget> OnTargetExpired;

        protected TargetProfileSO profile;
        protected float lifetimeTimer;
        protected TargetSpawnContext context; 
        
        private bool isHit;
        
        public void Initialize(TargetSpawnContext targetSpawnContext)
        {
            context = targetSpawnContext;
            profile = targetSpawnContext.profile;
            transform.localScale = Vector3.one * profile.scale;
            
            rb.isKinematic = true; 
            rb.useGravity = false;

            OnInitialize(targetSpawnContext);
        }
        
        protected abstract void OnInitialize(TargetSpawnContext ctx);

        protected virtual void Update()
        {
            lifetimeTimer += Time.deltaTime;
            
            float t = Mathf.Clamp01(lifetimeTimer / profile.lifetime);
            transform.localScale = Vector3.Lerp(Vector3.one * profile.scale, Vector3.zero, t);
            
            if (lifetimeTimer >= profile.lifetime && !isHit)
            {
                OnTargetExpired?.Invoke(this);
                Destroy(gameObject);
            }
        }

        protected virtual void FixedUpdate()
        {
            TickBehavior();
        }

        protected abstract void TickBehavior();

        public virtual void Hit()
        {
            if (isHit) return;

            isHit = true;
            OnTargetHit?.Invoke(this);
            Destroy(gameObject);
        }
    }
}