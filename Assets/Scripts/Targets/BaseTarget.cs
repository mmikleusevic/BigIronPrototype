using System;
using CombatRoom;
using Managers;
using UnityEngine;

namespace Targets
{
    public abstract class BaseTarget : MonoBehaviour
    {
        protected const float MIN_Y_HEIGHT = 1f; 
        protected const float MAX_Y_HEIGHT = 5.0f;

        [SerializeField] protected Rigidbody rb;
        
        public event Action<BaseTarget> OnTargetHit;
        public event Action<BaseTarget> OnTargetExpired;

        protected TargetStats stats;
        protected float lifetimeTimer;
        protected TargetSpawnContext context; 
        
        private bool isHit;

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        public void Initialize(TargetSpawnContext targetSpawnContext)
        {
            stats = TargetStats.FromProfile(targetSpawnContext.profile); 
            
            TargetModifierManager.Instance.ApplyModifiers(ref stats);
            
            context = targetSpawnContext;
            transform.localScale = Vector3.one * stats.scale;
            
            rb.isKinematic = true; 
            rb.useGravity = false;

            OnInitialize(targetSpawnContext);
            
            gameObject.SetActive(true);
        }
        
        protected abstract void OnInitialize(TargetSpawnContext targetSpawnContext);

        protected virtual void Update()
        {
            lifetimeTimer += Time.deltaTime;
            
            float t = Mathf.Clamp01(lifetimeTimer / stats.lifetime);
            transform.localScale = Vector3.Lerp(Vector3.one * stats.scale, Vector3.zero, t);
            
            if (lifetimeTimer >= stats.lifetime && !isHit)
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