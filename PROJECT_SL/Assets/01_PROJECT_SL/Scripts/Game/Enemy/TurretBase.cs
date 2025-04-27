using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectSL
{
    public class TurretBase : MonoBehaviour
    {
        public WeaponBase weapon1;
        public WeaponBase weapon2;

        [SerializeField] private bool shootingAllowed = false;
        [SerializeField] private bool isReloading = false;

        private void Update()
        {
            if (shootingAllowed)
            {
                Shoot();
            }
        }

        private void Shoot()
        {
            if (weapon1.currentMagazine > 0)
            {
                weapon1.Shoot();
            }
            else
            {
                if (!isReloading)
                {
                    StartCoroutine(WaitAndReload(weapon1));
                }
            }

            if (weapon2.currentMagazine > 0)
            {
                weapon2.Shoot();
            }
            else
            {
                if (!isReloading)
                {
                    StartCoroutine(WaitAndReload(weapon2));
                }
            }
        }
        
        private IEnumerator WaitAndReload(WeaponBase weaponBase)
        {
            isReloading = true;
            yield return new WaitForSeconds(1f);

            weaponBase.Reload();

            isReloading = false;
        }
        
    }
}
