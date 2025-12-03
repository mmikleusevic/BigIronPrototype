using System.Collections;
using UnityEngine;

namespace Weapons
{
    public class Revolver : Gun
    {
        public override void Shoot(Vector3 rayOrigin, Vector3 rayDirection)
        {
            if (!CanShoot) return;

            base.Shoot(rayOrigin, rayDirection);
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