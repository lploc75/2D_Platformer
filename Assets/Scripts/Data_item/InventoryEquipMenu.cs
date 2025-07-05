using UnityEngine;
using UnityEngine.UI;

public class InventoryEquipMenu : MonoBehaviour
{
    public Button equipButton;

    [HideInInspector]
    public ItemData currentItem;     // Item hiện tại chuột phải vào
    [HideInInspector]
    public int currentSlotIndex;     // Slot trong inventory

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    // Hiện menu tại vị trí chuột, với item đang chọn
    public void Show(ItemData item, int slotIndex, Vector3 screenPosition)
    {
        currentItem = item;
        currentSlotIndex = slotIndex;
        transform.position = screenPosition;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        currentItem = null;
    }
}
