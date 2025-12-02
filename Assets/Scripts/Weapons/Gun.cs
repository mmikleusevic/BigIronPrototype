using System;
using System.Collections;
using UnityEngine;

namespace Weapons
{
    public abstract class Gun : Weapon
    {
        public event Action<int, int> OnAmmoChanged;
        
        [field: SerializeField] public int Damage { get; private set; } 
        [SerializeField] protected Bullet bulletPrefab;
        [SerializeField] protected Transform shootPoint;
        [SerializeField] protected float reloadTime = 1f;
        [SerializeField] protected float fireRate = 0.5f;
        [SerializeField] protected int maxAmmo;
        
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
        
        public override void Use() => Shoot();

        public virtual void Shoot()
        {
            if (!CanShoot) return;

            nextShootTime = Time.time + fireRate;

            currentAmmo--;
            
            SpawnBullet();
            
            OnAmmoChanged?.Invoke(currentAmmo, maxAmmo);
        }
        
        private void SpawnBullet()
        {
            Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
        }
        
        protected void InvokeAmmoChanged() => OnAmmoChanged?.Invoke(currentAmmo, maxAmmo);

        public abstract void Reload();
    }
}