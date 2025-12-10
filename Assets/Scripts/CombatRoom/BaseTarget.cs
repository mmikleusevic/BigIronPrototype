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
        private bool isHit;
        
        public void Initialize(TargetSpawnContext ctx)
        {
            profile = ctx.profile;

            transform.localScale = Vector3.one * profile.scale;

            OnInitialize(ctx);

            StartCoroutine(LifetimeCountdown());
        }
        
        protected abstract void OnInitialize(TargetSpawnContext ctx);

        private IEnumerator LifetimeCountdown()
        {
            yield return new WaitForSeconds(profile.lifetime);

            if (!isHit) OnTargetExpired?.Invoke(this);

            Destroy(gameObject);
        }

        protected virtual void Update()
        {
            lifetimeTimer += Time.deltaTime;
            TickBehavior();
            ScaleDownOverTime();
        }

        protected abstract void TickBehavior();

        private void ScaleDownOverTime()
        {
            float t = Mathf.Clamp01(lifetimeTimer / profile.lifetime);
            transform.localScale = Vector3.Lerp(Vector3.one * profile.scale, Vector3.zero, t);
        }

        public virtual void Hit()
        {
            if (isHit) return;

            isHit = true;
            OnTargetHit?.Invoke(this);
            Destroy(gameObject);
        }
    }
}