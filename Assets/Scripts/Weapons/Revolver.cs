using System.Collections;
using UnityEngine;

namespace Weapons
{
    public class Revolver : Gun
    {
        public override void Shoot()
        {
            if (!CanShoot) return;

            base.Shoot();
            // spawn bullet, animations, etc.
        }

        public override void Reload()
        {
            if (!CanReload) return;
            
            StartCoroutine(ReloadCoroutine());
        }

        private IEnumerator ReloadCoroutine()
        {
            isReloading = true;

            yield return new WaitForSeconds(reloadTime);

            currentAmmo = maxAmmo;
            isReloading = false;

            InvokeAmmoChanged();
        }
    }
}