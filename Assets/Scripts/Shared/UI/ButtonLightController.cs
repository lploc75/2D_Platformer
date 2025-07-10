using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;

public class ButtonLightController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Light2D targetLight; // Gán Light 2D tương ứng vào đây

    void Start()
    {
        if (targetLight != null)
            targetLight.enabled = false; // Mặc định tắt
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (targetLight != null)
            targetLight.enabled = true; // Hover bật sáng
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (targetLight != null)
            targetLight.enabled = false; // Rời chuột tắt
    }
}
