using System;
using System.Collections;
using CombatRoom;
using Managers;
using Targets;
using UnityEngine;
using UnityEngine.VFX;

namespace Weapons
{
    public abstract class Gun : Weapon
    {
        public event Action OnReloadStarted;
        public event Action OnReloadFinished;
        public event Action<int, int> OnAmmoChanged;

        [SerializeField] protected Animator playerAnimator;
        [SerializeField] private BulletTracer tracerPrefab;
        [SerializeField] private AudioClip clickingSound;
        [SerializeField] protected AudioClip reloadSound;
        [SerializeField] private AudioClip shotSound;
        [SerializeField] protected Transform shootPoint;
        [SerializeField] protected ParticleSystem shootVFXPrefab;
        [SerializeField] private LayerMask hitMask;
        [SerializeField] protected float reloadTime = 1f;
        [SerializeField] protected float fireRate = 0.5f;
        [SerializeField] protected int maxAmmo;
        [SerializeField] private float raycastDistance = 30f;
        
        public int CurrentAmmo  => currentAmmo;
        public int MaxAmmo => maxAmmo;
        
        public Transform ShootPoint => shootPoint;
        protected virtual bool CanShoot => !isReloading && Time.time >= nextShootTime && currentAmmo > 0;
        protected virtual bool CanReload => !isReloading && currentAmmo < maxAmmo;
        
        protected int currentAmmo;
        private float nextShootTime;
        protected bool isReloading;
        private bool alternateShot; 

        protected override void Awake()
        {
            base.Awake();
            
            currentAmmo = maxAmmo;
            
            OnAmmoChanged?.Invoke(currentAmmo, maxAmmo);
        }

        protected void RaiseReloadStarted()
        {
            OnReloadStarted?.Invoke();
        }

        protected void RaiseReloadFinished()
        {
            OnReloadFinished?.Invoke();
        }
        
        public override void Use(Vector3 rayOrigin, Vector3 rayDirection) => Shoot(rayOrigin, rayDirection);

        public virtual void Shoot(Vector3 rayOrigin, Vector3 rayDirection)
        {
            if (currentAmmo == 0 && Time.time >= nextShootTime)
            {
                SoundManager.Instance.PlayVFX(clickingSound);
                nextShootTime = Time.time + fireRate;
            }
            
            if (!CanShoot) return;

            nextShootTime = Time.time + fireRate;
            currentAmmo--;
            
            SoundManager.Instance.PlayVFX(shotSound);
            
            SpawnShotVFX(rayDirection);
            HitScan(rayOrigin, rayDirection);
            
            OnAmmoChanged?.Invoke(currentAmmo, maxAmmo);
        }
        
        private void SpawnShotVFX(Vector3 direction)
        {
            if (shootVFXPrefab && shootPoint)
            {
                ParticleSystem vfx = Instantiate(shootVFXPrefab, shootPoint.position, Quaternion.identity);
                vfx.transform.forward = direction;
                vfx.Play();
            }
        }

        private void HitScan(Vector3 rayOrigin, Vector3 rayDirection)
        {
            Vector3 endPoint = rayOrigin + rayDirection * raycastDistance;
            
            if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hit, raycastDistance, hitMask))
            {
                if (hit.collider.TryGetComponent(out BaseTarget baseTarget))
                {
                    baseTarget.Hit();
                }
            }
            
            SpawnTracer(rayOrigin, endPoint);
        }
        
        private void SpawnTracer(Vector3 start, Vector3 end)
        {
            if (!tracerPrefab || !shootPoint) return;

            BulletTracer tracer = Instantiate(tracerPrefab);
            tracer.Initialize(start, end);
        }
        
        protected void InvokeAmmoChanged() => OnAmmoChanged?.Invoke(currentAmmo, maxAmmo);

        public abstract void Reload();
    }
}