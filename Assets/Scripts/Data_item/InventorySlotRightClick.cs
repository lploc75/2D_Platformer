using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlotRightClick : MonoBehaviour, IPointerClickHandler
{
    private int slotIndex;
    private ItemData item; // Chuẩn

    public void Init(int index)
    {
        slotIndex = index;
    }

    public void UpdateItem(ItemData item)
    {
        this.item = item;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right && item != null)
        {
            Debug.Log("Chuột phải tại: " + eventData.position);
            InventoryStaticUIController.Instance.ShowEquipMenu(slotIndex, eventData.position); // Lấy đúng vị trí chuột
        }
    }



}

