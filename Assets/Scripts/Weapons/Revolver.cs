using System.Collections;
using Managers;
using UnityEngine;

namespace Weapons
{
    public class Revolver : Gun
    {
        [SerializeField] private AudioClip revolverCockSound;
        
        public override void Shoot(Vector3 rayOrigin, Vector3 rayDirection)
        {
            if (CanShoot) SoundManager.Instance.PlayVFX(revolverCockSound);

            base.Shoot(rayOrigin, rayDirection);
        }

        public override void Reload()
        {
            if (!CanReload) return;
            
            StartCoroutine(ReloadCoroutine());
        }

        private IEnumerator ReloadCoroutine()
        {
            playerAnimator.SetLayerWeight(1, 1f);
            playerAnimator.Play(GameStrings.RELOAD);
            
            isReloading = true;
            
            RaiseReloadStarted();
            
            yield return new WaitForSeconds(reloadTime);
                
            RaiseReloadFinished();
            
            currentAmmo = maxAmmo;
            isReloading = false;
            
            InvokeAmmoChanged();
        }
    }
}