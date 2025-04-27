using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ProjectSL
{
    public class InputSystem : SingletonBase<InputSystem>
    {
        public bool IsForceCursorVisible 
        {
            get => isForceCursorVisible;
            set
            {
                isForceCursorVisible = value;
                SetVisibleCursor(value || Input.GetKey(KeyCode.LeftAlt));
            }
        }
        [field: SerializeField] private bool isForceCursorVisible = false;


        public Vector2 Movement { get; private set; }
        public Vector2 Look { get; private set; }
        public bool IsLeftShift => Input.GetKey(KeyCode.LeftShift);
        public bool IsLeftMouseButton => Input.GetMouseButton(0);
        public bool IsRightMouseButton => Input.GetMouseButton(1);

        public System.Action OnClickedSpace;            // Jump
        public System.Action OnClickedLeftControl;      // Roll
        public System.Action OnClickedCrouch;
        public System.Action OnClickedAlpha1;
        public System.Action OnClickedAlpha2;
        public System.Action OnClickedReload;

        public System.Action OnMouseWheelUp;
        public System.Action OnMouseWheelDown;

        public System.Action OnClickedInteraction;      // F
        public System.Action OnClickInventory;          // I
        public System.Action OnClickInventoryTemp;      // O
        public System.Action OnClickedPauseButton;      // esc

        private bool isInitialized = false;

        private float horizontal;
        private float vertical;


        public void Initialize()
        {
            if (isInitialized) return;

            SetVisibleCursor(false);
            isInitialized = true;
        }

        private void Update()
        {
            // 마우스 포인터가 UI 위에 올라가 있는지 확인하는 방법
            // EventSystem > 인스펙터에서 디버그 모드 > Standalone Input Module 에 있는 값
            //bool isMouseOverUI = EventSystem.current.IsPointerOverGameObject();
            
            if (!isForceCursorVisible)
            {
                SetVisibleCursor(Input.GetKey(KeyCode.LeftAlt));
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                OnClickedSpace?.Invoke();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                OnClickedReload?.Invoke();
            }

            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                OnClickedLeftControl?.Invoke();
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                OnClickedCrouch?.Invoke();
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                OnClickedAlpha1?.Invoke();
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                OnClickedAlpha2?.Invoke();
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                OnClickedInteraction?.Invoke();
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                OnClickInventory?.Invoke();
            }

            if (Input.GetKeyDown(KeyCode.O))
            {
                OnClickInventoryTemp?.Invoke();
            }

            if (Input.GetKeyDown(KeyCode.BackQuote))
            {
                OnClickedPauseButton?.Invoke();
            }

            if (PlayerCharacterController.Instance != null && !PlayerCharacterController.Instance.LinkedCharacter.fixDirection)
            {
                horizontal = Input.GetAxis("Horizontal");
                vertical = Input.GetAxis("Vertical");
            }

            Movement = new Vector2(horizontal, vertical);

            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            Look = isForceCursorVisible ? Vector2.zero : new Vector2(mouseX, mouseY);
            //Look = new Vector2(mouseX, mouseY);

            // 마우스 휠을 올렸을 때
            if (Input.mouseScrollDelta.y > 0)
            {
                OnMouseWheelUp?.Invoke();
            }
            // 마우스 휠을 내렸을 때
            else if (Input.mouseScrollDelta.y < 0)
            {
                 OnMouseWheelDown?.Invoke();
            }
        }

        public void SetVisibleCursor(bool isVisible)
        {
            Cursor.visible = isVisible;
            Cursor.lockState = isVisible ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }
}
