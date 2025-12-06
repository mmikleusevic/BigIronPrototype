using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CombatRoom
{
    public class Target : MonoBehaviour
    {
        public event Action<Target> OnTargetHit;
        public event Action<Target> OnTargetExpired;

        private TargetProfileSO targetProfileSO;
        
        private Vector3 enemyPosition;
        private Vector3 startScale;
        private float orbitAngle;
        private float timer;
        private bool isHit;

        public void Initialize(TargetProfileSO targetProfileSO, Vector3 enemyPosition, float initialAngle)
        {
            this.targetProfileSO = targetProfileSO;

            startScale = Vector3.one * targetProfileSO.scale;
            transform.localScale = startScale;
            orbitAngle = initialAngle;
            this.enemyPosition = enemyPosition;

            StartCoroutine(LifetimeCountdown());
        }

        private void Update()
        {
            timer += Time.deltaTime;

            Orbit();
            ScaleDownOverTime();
        }

        private IEnumerator LifetimeCountdown()
        {
            yield return new WaitForSeconds(targetProfileSO.lifetime);

            if (!isHit) OnTargetExpired?.Invoke(this);
            Destroy(gameObject);
        }

        private void Orbit()
        {
            orbitAngle += targetProfileSO.speed * Time.deltaTime;

            float x = Mathf.Cos(orbitAngle) * targetProfileSO.orbitRadius;
            float z = Mathf.Sin(orbitAngle) * targetProfileSO.orbitRadius;
            float y = Mathf.Sin(orbitAngle * targetProfileSO.speed) * 0.5f;

            transform.position = enemyPosition + new Vector3(x, y, z);
        }

        private void ScaleDownOverTime()
        {
            float t = Mathf.Clamp01(timer / targetProfileSO.lifetime);
            transform.localScale = Vector3.Lerp(startScale, Vector3.zero, t);
        }

        public void Hit()
        {
            if (isHit) return;

            isHit = true;
            OnTargetHit?.Invoke(this);
            Destroy(gameObject);
        }
    }
}