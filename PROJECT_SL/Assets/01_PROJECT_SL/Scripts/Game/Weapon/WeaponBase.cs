using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectSL
{
    public class WeaponBase : MonoBehaviour
    {
        public Transform firePoint;
        public GameObject bulletPrefab;
        public float fireRate;
        public float magazineSize;
        public float currentMagazine;

        private float lastFireTime = 0f;
        
        private CharacterBase linkedCharacter;
        [SerializeField] private int weaponType;

        private void Awake()
        {
            linkedCharacter = GetComponentInParent<CharacterBase>();
            if (weaponType == (int)WeaponType.Rifle)
            {
                firePoint = linkedCharacter.rifleFirePoint;
            }
            else if(weaponType == (int)WeaponType.Pistol)
            {
                firePoint = linkedCharacter.pistolFirePoint;
            }
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

                var muzzle = EffectManager.Instance.GetMuzzleEffect("Muzzle_01");
                muzzle.transform.SetPositionAndRotation(firePoint.position, firePoint.rotation);
            }
        }

        public void Reload()
        {
            currentMagazine = magazineSize;
        }
    }
}
