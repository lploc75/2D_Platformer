﻿using UnityEngine;

public class CameraFollows : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    public float minX = -11.97f; // Giới hạn trái camera
    public float minY = -1f;     // Giới hạn dưới camera (Y)

    public Vector3 shakeOffset = Vector3.zero; // THÊM DÒNG NÀY

    void LateUpdate()
    {
        if (target == null) return;

        // Clamp vị trí mong muốn của camera trên cả X và Y
        float desiredX = Mathf.Max(target.position.x + offset.x, minX);
        float desiredY = Mathf.Max(target.position.y + offset.y, minY);

        Vector3 desiredPosition = new Vector3(desiredX, desiredY, transform.position.z);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // THÊM shakeOffset vào vị trí cuối
        transform.position = smoothedPosition + shakeOffset;
    }
}
