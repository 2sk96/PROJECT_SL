using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectSL
{
    public enum WeaponType
    {
        Rifle=1,
        Pistol=2,
    }
    
    public class PlayerCharacterController : MonoBehaviour
    {
        private CharacterBase linkedCharacter;

        private float bottomClamp = - 90.0f;
        private float topClamp = 90.0f;
        private float cameraSensitivity = 2.0f;
        private float threshold = 0.01f;
        private float targetYaw;
        private float targetPitch;

        public MainHUDUI mainHUDUI;

        private void Awake()
        {
            linkedCharacter = GetComponent<CharacterBase>();

            mainHUDUI = UIManager.Singleton.GetUI<MainHUDUI>(UIList.MainHUDUI);
        }

        private void Start()
        {
            InputSystem.Singleton.OnClickedAlpha1 += OnClickedAlpha1;
            InputSystem.Singleton.OnClickedAlpha2 += OnClickedAlpha2;
            InputSystem.Singleton.OnClickedCrouch += OnClickedCrouch;
            InputSystem.Singleton.OnClickedReload += OnClickedReload;
        }


        private void OnDestroy()
        {
            InputSystem.Singleton.OnClickedAlpha1 -= OnClickedAlpha1;
            InputSystem.Singleton.OnClickedAlpha2 += OnClickedAlpha2;
            InputSystem.Singleton.OnClickedCrouch -= OnClickedCrouch;
            InputSystem.Singleton.OnClickedReload -= OnClickedReload;
        }

        private void Update()
        {
            if (!linkedCharacter.IsAlive) return;
            
            linkedCharacter.IsRun = InputSystem.Singleton.IsLeftShift;
            linkedCharacter.Move(InputSystem.Singleton.Movement, Camera.main.transform.eulerAngles.y);
            linkedCharacter.Rotate(CameraSystem.Instance.CameraAimingPoint);

            if (linkedCharacter.IsArmed)
            {
                linkedCharacter.IsAiming = InputSystem.Singleton.IsRightMouseButton;
            }

            if (InputSystem.Singleton.IsLeftMouseButton)
            {
                if (linkedCharacter.IsArmed)
                {
                    linkedCharacter.Shoot();
                }
            }

            mainHUDUI.UpdateHealthBar(linkedCharacter.currentHealth, linkedCharacter.maxHealth);
            mainHUDUI.UpdateStaminaBar(linkedCharacter.currentStamina, linkedCharacter.maxStamina);


            //switch (linkedCharacter.currentWeaponType)
            //{
            //    case (int)WeaponType.Rifle:
            //        mainHUDUI.UpdateDisplayMag(linkedCharacter.linkedRifle.currentMagazine, linkedCharacter.linkedRifle.magazineSize);
            //        break;
            //    case (int)WeaponType.Pistol:
            //        mainHUDUI.UpdateDisplayMag(linkedCharacter.linkedPistol.currentMagazine, linkedCharacter.linkedPistol.magazineSize);
            //        break;
            //}

            mainHUDUI.UpdateDisplayMag(linkedCharacter.linkedRifle.currentMagazine, linkedCharacter.linkedRifle.magazineSize);
        }

        // Interaction 관련 업데이트
        private void FixedUpdate()
        {
            if (!linkedCharacter.IsAlive) return;

            ShowCrosshair(InputSystem.Singleton.IsRightMouseButton);
        }

        private void LateUpdate()
        {
            if (!linkedCharacter.IsAlive) return;
            CameraRotation();
        }

        private void ShowCrosshair(bool isRightMouseButtonClicked)
        {
            if (!linkedCharacter.IsArmed)
            {
                UIManager.Hide<CrosshairUI>(UIList.CrosshairUI);
                return;
            }
            
            if (isRightMouseButtonClicked)
            {
                UIManager.Show<CrosshairUI>(UIList.CrosshairUI);
            }
            else
            {
                UIManager.Hide<CrosshairUI>(UIList.CrosshairUI);
            }
        }
        
        // 마우스 움직임에 따른 카메라 회전을 위한 함수
        private void CameraRotation()
        {
            if (InputSystem.Singleton.Look.sqrMagnitude >= threshold)
            {
                float yaw = InputSystem.Singleton.Look.x * cameraSensitivity;
                float pitch = InputSystem.Singleton.Look.y * cameraSensitivity;

                targetYaw += yaw;
                targetPitch -= pitch;
            }

            targetYaw = ClampAngle(targetYaw, float.MinValue, float.MaxValue);
            targetPitch = ClampAngle(targetPitch, bottomClamp, topClamp);

            linkedCharacter.CameraPivot.rotation = Quaternion.Euler(targetPitch, targetYaw, 0f);
        }

        // 각도가 0~360도 이내로만 존재할 수 있도록 보정해 주는 함수
        private static float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360f) angle += 360f;
            if (angle > 360f) angle -= 360f;
            return Mathf.Clamp(angle, min, max);
        }

        private void OnClickedAlpha1()
        {
            linkedCharacter.SetWeaponEquipState((int)WeaponType.Rifle);
        }

        private void OnClickedAlpha2()
        {
            linkedCharacter.SetWeaponEquipState((int)WeaponType.Pistol);
        }
        
        private void OnClickedCrouch()
        {
            linkedCharacter.IsCrouch = !linkedCharacter.IsCrouch;
        }
        private void OnClickedReload()
        {
            linkedCharacter.Reload();
        }

    }
}
