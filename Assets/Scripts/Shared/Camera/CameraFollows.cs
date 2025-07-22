using UnityEngine;

public class CameraFollows : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    public float minX = -11.97f; // Giới hạn trái camera
    public float maxX = 100f;    // Giới hạn phải camera (đặt phù hợp với map của bạn)
    public float minY = -1f;     // Giới hạn dưới camera (Y)

    public Vector3 shakeOffset = Vector3.zero; // Hiệu ứng rung camera (nếu có)

    void LateUpdate()
    {
        if (target == null) return;

        // Vị trí mong muốn của camera có offset
        float desiredX = Mathf.Clamp(target.position.x + offset.x, minX, maxX);
        float desiredY = Mathf.Max(target.position.y + offset.y, minY);

        Vector3 desiredPosition = new Vector3(desiredX, desiredY, transform.position.z);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Thêm hiệu ứng rung (nếu có)
        transform.position = smoothedPosition + shakeOffset;
    }
}
