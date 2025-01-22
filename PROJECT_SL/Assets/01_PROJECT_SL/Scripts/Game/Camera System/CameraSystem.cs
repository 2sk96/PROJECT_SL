using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectSL
{
    public class CameraSystem : MonoBehaviour
    {
        public static CameraSystem Instance { get; private set; }
        public Vector3 CameraAimingPoint { get; private set; }

        private Camera mainCamera;
        public Transform aimSphere;


        private void Awake()
        {
            Instance = this;
            mainCamera = Camera.main;
        }

        private void Update()
        {
            // 카메라에서 viewport point로 가는 광선을 만들어준다
            Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 1f));

            // 인자로 받은 ray의 정보를 바탕으로 최대거리만큼 ray를 쏘는 함수, collider와 충돌이 있으면 true 반환, 없으면 false 반환
            if (Physics.Raycast(ray, out RaycastHit hitinfo, 1000f))
            {
                CameraAimingPoint = hitinfo.point;
            }
            else
            {
                CameraAimingPoint = ray.GetPoint(1000f);
            }
            // 충돌이 있었다면 해당 콜라이더와 충돌이 일어난 지점 좌표
            // 충돌이 없다면 그냥 이 ray가 1000f만큼 간 후의 좌표

            aimSphere.transform.position = CameraAimingPoint;
        }
    }
}
