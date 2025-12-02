using System;
using UnityEngine;

namespace CombatRoom
{
    public class Target : MonoBehaviour
    {
        public event Action<Target> OnTargetHit;

        [SerializeField] private float moveRadius = 1f;
        [SerializeField] private float moveSpeed = 1f;
        [SerializeField] private Vector3 initialScale = Vector3.one;
        [SerializeField] private Vector3 targetScale = Vector3.one * 1.5f;

        private Vector3 startPos;
        private Vector3 randomTargetPos;
        private float scaleLerp;

        private void Awake()
        {
            startPos = transform.position;
            PickNewTargetPosition();
        }

        private void Update()
        {
            MoveAndScale();
        }

        private void MoveAndScale()
        {
            transform.position = Vector3.Lerp(transform.position, randomTargetPos, moveSpeed * Time.deltaTime);
            scaleLerp += Time.deltaTime * moveSpeed;
            transform.localScale = Vector3.Lerp(initialScale, targetScale, Mathf.PingPong(scaleLerp, 1f));

            if (Vector3.Distance(transform.position, randomTargetPos) < 0.1f)
            {
                PickNewTargetPosition();
            }
        }

        private void PickNewTargetPosition()
        {
            randomTargetPos = startPos + UnityEngine.Random.insideUnitSphere * moveRadius;
            randomTargetPos.y = startPos.y;
        }

        private void Hit()
        {
            OnTargetHit?.Invoke(this);
            Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            Hit();
        }
    }
}