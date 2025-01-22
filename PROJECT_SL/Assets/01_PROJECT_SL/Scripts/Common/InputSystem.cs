using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectSL
{
    public class InputSystem : SingletonBase<InputSystem>
    {

        public Vector2 Movement { get; private set; }
        public Vector2 Look { get; private set; }
        public bool IsLeftShift => Input.GetKey(KeyCode.LeftShift);
        public bool IsLeftMouseButton => Input.GetMouseButton(0);
        public bool IsRightMouseButton => Input.GetMouseButtonDown(1);

        public System.Action OnClickedSpace;
        public System.Action OnClickedLeftControl;
        public System.Action OnClickedCrouch;
        public System.Action OnClickedAlpha1;
        public System.Action OnClickedReload;

        public System.Action OnMouseWheelUp;
        public System.Action OnMouseWheelDown;

        public System.Action OnClickedInteraction;

        public System.Action OnClickedPauseButton;

        private bool isInitialized = false;
        
        public void Initialize()
        {
            if (isInitialized) return;

            SetVisibleCursor(false);
            isInitialized = true;
        }

        private void Update()
        {
            SetVisibleCursor(Input.GetKey(KeyCode.LeftAlt));

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

            if (Input.GetKeyDown(KeyCode.F))
            {
                OnClickedInteraction?.Invoke();
            }

            if (Input.GetKeyDown(KeyCode.BackQuote))
            {
                OnClickedPauseButton?.Invoke();
            }

            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            Movement = new Vector2(horizontal, vertical);

            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");
            Look = new Vector2(mouseX, mouseY);

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

        private void SetVisibleCursor(bool isVisible)
        {
            Cursor.visible = isVisible;
            Cursor.lockState = isVisible ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }
}
