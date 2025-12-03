using System;
using System.Collections;
using CombatRoom;
using UnityEngine;

namespace Weapons
{
    public abstract class Gun : Weapon
    {
        public event Action<int, int> OnAmmoChanged;
        
        [field: SerializeField] public int Damage { get; private set; } 
        [SerializeField] protected Transform shootPoint;
        [SerializeField] protected float reloadTime = 1f;
        [SerializeField] protected float fireRate = 0.5f;
        [SerializeField] protected int maxAmmo;
        [SerializeField] private float raycastDistance = 30f;
        [SerializeField] private LayerMask hitMask;
        
        public int CurrentAmmo  => currentAmmo;
        public int MaxAmmo => maxAmmo;
        
        protected virtual bool CanShoot => !isReloading && Time.time >= nextShootTime && currentAmmo > 0;
        protected virtual bool CanReload => !isReloading && currentAmmo < maxAmmo;
        
        protected int currentAmmo;
        private float nextShootTime;
        protected bool isReloading;

        protected override void Awake()
        {
            base.Awake();
            
            currentAmmo = maxAmmo;
            
            OnAmmoChanged?.Invoke(currentAmmo, maxAmmo);
        }
        
        public override void Use(Vector3 rayOrigin, Vector3 rayDirection) => Shoot(rayOrigin, rayDirection);

        public virtual void Shoot(Vector3 rayOrigin, Vector3 rayDirection)
        {
            if (!CanShoot) return;

            nextShootTime = Time.time + fireRate;

            currentAmmo--;

            HitScan(rayOrigin, rayDirection);
            
            OnAmmoChanged?.Invoke(currentAmmo, maxAmmo);
        }

        private void HitScan(Vector3 rayOrigin, Vector3 rayDirection)
        {
            RaycastHit hit;
            
            if (Physics.Raycast(rayOrigin, rayDirection, out hit, raycastDistance, hitMask))
            {
                if (hit.collider.TryGetComponent(out Target target))
                {
                    target.Hit();
                }
            }
        }
        
        protected void InvokeAmmoChanged() => OnAmmoChanged?.Invoke(currentAmmo, maxAmmo);

        public abstract void Reload();
    }
}