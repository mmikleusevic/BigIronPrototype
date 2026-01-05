using System;
using System.Threading;
using CombatRoom;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Weapons
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float speed = 10f;
        [SerializeField] private float hitDistance = 0.2f;
        
        private Combatant target;
        private Combatant owner;
        private UniTaskCompletionSource completion;
        
        private int damage;

        private void Awake()
        {
            completion = new UniTaskCompletionSource();
        }
        
        public void Initialize(Combatant target, Combatant owner, int damage)
        {
            this.target = target;
            this.owner = owner;
            this.damage = damage;

            if (target) transform.position = target.transform.position + Vector3.up;
        }

        public UniTask WaitForHitAsync(CancellationToken token)
        {
            return completion.Task.AttachExternalCancellation(token);
        }

        private void OnTriggerEnter(Collider other)
        {
            target.TakeDamage(owner, target, damage);
            Complete();
        }

        private void OnDestroy()
        {
            Complete();
        }

        private void Complete()
        {
            if (completion.Task.Status.IsCompleted()) return;

            completion.TrySetResult();
            Destroy(gameObject);
        }
    }
}