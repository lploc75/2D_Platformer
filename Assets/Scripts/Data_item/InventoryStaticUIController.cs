using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryStaticUIController : MonoBehaviour
{
    public static InventoryStaticUIController Instance;
    public List<Button> slotButtons;              // Các button slot inventory (kéo vào)
    public List<ItemData> inventoryItems;         // Dữ liệu item (đa dụng)
    public InventoryTooltipUI tooltipUI;
    public GameObject equipMenuPanel;
    public Button equipBtn;
    int lastRightClickSlot = -1;

    void Awake() { Instance = this; }

    void Start()
    {
        // Lắng nghe nút Equip
        if (equipBtn != null)
            equipBtn.onClick.AddListener(OnEquipBtnClick);

        // Gán script chuột phải cho từng slot
        for (int i = 0; i < slotButtons.Count; i++)
        {
            var btn = slotButtons[i];
            var rightClick = btn.GetComponent<InventorySlotRightClick>();
            if (rightClick == null) rightClick = btn.gameObject.AddComponent<InventorySlotRightClick>();
            rightClick.Init(i);
        }

        UpdateInventorySlots();
    }

    // Hàm hiển thị menu equip ở vị trí chuột phải
    public void ShowEquipMenu(int slotIndex, Vector3 screenPosition)
    {
        lastRightClickSlot = slotIndex;

        equipMenuPanel.SetActive(true);

        // Xác định Canvas
        Canvas canvas = equipMenuPanel.GetComponentInParent<Canvas>();
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        RectTransform menuRect = equipMenuPanel.GetComponent<RectTransform>();

        // Chuyển screen position thành local position trên canvas
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screenPosition,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
            out localPoint
        );
        // Đặt menu về đúng vị trí
        menuRect.anchoredPosition = localPoint;

        // (Option) Nếu thấy bị biến mất, thử code này đảm bảo menu không văng khỏi màn hình:
        Vector2 size = menuRect.sizeDelta;
        Vector2 canvasSize = canvasRect.sizeDelta;
        // Giới hạn menu không ra ngoài (chạm lề phải/lề dưới)
        localPoint.x = Mathf.Clamp(localPoint.x, size.x / 2, canvasSize.x / 2 - size.x / 2);
        localPoint.y = Mathf.Clamp(localPoint.y, -canvasSize.y / 2 + size.y / 2, canvasSize.y / 2 - size.y / 2);
        menuRect.anchoredPosition = localPoint;
    }


    public void HideEquipMenu()
    {
        equipMenuPanel.SetActive(false);
    }

    // Trang bị vật phẩm từ inventory slot
    void OnEquipBtnClick()
    {
        if (inventoryItems != null && inventoryItems.Count > lastRightClickSlot && inventoryItems[lastRightClickSlot] != null)
        {
            var item = inventoryItems[lastRightClickSlot];
            EquipmentManager.Instance.Equip(item);
            RemoveItemAt(lastRightClickSlot);   // Xoá khỏi inventory sau khi trang bị
            HideEquipMenu();
        }
    }

    // Cập nhật UI các slot
    public void UpdateInventorySlots()
    {
        for (int i = 0; i < slotButtons.Count; i++)
        {
            var btn = slotButtons[i];
            var iconImg = btn.transform.Find("Icon").GetComponent<Image>();
            var qualityUI = btn.GetComponentInChildren<ItemQualityUI>();
            var hover = btn.GetComponent<InventorySlotHover>();
            if (hover == null) hover = btn.gameObject.AddComponent<InventorySlotHover>();

            var rightClick = btn.GetComponent<InventorySlotRightClick>();
            if (rightClick == null) rightClick = btn.gameObject.AddComponent<InventorySlotRightClick>();
            rightClick.Init(i);

            ItemData item = (inventoryItems != null && i < inventoryItems.Count) ? inventoryItems[i] : null;

            btn.onClick.RemoveAllListeners();

            if (item != null)
            {
                iconImg.sprite = item.icon;
                iconImg.enabled = true;
                iconImg.preserveAspect = true;

                if (qualityUI != null)
                {
                    qualityUI.itemData = item;
                    qualityUI.UpdateQualityFrame();
                }

                int idx = i;
                btn.onClick.AddListener(() =>
                {
                    if (idx < inventoryItems.Count && inventoryItems[idx] != null)
                        OnClickItem(inventoryItems[idx]);
                });

                hover.Setup(item, tooltipUI);
                rightClick.UpdateItem(item);
            }
            else
{
    // Slot trống
    iconImg.sprite = null;
    iconImg.enabled = false;
    if (qualityUI != null)
    {
        qualityUI.itemData = null;
        qualityUI.UpdateQualityFrame(); // Để script tự lo bật/tắt
    }

    hover.Setup(null, tooltipUI);
    rightClick.UpdateItem(null);
}

        }
    }

    void OnClickItem(ItemData item)
    {
        Debug.Log("Đã chọn item: " + item.itemName);
        // Xử lý hiện detail hoặc dùng item, tuỳ nhu cầu
    }

    public void RemoveItemAt(int index)
    {
        if (index >= 0 && index < inventoryItems.Count)
        {
            inventoryItems.RemoveAt(index);
            UpdateInventorySlots();
        }
    }
}
