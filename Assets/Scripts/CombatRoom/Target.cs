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

        [SerializeField] private float orbitRadius = 2;
        [SerializeField] private float orbitSpeed = 1f;
        
        private Vector3 startPos;
        private float lifetime;
        private float timer;
        private bool isHit;

        private Vector3 startScale;
        private float orbitAngle;

        public void Initialize(TargetProfileSO targetProfileSo)
        {
            lifetime = targetProfileSo.lifetime;
            startScale = Vector3.one * targetProfileSo.radius;
            transform.localScale = startScale;

            startPos = transform.position;
            
            orbitAngle = Random.Range(0f, Mathf.PI * 2f);

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
            yield return new WaitForSeconds(lifetime);

            if (!isHit) OnTargetExpired?.Invoke(this);
            Destroy(gameObject);
        }

        private void Orbit()
        {
            orbitAngle += orbitSpeed * Time.deltaTime;
            
            float x = Mathf.Cos(orbitAngle) * orbitRadius;
            float z = Mathf.Sin(orbitAngle) * orbitRadius;

            transform.position = startPos + new Vector3(x, 0, z);
        }

        private void ScaleDownOverTime()
        {
            float t = Mathf.Clamp01(timer / lifetime);
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