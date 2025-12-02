using UI;
using UnityEngine;

namespace Weapons
{
    public class GunUIController : MonoBehaviour
    {
        [SerializeField] private GunUI gunUI;

        private Gun currentGun;
        
        public void SetGun(Gun newGun)
        {
            currentGun = newGun;

            currentGun.OnAmmoChanged += HandleAmmoChanged;
            currentGun.OnWeaponNameChanged += HandleWeaponNameChanged;
            
            HandleWeaponNameChanged(currentGun.name);
            HandleAmmoChanged(currentGun.CurrentAmmo, currentGun.MaxAmmo);
        }
        
        private void OnDisable()
        {
            if (!currentGun) return;
            
            currentGun.OnAmmoChanged -= HandleAmmoChanged;
            currentGun.OnWeaponNameChanged -= HandleWeaponNameChanged;
        }

        private void HandleAmmoChanged(int current, int max)
        {
            gunUI.SetAmmo(current, max);
        }

        private void HandleWeaponNameChanged(string gunName)
        {
            gunUI.SetGunName(gunName);
        }
    }
}