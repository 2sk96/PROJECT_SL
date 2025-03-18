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
        public float baseDamage;
        public float accuracy;

        private float lastFireTime = 0f;
        
        private CharacterBase linkedCharacter;

        public string itemID;
        public string itemName;

        private void Awake()
        {
            linkedCharacter = GetComponentInParent<CharacterBase>();

            if (GameDataModel.Singleton.GetItemData(itemID, out ItemDataDTO.ItemData itemData))
            {
                itemName = itemData.ItemName;
            }

            if (GameDataModel.Singleton.GetWeaponItemData(itemID, out WeaponDataDTO.WeaponData weaponData))
            {
                fireRate = weaponData.FireRate;
                magazineSize = weaponData.MagazineSize;
                baseDamage = weaponData.BaseDamage;
                accuracy = weaponData.Accuracy;
            }

            currentMagazine = magazineSize;
        }

        public void Shoot()
        {
            if (currentMagazine <= 0) return;
            if (!firePoint) firePoint = this.gameObject.transform;

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
