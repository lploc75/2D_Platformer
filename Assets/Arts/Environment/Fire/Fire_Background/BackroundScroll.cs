using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    public float parallaxFactor = 0.5f; // Xa nhất: 0.1, gần nhất: 1
    private Transform cam;
    private Vector3 prevCamPos;

    void Start()
    {
        cam = Camera.main.transform;
        prevCamPos = cam.position;
    }

    void LateUpdate()
    {
        Vector3 delta = cam.position - prevCamPos;
        transform.position += new Vector3(delta.x * parallaxFactor, delta.y * parallaxFactor, 0);
        prevCamPos = cam.position;
    }
}
