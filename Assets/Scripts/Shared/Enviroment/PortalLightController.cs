using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal; 
using System.Collections;

public class PortalLightController : MonoBehaviour
{
    public Light2D portalLight;    // Gán Light2D trong Inspector
    public float flashMax = 400f;  // Giá trị lóe tối đa
    public float flashDuration = 1f; // Tổng thời gian lóe (từ 0->max->0)

    bool playerInZone = false;
    bool isFlashing = false;

    void Start()
    {
        if (portalLight != null) portalLight.intensity = 0;
    }

    void Update()
    {
        // Input System mới
        if (playerInZone && !isFlashing && Keyboard.current.eKey.wasPressedThisFrame)
        {
            StartCoroutine(FlashLight());
        }
    }

    private IEnumerator FlashLight()
    {
        isFlashing = true;
        float timer = 0f;

        // Lên đỉnh lóe (0 -> max)
        while (timer < flashDuration / 2f)
        {
            float t = timer / (flashDuration / 2f);
            portalLight.intensity = Mathf.Lerp(0, flashMax, t);
            timer += Time.deltaTime;
            yield return null;
        }
        portalLight.intensity = flashMax;

        // Xuống lại (max -> 0)
        timer = 0f;
        while (timer < flashDuration / 2f)
        {
            float t = timer / (flashDuration / 2f);
            portalLight.intensity = Mathf.Lerp(flashMax, 0, t);
            timer += Time.deltaTime;
            yield return null;
        }
        portalLight.intensity = 0;
        isFlashing = false;
    }

    // Check người chơi vào/ra vùng trigger
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
            playerInZone = true;
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
            playerInZone = false;
    }
}
