using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace ProjectSL
{
    public class CharacterBase : MonoBehaviour
    {
        public bool IsRun
        {
            get => isRun;
            set
            {
                if (IsCrouch)
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
                SetEquipState(isArmed);
            }
        }
        public bool IsAiming
        {
            get => isAiming;
            set
            {
                if (!isArmed) return;
                isAiming = value;
            }
        }
        public float MoveSpeed => moveSpeed;
        public bool IsAlive => currentHealth > 0f;
        public Transform CameraPivot { get; private set; }

        public float currentHealth;
        public float currentStamina;
        public float maxStamina = 100f;
        public float maxHealth = 100f;

        public float walkSpeed = 2.0f;
        public float runSpeed = 7.0f;



        private Animator characterAnimator;
        private CharacterController characterController;

        private Vector2 movementInput;

        [SerializeField] private float moveSpeed;       // 실제 캐릭터의 이동량에 영향을 주는 속도 값
        [SerializeField] private float targetSpeed;     // Animator의 parameter로 사용하기 위한 속도 값
        private float smoothHorizontal;                 // Animator의 parameter로 사용하기 위한 값
        private float smoothVertical;                   // Animator의 parameter로 사용하기 위한 값
        private float smoothCrouch;                     // Animator의 parameter로 사용하기 위한 값
        private float smoothAiming;

        private float targetRotation;
        private float rotationVelocity;
        private float rotationSmoothTime = 0.1f;

        [SerializeField] private bool isRun = false;
        [SerializeField] private bool isArmed = false;
        [SerializeField] private bool isAiming = false;

        private void Awake()
        {
            characterAnimator = GetComponent<Animator>();
            characterController = GetComponent<CharacterController>();

            CameraPivot = transform.Find("CameraPivot");
        }

        private void Start()
        {
            currentHealth = maxHealth;
            currentStamina = maxStamina;
        }

        private void Update()
        {
            smoothHorizontal = Mathf.Lerp(smoothHorizontal, movementInput.x, Time.deltaTime * 10f);
            smoothVertical = Mathf.Lerp(smoothVertical, movementInput.y, Time.deltaTime * 10f);
            smoothCrouch = Mathf.Lerp(smoothCrouch, IsCrouch ? 1.0f : 0f, Time.deltaTime * 5f);
            smoothAiming = Mathf.Lerp(smoothAiming, isAiming ? 1.0f : 0f, Time.deltaTime * 10f);

            characterAnimator.SetFloat("Speed", targetSpeed);
            characterAnimator.SetFloat("Horizontal", smoothHorizontal);
            characterAnimator.SetFloat("Vertical", smoothVertical);
            characterAnimator.SetFloat("Crouch", smoothCrouch);
            characterAnimator.SetFloat("Aiming", smoothAiming);
            
        }

        public void Move(Vector2 input, float yAxisAngle)
        {
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

            if (isInputSomething && !isArmed)
            {
                Vector3 inputDirection = new Vector3(input.x, 0f, input.y).normalized;
                targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + yAxisAngle;
                // SmoothDampAngle: 현재 각도 (transform.eulerAngles.y) 에서 원하는 각도 (targetRotation) 까지 천천히 각도를 변경해준다. 부드러운 회전 구현
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity, rotationSmoothTime);
                transform.rotation = Quaternion.Euler(0f, rotation, 0f);
            }

            Vector3 movement = Vector3.zero;
            if (isArmed) // 무장상태 에서는 캐릭터가 입력 방향에 맞추어 forward / right 방향으로 이동, 캐릭터가 바라보는 방향은 카메라와 동일한 정면
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

            // 중력 아직 미구현
            //movementInput.y += verticalVelocity;
            characterController.Move(movement * moveSpeed * Time.deltaTime);
        }

        // 무장중일 때만 작동
        // 이 코드는 캐릭터가 플레이어일 때만 유효한 코드같아 보이는데, 그러면 PlayerCharacterController로 위치를 바꿔줘야 하는가?
        public void Rotate(Vector3 targetAimPoint)
        {
            if (isArmed)
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

        public void SetEquipState(bool isEquip)
        {
            
            characterAnimator.SetBool("IsArmed", isEquip);

            // Equip/Holster 애니메이션 아직 미구현
            if (isEquip)
            {
                //characterAnimator.SetTrigger("Equip Trigger");
            }
            else
            {
                //characterAnimator.SetTrigger("Holster Trigger");
            }
        }

        public void Shoot()
        {
            // 총기 사용 중 발사를 할 때 실행될 스크립트
        }

        public void Reload()
        {
            // 총기 사용 중 재장전을 할 때 실행될 스크립트
        }
    }
}
