using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace ProjectSL
{
    public class CharacterBase : MonoBehaviour
    {
        public bool IsRun
        {
            get => isRun;
            set
            {
                if (IsCrouch || IsAiming)
                {
                    isRun = false;
                }
                else
                {
                    isRun = value;
                }
            }
        }
        [field: SerializeField] public bool IsCrouch { get; set; } = false;


        public bool IsArmed
        {
            get => isArmed;
            set
            {
                if (!IsAlive) return;

                isArmed = value;
                characterAnimator.SetBool("IsArmed", value);
            }
        }
        public bool IsAiming
        {
            get => isAiming;
            set
            {
                if (!isArmed) return;
                isAiming = value;
                characterAnimator.SetBool("IsAiming", value);
                //isAiming = true;
                //characterAnimator.SetBool("IsAiming", true);
            }
        }
        public float MoveSpeed => moveSpeed;
        public bool IsAlive => currentHealth > 0f;
        public Transform CameraPivot { get; private set; }

        // 총 관련
        public GameObject riflePrefab;
        public GameObject pistolPrefab;
        public Transform rifleFirePoint;
        public Transform pistolFirePoint;
        public GameObject weaponRifle;
        public GameObject weaponPistol;
        // 이건 필요 없을수도
        public WeaponBase currentWeaponBase;
        public WeaponBase rifleWeaponBase;
        public WeaponBase pistolWeaponBase;

        public int currentWeaponType = (int)WeaponType.Rifle;

        public Vector3 rifleEquipOffsetPos;
        public Vector3 rifleEquipOffsetRot;
        public Vector3 pistolEquipOffsetPos;
        public Vector3 pistolEquipOffsetRot;
        // 무기에 따라 equipOffset이 바뀌면 rifle/pistol 나눠야함
        public Vector3 rifleHolsterOffsetPos;
        public Vector3 rifleHolsterOffsetRot;
        public Vector3 pistolHolsterOffsetPos;
        public Vector3 pistolHolsterOffsetRot;

        public Rig aimingRig;
        public Rig leftHandRifleRig;
        public Rig leftHandPistolRig;

        public bool isReloading = false;
        public bool isChangingWeaponState = false;    // left hand IK 잡아주기 위한 bool parameter, true 일 경우 무기 변경/장착/해제 중인 상황

        public bool allowCharacterMovement = true;
        public bool allowCharacterAction = true;

        // 캐릭터 스탯 관련
        public float currentHealth;
        public float currentStamina;
        public float maxStamina = 100f;
        public float maxHealth = 100f;

        public float walkSpeed = 2.0f;
        public float runSpeed = 7.0f;

        public float jumpHeight = 2f;
        public float jumpStaminaCost = 20f;
        public bool jumpTrigger = false;

        public float rollStaminaCost = 20f;
        public bool rollTrigger = false;

        public float gravity = -9.81f;
        public float terminalVelocity = 50f;
        public float groundOffset;
        public float groundCheckRadius = 0.25f;
        public LayerMask groundLayer;

        [SerializeField] private float verticalVelocity;
        [SerializeField] private bool isGrounded;

        private Animator characterAnimator;
        private CharacterController characterController;

        private Vector2 movementInput;

        private float runStaminaCost = 10f;
        private float staminaRegen = 5f;

        [SerializeField] private float moveSpeed;       // 실제 캐릭터의 이동량에 영향을 주는 속도 값
        [SerializeField] private float targetSpeed;     // Animator의 parameter로 사용하기 위한 속도 값
        private float smoothTargetSpeed;                // Animator의 parameter로 사용하기 위한 값
        private float smoothHorizontal;                 // Animator의 parameter로 사용하기 위한 값
        private float smoothVertical;                   // Animator의 parameter로 사용하기 위한 값
        private float smoothCrouch;                     // Animator의 parameter로 사용하기 위한 값
        private float smoothArmed;
        private float smoothAiming;

        private float targetRotation;
        private float rotationVelocity;
        private float rotationSmoothTime = 0.1f;

        private Transform rightHandTransform;       // 무기를 장착했을 때 위치
        private Transform backTransform;            // 라이플을 장착하지 않았을 때 위치 (등)
        private Transform rightWaistTransform;      // 권총을 장착하지 않았을 때 위치 (오른쪽 허리춤)

        private bool isActiveAimingIK;
        private bool isActiveLeftHandIKRifle;
        private bool isActiveLeftHandIKPistol;

        private DropItem targetPickUpItem;

        [SerializeField] private bool isRun = false;
        [SerializeField] private bool isArmed = false;
        [SerializeField] private bool isAiming = false;

        private void Awake()
        {
            characterAnimator = GetComponent<Animator>();
            characterController = GetComponent<CharacterController>();

            CameraPivot = transform.Find("CameraPivot");

            rightHandTransform = characterAnimator.GetBoneTransform(HumanBodyBones.RightHand);
            backTransform = characterAnimator.GetBoneTransform(HumanBodyBones.Spine);
            rightWaistTransform = characterAnimator.GetBoneTransform(HumanBodyBones.RightUpperLeg);

            weaponRifle = Instantiate(riflePrefab, backTransform);
            weaponPistol = Instantiate(pistolPrefab, rightWaistTransform);
            weaponRifle.transform.SetLocalPositionAndRotation(rifleHolsterOffsetPos, Quaternion.Euler(rifleHolsterOffsetRot));
            weaponPistol.transform.SetLocalPositionAndRotation(pistolHolsterOffsetPos, Quaternion.Euler(pistolHolsterOffsetRot));

            rifleWeaponBase = weaponRifle.GetComponent<WeaponBase>();
            pistolWeaponBase = weaponPistol.GetComponent<WeaponBase>();
            currentWeaponBase = rifleWeaponBase;
        }

        private void Start()
        {
            currentHealth = maxHealth;
            currentStamina = maxStamina;
        }

        private void Update()
        {
            UpdateStamina();
            CheckGround();
            ApplyGravity();

            smoothTargetSpeed = Mathf.Lerp(smoothTargetSpeed, targetSpeed, Time.deltaTime * 10f);
            smoothHorizontal = Mathf.Lerp(smoothHorizontal, movementInput.x, Time.deltaTime * 10f);
            smoothVertical = Mathf.Lerp(smoothVertical, movementInput.y, Time.deltaTime * 10f);
            smoothCrouch = Mathf.Lerp(smoothCrouch, IsCrouch ? 1.0f : 0f, Time.deltaTime * 10f);
            smoothArmed = Mathf.Lerp(smoothArmed, isArmed ? 1.0f : 0f, Time.deltaTime * 10f);
            smoothAiming = Mathf.Lerp(smoothAiming, isAiming ? 1.0f : 0f, Time.deltaTime * 10f);

            characterAnimator.SetFloat("Speed", targetSpeed);
            characterAnimator.SetFloat("Smooth Speed", smoothTargetSpeed);
            characterAnimator.SetFloat("Horizontal", smoothHorizontal);
            characterAnimator.SetFloat("Vertical", smoothVertical);
            characterAnimator.SetFloat("Crouch", smoothCrouch);
            characterAnimator.SetFloat("Armed", smoothArmed);
            characterAnimator.SetFloat("Aiming", smoothAiming);

            CheckActiveIK_Aiming();
            CheckActiveIK_LeftHand();

            aimingRig.weight = Mathf.Lerp(aimingRig.weight, isActiveAimingIK ? 1f : 0f, Time.deltaTime * 10f);
            leftHandRifleRig.weight = Mathf.Lerp(leftHandRifleRig.weight, isActiveLeftHandIKRifle ? 1f : 0f, Time.deltaTime * 10f);
            leftHandPistolRig.weight = Mathf.Lerp(leftHandPistolRig.weight, isActiveLeftHandIKPistol ? 1f : 0f, Time.deltaTime * 10f);
        }

        private void OnAnimatorMove()
        {
            
        }

        // isAiming일때
        // 살아있을 때
        // 무기 변경/장착/해제 모션을 하고 있지 않을 때
        private void CheckActiveIK_Aiming()
        {
            isActiveAimingIK = isAiming && IsAlive && !isChangingWeaponState;
        }

        // 라이플을 장착중일 때 (currentWeaponType 체크, isArmed 체크)
        // 재장전을 하고 있지 않을 때 (isReloading 체크)
        // 무기 변경/장착/해제 모션을 하고 있지 않을 때
        private void CheckActiveIK_LeftHand()
        {
            isActiveLeftHandIKRifle = currentWeaponType == (int)WeaponType.Rifle && isArmed && IsAlive && !isReloading && !isChangingWeaponState;
            isActiveLeftHandIKPistol = currentWeaponType == (int)WeaponType.Pistol && isArmed && IsAlive && !isReloading && !isChangingWeaponState;
        }

        private void UpdateStamina()
        {
            if (IsRun && isGrounded)
            {
                currentStamina -= runStaminaCost * Time.deltaTime;
            }
            else
            {
                currentStamina += staminaRegen * Time.deltaTime;
            }

            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        }

        public void Move(Vector2 input, float yAxisAngle)
        {
            if (!allowCharacterMovement) return;

            movementInput = input;
            bool isInputSomething = input.sqrMagnitude > 0;

            if (currentStamina > 0)
            {
                moveSpeed = Mathf.Lerp(moveSpeed, (isRun ? runSpeed : walkSpeed), Time.deltaTime * 10f);
                targetSpeed = isInputSomething ? (isRun ? 3.0f : 1.0f) : 0f;
            }
            else
            {
                moveSpeed = walkSpeed;
                targetSpeed = isInputSomething ? 1.0f : 0.0f;
            }

            if (isInputSomething && !isAiming)
            {
                Vector3 inputDirection = new Vector3(input.x, 0f, input.y).normalized;
                targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + yAxisAngle;
                // SmoothDampAngle: 현재 각도 (transform.eulerAngles.y) 에서 원하는 각도 (targetRotation) 까지 천천히 각도를 변경해준다. 부드러운 회전 구현
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity, rotationSmoothTime);
                transform.rotation = Quaternion.Euler(0f, rotation, 0f);
            }

            Vector3 movement = Vector3.zero;
            float movementAllowed = characterAnimator.GetFloat("MovementAllowed");

            if (movementAllowed > 0.95)
            {
                if (isAiming) // 무장상태 에서는 캐릭터가 입력 방향에 맞추어 forward / right 방향으로 이동, 캐릭터가 바라보는 방향은 카메라와 동일한 정면
                {
                    movement = transform.forward * movementInput.y + transform.right * movementInput.x;
                }
                else         // 비무장 상태 에서는 캐릭터가 앞으로만 이동, 캐릭터가 바라보는 방향은 카메라 방향과 무관하게 이동하는 방향을 바라봄
                {
                    if (isInputSomething)
                    {
                        movement = transform.forward;
                    }
                }
            }

            // moveSpeed 추가
            movement = movement * moveSpeed * Time.deltaTime;
            movement.y += verticalVelocity * Time.deltaTime;
            characterController.Move(movement);
            //characterController.Move(movement * moveSpeed * Time.deltaTime);
        }

        // 무장중일 때만 작동
        // 이 코드는 캐릭터가 플레이어일 때만 유효한 코드같아 보이는데, 그러면 PlayerCharacterController로 위치를 바꿔줘야 하는가?
        public void Rotate(Vector3 targetAimPoint)
        {
            if (isAiming)
            {
                // 캐릭터가 바라보고 있을 방향, 플레이어의 경우 CameraSystem에서 받아온 CameraAimPoint
                Vector3 aimTarget = targetAimPoint;
                // 캐릭터는 좌/우 로만 회전하기 때문에 y 값은 캐릭터의 y 좌표로 보정
                aimTarget.y = transform.position.y;
                Vector3 pos = transform.position;
                // 캐릭터가 보는 위치의 좌표와 현재 위치 좌표를 비교해서 바라보는 방향 찾기 
                Vector3 aimDirection = (aimTarget - pos).normalized;
                // 해당 방향으로 transform.forward 설정하여 캐릭터 회전
                transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 10f);
            }
        }

        public void ApplyGravity()
        {
            if (isGrounded)
            {
                if (jumpTrigger)
                {
                    verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
                    jumpTrigger = false;
                    characterAnimator.SetTrigger("Jump Trigger");

                }

                if (verticalVelocity <= 0f)
                {
                    verticalVelocity = -2f;
                }
            }
            else
            {
                verticalVelocity += gravity * Time.deltaTime;

                // 떨어지는 최대 속도를 정해줘야 한다
                //verticalVelocity = Mathf.Clamp(verticalVelocity, -terminalVelocity, Time.deltaTime * 10f);
            }
        }

        public void CheckGround()
        {
            isGrounded = Physics.CheckSphere(
                transform.position + (Vector3.up * groundOffset),
                groundCheckRadius,
                groundLayer,
                QueryTriggerInteraction.Ignore);

            // 떨어지는 애니메이션 추가
             characterAnimator.SetBool("IsGrounded", isGrounded);
        }

        public void SetWeaponEquipState(int weaponType)
        {
            if (!IsAlive || isChangingWeaponState) return;

            isChangingWeaponState = true;
            characterAnimator.SetInteger("Weapon Type", weaponType);

            if (!isArmed)
            {
                IsArmed = true;
                currentWeaponType = weaponType;
            }
            else
            {
                if (currentWeaponType == weaponType)
                {
                    if (isAiming)
                    {
                        IsAiming = false;
                    }
                    IsArmed = false;
                }
                else
                {
                    currentWeaponType = weaponType;
                }
            }

            SetCurrentWeapon();
        }

        // 현재 장착중인 무기의 WeaponBase를 currentWeaponBase로 설정
        private void SetCurrentWeapon()
        {
            if (currentWeaponType == (int)WeaponType.Rifle)
            {
                currentWeaponBase = rifleWeaponBase;
            }
            if (currentWeaponType == (int)WeaponType.Pistol)
            {
                currentWeaponBase = pistolWeaponBase;
            }
        }

        public void Shoot()
        {
            if (isArmed && !isAiming)
            {
                IsAiming = true;
            }

            float shootingAllowed = characterAnimator.GetFloat("ShootingAllowed");

            if (!isAiming || isReloading || isChangingWeaponState || shootingAllowed < 0.95f) return;

            if (currentWeaponBase.currentMagazine <= 0)
            {
                Reload();
                return;
            }
            currentWeaponBase.Shoot();
        }

        public void Reload()
        {
            // 총기 사용 중 재장전을 할 때 실행될 스크립트
            if (!IsArmed || isChangingWeaponState) return;

            if (!isArmed)
            {
                IsArmed = true;
            }

            if (!isReloading)
            {
                isChangingWeaponState = true;
                characterAnimator.SetTrigger("Reload Trigger");
                isReloading = true;
            }
        }

        public void PickUp(DropItem targetItem)
        {
            if (isArmed || isChangingWeaponState) return;

            targetPickUpItem = targetItem;

            isChangingWeaponState = true;
            characterAnimator.SetTrigger("Pick Up Trigger");
        }


        public void Jump()
        {
            if (isGrounded&&currentStamina>0)
            {
                currentStamina -= jumpStaminaCost;
                jumpTrigger = true;
            }
        }

        public void Roll()
        {
            if (isGrounded && currentStamina > 0)
            {
                currentStamina -= rollStaminaCost;
                //rollTrigger = true;
                characterAnimator.SetTrigger("Roll Trigger");
            }
        }

        private void OnPistolToHand()
        {
            weaponPistol.transform.SetParent(rightHandTransform);
            weaponPistol.transform.SetLocalPositionAndRotation(pistolEquipOffsetPos, Quaternion.Euler(pistolEquipOffsetRot));
        }

        private void OnPistolToWaist()
        {
            weaponPistol.transform.SetParent(rightWaistTransform);
            weaponPistol.transform.SetLocalPositionAndRotation(pistolHolsterOffsetPos, Quaternion.Euler(pistolHolsterOffsetRot));
        }

        private void OnRifleToHand()
        {
            weaponRifle.transform.SetParent(rightHandTransform);
            weaponRifle.transform.SetLocalPositionAndRotation(rifleEquipOffsetPos, Quaternion.Euler(rifleEquipOffsetRot));
        }

        private void OnRifleToBack()
        {
            weaponRifle.transform.SetParent(backTransform);
            weaponRifle.transform.SetLocalPositionAndRotation(rifleHolsterOffsetPos, Quaternion.Euler(rifleHolsterOffsetRot));
        }

        // 무기 장착/해제/변경 이벤트
        public void OnPistolEquipToHand()
        {
            OnPistolToHand();
        }

        public void OnPistolEquipComplete()
        {
            isChangingWeaponState = false;
        }

        public void OnPistolHolsterToWaist()
        {
            OnPistolToWaist();
        }

        public void OnPistolHolsterComplete()
        {
            isChangingWeaponState = false;
        }

        public void OnRifleEquipToHand()
        {
            OnRifleToHand();
        }

        public void OnRifleEquipComplete()
        {
            isChangingWeaponState = false;
        }

        public void OnRifleHolsterToBack()
        {
            OnRifleToBack();
        }

        public void OnRifleHolsterComplete()
        {
            isChangingWeaponState = false;
        }

        public void OnPistolToRiflePistolToWaist()
        {
            OnPistolToWaist();
        }

        public void OnPistolToRifleRifleToHand()
        {
            OnRifleToHand();
        }

        public void OnPistolToRifleComplete()
        {
            isChangingWeaponState = false;
        }

        public void OnRifleToPistolRifleToBack()
        {
            OnRifleToBack();
        }

        public void OnRifleToPistolPistolToHand()
        {
            OnPistolToHand();
        }

        public void OnRifleToPistolComplete()
        {
            isChangingWeaponState = false;
        }

        // 재장전 관련 모션 이벤트
        public void OnPistolStandRelaxedReloadEnd()
        {
            OnReloadEnd();
        }

        public void OnPistolStandAimReloadEnd()
        {
            OnReloadEnd();
        }

        public void OnPistolCrouchRelaxedReloadEnd()
        {
            OnReloadEnd();
        }

        public void OnPistolCrouchAimReloadEnd()
        {
            OnReloadEnd();
        }

        public void OnRifleStandRelaxedReloadEnd()
        {
            OnReloadEnd();
        }

        public void OnRifleStandAimReloadEnd()
        {
            OnReloadEnd();
        }

        public void OnRifleCrouchRelaxedReloadEnd()
        {
            OnReloadEnd();
        }

        public void OnRifleCrouchAimReloadEnd()
        {
            OnReloadEnd();
        }

        public void OnPickUpComplete()
        {
            isChangingWeaponState = false;
        }

        public System.Action<DropItem> OnItemPicked;

        public void OnItemPickUp()
        {
            if (targetPickUpItem != null)
            {
                Destroy(targetPickUpItem.gameObject);
                OnItemPicked?.Invoke(targetPickUpItem);
                targetPickUpItem = null;
            }
            // 실제 아이템 픽업 실행
            // 필드의 아이템 사라지게 처리
            // 플레이어의 인벤토리에 추가
        }

        private void OnReloadEnd()
        {
            // IK 관련 작업
            isChangingWeaponState = false;
            isReloading = false;
            currentWeaponBase.Reload();
        }
    }
}
