using Cinemachine;
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
        public float recoil;
        public float bulletSpeed = 20f;

        public CrosshairUI crosshairUI;

        private float lastFireTime = 0f;
        
        private CharacterBase linkedCharacter;

        [SerializeField] private float adjustedAccuracy;
        [SerializeField] private float spreadAmount;
        [SerializeField] private CinemachineImpulseSource impulseSource;

        public string itemID;
        public string itemName;

        private void Awake()
        {
            linkedCharacter = GetComponentInParent<CharacterBase>();
            impulseSource = GetComponent<CinemachineImpulseSource>();

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
                recoil = weaponData.Recoil;
            }

            currentMagazine = magazineSize;
            if (linkedCharacter.isPlayer)
            {
                crosshairUI = UIManager.Singleton.GetUI<CrosshairUI>(UIList.CrosshairUI);
                // recoil = 10 기준 x=0.05 z=0.05, 10단위로 커질때마다 
                float defaultVelocity = recoil / 10 * 0.05f;
                impulseSource.m_DefaultVelocity = new Vector3(0, defaultVelocity, defaultVelocity);
            }
        }

        private void Update()
        {
            if (linkedCharacter != null)
            {
                float accuracyFactor = 0;
                // 
                
                if (linkedCharacter.IsCrouch)
                {
                    accuracyFactor += 0.4f;
                }
                if (linkedCharacter.TargetSpeed > 0)
                {
                    accuracyFactor -= 0.2f;
                }
                if (!linkedCharacter.IsGrounded)
                {
                    accuracyFactor -= 0.2f;
                }

                adjustedAccuracy = accuracy + (100 - accuracy) * accuracyFactor;
                spreadAmount = GameDataModel.Singleton.GetWeaponAccuracy(adjustedAccuracy);
            }
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

                // bulletForceDirection 에서 최대 spreadAmount 만큼 right/up 방향으로 랜덤 추가
                Vector3 bulletForceDirection = firePoint.forward + firePoint.right * Random.Range(-1f, 1f) * spreadAmount + firePoint.up * Random.Range(-1f, 1f) * spreadAmount;
                bulletForceDirection = bulletForceDirection.normalized;
                bulletRigidbody.AddForce(bulletForceDirection * bulletSpeed, ForceMode.Impulse);

                if (linkedCharacter.isPlayer && crosshairUI != null)
                {
                    crosshairUI.SpreadCrosshair(adjustedAccuracy);
                }

                lastFireTime = Time.time;
                currentMagazine -= 1;

                Destroy(newBullet, 5f);

                var muzzle = EffectManager.Instance.GetMuzzleEffect("Muzzle_01");
                muzzle.transform.SetPositionAndRotation(firePoint.position, firePoint.rotation);

                // 플레이어일때만 GenerateImpulse 및 CameraRecoil 이 실행되어야 한다. NPC 일 경우 GenerateImpulse가 발생하면 플레이어 카메라에 영향을 주기 때문에 실행되면 안된다
                if (linkedCharacter.isPlayer)
                {
                    
                    impulseSource.GenerateImpulse();
                    // 여기는 총기 Recoil 에 따라 알맞는 값 적용
                    // recoil = 10 기준 recoilAmount = 1f recoverySpeed = 2f
                    float recoilAmount = recoil / 10;
                    float verticalRecoil = 2f;
                    float horizontalRecoil = 1f;
                    PlayerCharacterController.Instance.CameraRecoil(recoilAmount, verticalRecoil, horizontalRecoil);

                }
            }
        }

        public void Reload()
        {
            currentMagazine = magazineSize;
        }
    }
}
