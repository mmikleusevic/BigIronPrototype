using System.Threading;
using Cysharp.Threading.Tasks;
using Managers;
using Player;
using UnityEngine;
using Weapons;

namespace CombatRoom
{
    public abstract class Combatant : MonoBehaviour
    {
        [field: SerializeField] public Gun Gun { get; private set; }
        [field: SerializeField] public CombatantDataSO Data { get; protected set; }
        
        [SerializeField] private float rotationSpeed = 5f;
        [SerializeField] protected Animator combatantAnimator;
        [SerializeField] private Renderer combatantRenderer;
        
        public abstract Health Health { get; }
        public abstract Gold Gold { get; }
        public Animator CombatantAnimator => combatantAnimator;
        public bool IsDead => Health && Health.CurrentHealth <= 0;
        
        public void GainGoldAmount(int amount)
        {
            Gold.GainGoldAmount(amount);
        }

        public int LoseGoldAmount(int amount)
        {
            return Gold.LoseGoldAmount(amount);
        }

        public int TakeDamage(Combatant damager, Combatant receiver, int damage)
        {
            return Health.TakeDamage(damager, receiver, damage);
        }

        public int Heal(int healAmount)
        {
            return Health.Heal(healAmount);
        }
        
        public void ToggleVisibility(bool visible)
        {
            if (combatantRenderer) combatantRenderer.enabled = visible;
            if (Gun) Gun.gameObject.SetActive(visible);
        }
        
        public async UniTask RotateTowardsTargetAndFire(Combatant target, int damage, CancellationToken cancellationToken)
        {
            if (!target) return;
            
            cancellationToken.ThrowIfCancellationRequested();

            Quaternion originalRotation = transform.rotation;
            
            Vector3 direction = (target.transform.position - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            await RotateTo(targetRotation, rotationSpeed, cancellationToken);
            
            cancellationToken.ThrowIfCancellationRequested();

            if (combatantAnimator)
            {
                combatantAnimator.Play(GameStrings.AIM);
                combatantAnimator.SetLayerWeight(1, 1f);
            }
            
            await FireProjectileAtTarget(target, damage, cancellationToken);
            
            cancellationToken.ThrowIfCancellationRequested();
            
            await RotateTo(originalRotation, rotationSpeed, cancellationToken);

            if (combatantAnimator)
            {
                combatantAnimator.SetLayerWeight(1, 0f);
                combatantAnimator.Play(GameStrings.IDLE);
            }
        }

        private async UniTask RotateTo(Quaternion targetRotation, float speed, CancellationToken cancellationToken)
        {
            while (this && Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
            {
                cancellationToken.ThrowIfCancellationRequested();
                
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * speed);
                await UniTask.Yield(cancellationToken);
            }

            transform.rotation = targetRotation;
        }

        private async UniTask FireProjectileAtTarget(Combatant target, int damage, CancellationToken cancellationToken)
        {
            if (!target) return;
            if (!Gun) return;
            
            Gun.OnShootEnemy(target.transform.position + Vector3.up);
            Projectile projectile = Instantiate(Gun.ProjectilePrefab, Gun.ShootPoint.position, Gun.ShootPoint.rotation);
            projectile.Initialize(target, this, damage);
            
            await projectile.WaitForHitAsync(cancellationToken);
        }
    }
}