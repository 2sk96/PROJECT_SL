using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectSL
{
    public class WeaponPistol : MonoBehaviour
    {
        //public Transform firePoint;
        //public GameObject bulletPrefab;
        //public float fireRate = 0.5f;
        //public float magazineSize = 8f;

        //private WeaponBase weaponBase;

        //private void Awake()
        //{
        //    weaponBase = GetComponent<WeaponBase>();
        //    weaponBase.currentMagazine = magazineSize;
        //}
        public Transform firePoint;
        public GameObject bulletPrefab;
        public float fireRate = 0.5f;
        public float magazineSize = 7f;
        public float currentMagazine;

        private float lastFireTime = 0f;

        private CharacterBase linkedCharacter;

        private void Awake()
        {
            linkedCharacter = GetComponentInParent<CharacterBase>();
            firePoint = linkedCharacter.pistolFirePoint;
            currentMagazine = magazineSize;
        }

        public void Shoot()
        {
            if (currentMagazine <= 0) return;

            if (Time.time - lastFireTime > fireRate)
            {
                GameObject newBullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                newBullet.gameObject.SetActive(true);
                Rigidbody bulletRigidbody = newBullet.GetComponent<Rigidbody>();
                bulletRigidbody.AddForce(firePoint.forward * 10f, ForceMode.Impulse);
                lastFireTime = Time.time;
                currentMagazine -= 1;

                Destroy(newBullet, 5f);
            }
        }

        public void Reload()
        {
            // 임시
            currentMagazine = magazineSize;
        }
    }
}
