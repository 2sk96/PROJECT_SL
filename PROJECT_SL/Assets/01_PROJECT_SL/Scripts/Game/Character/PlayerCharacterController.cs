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

        private List<IInteractable> prevInteractables = new List<IInteractable>();

        public MainHUDUI mainHUDUI;
        public InteractionUI interactionUI;

        public float interactionRadius = 2f;
        public LayerMask interactionLayer;
        public List<IInteractable> interactables = new List<IInteractable>();


        private void Awake()
        {
            linkedCharacter = GetComponent<CharacterBase>();

            mainHUDUI = UIManager.Singleton.GetUI<MainHUDUI>(UIList.MainHUDUI);
            interactionUI = UIManager.Show<InteractionUI>(UIList.InteractionUI);
        }

        private void Start()
        {
            InputSystem.Singleton.OnClickedAlpha1 += OnClickedAlpha1;
            InputSystem.Singleton.OnClickedAlpha2 += OnClickedAlpha2;
            InputSystem.Singleton.OnClickedCrouch += OnClickedCrouch;
            InputSystem.Singleton.OnClickedReload += OnClickedReload;
            InputSystem.Singleton.OnClickedInteraction += OnClickedInteraction;
        }


        private void OnDestroy()
        {
            InputSystem.Singleton.OnClickedAlpha1 -= OnClickedAlpha1;
            InputSystem.Singleton.OnClickedAlpha2 += OnClickedAlpha2;
            InputSystem.Singleton.OnClickedCrouch -= OnClickedCrouch;
            InputSystem.Singleton.OnClickedReload -= OnClickedReload;
            InputSystem.Singleton.OnClickedInteraction -= OnClickedInteraction;
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
            mainHUDUI.UpdateDisplayMag(linkedCharacter.currentWeaponBase.currentMagazine, linkedCharacter.currentWeaponBase.magazineSize);
        }

        // Interaction 관련 업데이트
        private void FixedUpdate()
        {
            if (!linkedCharacter.IsAlive) return;

            ShowCrosshair(InputSystem.Singleton.IsRightMouseButton);
            ShowInteractionUI();
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

        


        private void ShowInteractionUI()
        {
            // 무장중이 아닐때는 interactables 관련 기능 실행되어야 함
            if (!linkedCharacter.IsArmed)
            {
                // 기존 Interactables 리스트를 prevInteractables 로 저장 후 interactables 초기화
                prevInteractables = new List<IInteractable>(interactables);
                interactables.Clear();
                // layerMask 이내 주변 Collider 추출
                Collider[] overlapped = Physics.OverlapSphere(transform.position, interactionRadius, interactionLayer);
                // overlapped 배열 돌면서 IInteractable 을 상속받은 오브젝트일 경우 interactables 에 추가
                for (int i = 0; i < overlapped.Length; i++)
                {
                    if (overlapped[i].TryGetComponent(out IInteractable interactable))
                    {
                        interactables.Add(interactable);
                    }
                }
                // TODO : prevInteractables 와 새로 갱신 된 interactables 차이가 있다면? => InteractionUI 에 갱신해준다
                // 새로운 interactables 를 돌면서 prevInteractables에 없다면 해당 InteractionContent 추가
                for (int i = 0; i < interactables.Count; i++)
                {
                    if (!prevInteractables.Contains(interactables[i]))
                    {
                        interactionUI.AddInteractionContent(interactables[i]);
                    }
                }
                // 기존 prevInteractables 를 돌면서 갱신된 interactables 에 없다면 해당 InteractionContent 제거
                for (int i = 0; i < prevInteractables.Count; i++)
                {
                    if (!interactables.Contains(prevInteractables[i]))
                    {
                        interactionUI.RemoveInteractionContent(prevInteractables[i]);
                    }
                }
            }
            // 무장중일 때는 prevInteractables, interactables, InteractionUI 의 createdContents 초기화 진행
            else
            {
                if (interactionUI.createdContents.Count > 0)
                {
                    interactionUI.RemoveAllInteractionContent();
                    prevInteractables.Clear();
                    interactables.Clear();
                }
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

        private void OnClickedInteraction()
        {
            if (interactables.Count > 0)
            {
                interactionUI.ExecuteInteract();
            }
        }
    }
}
