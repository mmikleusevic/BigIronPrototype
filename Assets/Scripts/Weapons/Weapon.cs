using System;
using UnityEngine;

namespace Weapons
{
    public abstract class Weapon : MonoBehaviour
    {
        public event Action<string> OnWeaponNameChanged;

        [SerializeField] protected string weaponName;
        public string WeaponName => weaponName;

        protected virtual void Awake()
        {
            OnWeaponNameChanged?.Invoke(weaponName);
        }

        public abstract void Use();
    }
}