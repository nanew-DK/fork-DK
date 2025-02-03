using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform target;  // 플레이어
    public Vector3 offset = new Vector3(0, 2.5f, -10);
    public float smoothSpeed = 0.125f;  // 부드러운 이동 속도

    void LateUpdate()
    {
        if (target != null)
        {
            // 대상의 위치에 오프셋 적용
            Vector3 desiredPosition = target.position + offset;
            // 부드럽게 이동
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}
