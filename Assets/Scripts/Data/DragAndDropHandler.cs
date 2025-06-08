using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDropHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public Image icon; // Icon của vật phẩm (được kéo)
    public GameObject originalSlot; // Vị trí gốc của vật phẩm
    public GameObject currentSlot;  // Vị trí hiện tại khi thả vật phẩm

    private CanvasGroup canvasGroup;
    private Vector3 originalPosition;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        originalPosition = transform.position;
    }

    // Khi bắt đầu kéo
    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;  // Ngừng block các sự kiện raycast (cho phép kéo)
        originalSlot = this.gameObject;      // Lưu lại vị trí gốc
        icon = GetComponent<Image>();        // Lấy icon của vật phẩm
    }

    // Khi đang kéo
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;  // Di chuyển vật phẩm theo vị trí kéo
    }

    // Khi kết thúc kéo
    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;  // Bật lại block raycasts
        if (currentSlot != null)
        {
            // Di chuyển vật phẩm đến vị trí mới (đã thả)
            transform.position = currentSlot.transform.position;
            currentSlot.GetComponent<Slot>().SetItem(icon.sprite);  // Cập nhật icon trong slot
        }
        else
        {
            // Nếu không thả vào slot hợp lệ, quay lại vị trí gốc
            transform.position = originalPosition;
        }
    }

    // Khi vật phẩm được thả vào một slot mới
    public void OnDrop(PointerEventData eventData)
    {
        currentSlot = eventData.pointerEnter; // Vị trí slot mới khi thả
    }
}
