using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectSL
{
    public class CrosshairUI : UIBase
    {
        public RectTransform left;
        public RectTransform right;
        public RectTransform top;
        public RectTransform bottom;

        public float minSpread = 40f;
        public float maxSpread = 140f;
        public float targetSpread;
        public float currentSpread;
        public float recoverySpeed;

        private void Awake()
        {
            targetSpread = minSpread;
            currentSpread = minSpread;
        }

        public override void Show()
        {
            base.Show();
        }

        public override void Hide()
        {
            base.Hide();
        }

        private void Update()
        {
            // 현재 crosshair의 위치가 minSpread보다 밖일 경우 currentSpread를 minSpread가 될 때 까지 감소
            if (currentSpread > minSpread)
            {
                currentSpread -= recoverySpeed * Time.deltaTime;
                currentSpread = Mathf.Max(currentSpread, minSpread);
                UpdateCrosshairPosition();
            }
        }

        // 정확도 100 이면 크로스헤어 움직임 없음
        // 정확도가 낮을수록 크로스헤어 움직임 커짐
        public void SpreadCrosshair(float accuracy)
        {
            // accuracy가 낮아질수록 spreadAmount는 커짐
            // spreadAmount에 맞는 targetSpread 및 currentSpread 설정
            float spreadAmount = (100f - accuracy) / 100f;
            targetSpread = Mathf.Lerp(minSpread, maxSpread, spreadAmount);
            currentSpread = Mathf.Max(targetSpread, minSpread);
            UpdateCrosshairPosition();
        }
        private void UpdateCrosshairPosition()
        {
            left.anchoredPosition = new Vector2(-currentSpread, 0f);
            right.anchoredPosition = new Vector2(currentSpread, 0f);
            top.anchoredPosition = new Vector2(0f, currentSpread);
            bottom.anchoredPosition = new Vector2(0f, -currentSpread);
        }
    }
}
