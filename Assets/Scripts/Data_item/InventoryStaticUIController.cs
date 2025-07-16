using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;  // Thêm dòng này

public class InventoryStaticUIController : MonoBehaviour
{
    public static InventoryStaticUIController Instance;
    public List<Button> slotButtons;
    public InventoryTooltipUI tooltipUI;
    public GameObject equipMenuPanel;
    public Button equipBtn;
    int lastRightClickSlot = -1;

    private InventoryManager inventoryManager;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        inventoryManager = InventoryManager.Instance;

        if (equipBtn != null)
            equipBtn.onClick.AddListener(OnEquipBtnClick);

        // Add hover & right click scripts
        for (int i = 0; i < slotButtons.Count; i++)
        {
            var btn = slotButtons[i];
            var hover = btn.GetComponent<InventorySlotHover>();
            if (hover == null) hover = btn.gameObject.AddComponent<InventorySlotHover>();

            var rightClick = btn.GetComponent<InventorySlotRightClick>();
            if (rightClick == null) rightClick = btn.gameObject.AddComponent<InventorySlotRightClick>();
            rightClick.Init(i);
        }
        // Load inventory when the game starts
        LoadInventory();
        UpdateInventorySlots();
    }


    /// <summary>
    /// Load inventory từ tệp JSON
    /// </summary>
    public void LoadInventory()
    {
        Debug.Log("Đã chạy LoadInventory()");

        string savePath = Path.Combine(Application.persistentDataPath, "inventory_data.json");
        Debug.Log("Đọc tệp JSON từ: " + savePath);

        // Kiểm tra nếu tệp JSON tồn tại trước khi tải
        if (File.Exists(savePath))
        {
            // Nạp vật phẩm vào ItemDatabase trước khi tải kho
            ItemDatabase.LoadItemsFromJson(savePath); // Đảm bảo gọi trước

            // Tải kho từ tệp JSON
            InventoryFileHandler.LoadInventoryFromFile(ref inventoryManager.inventoryItems);  // Không cần lưu kết quả

            // Kiểm tra nếu kho có vật phẩm
            if (inventoryManager.inventoryItems.Count > 0)
            {
                Debug.Log("Kho đã được tải thành công.");
            }
            else
            {
                Debug.LogWarning("Không có vật phẩm trong kho hoặc lỗi khi tải kho.");
                // Nếu kho trống, hiển thị trạng thái trống cho giao diện
            }
        }
        else
        {
            Debug.LogWarning("Không tìm thấy tệp kho. Sử dụng kho trống.");
            // Nếu không có tệp JSON, hiển thị kho trống
            inventoryManager.inventoryItems.Clear();  // Đảm bảo kho trống nếu không có tệp
        }

        // Cập nhật UI kho sau khi tải
        UpdateInventorySlots();
    }

    // Cập nhật các ô kho trong UI
    public void UpdateInventorySlots()
    {
        var inventoryItems = inventoryManager.inventoryItems;
        for (int i = 0; i < slotButtons.Count; i++)
        {
            var btn = slotButtons[i];
            var iconImg = btn.transform.Find("Icon")?.GetComponent<Image>();  // Kiểm tra sự tồn tại của Icon
            var amountText = btn.transform.Find("amount")?.GetComponent<TMPro.TMP_Text>();  // Kiểm tra TextMeshPro (TMP_Text)
            var qualityUI = btn.GetComponentInChildren<ItemQualityUI>();  // Cập nhật chất lượng

            InventoryManager.InventorySlot slot = (inventoryItems != null && i < inventoryItems.Count) ? inventoryItems[i] : null;

            // Xóa sự kiện cũ trước khi thêm mới
            btn.onClick.RemoveAllListeners();

            // Gán lại sự kiện hover cho mỗi slot
            var hover = btn.GetComponent<InventorySlotHover>();
            if (hover == null) hover = btn.gameObject.AddComponent<InventorySlotHover>();

            // Gán lại sự kiện right-click cho mỗi slot
            var rightClick = btn.GetComponent<InventorySlotRightClick>();
            if (rightClick == null) rightClick = btn.gameObject.AddComponent<InventorySlotRightClick>();

            // Nếu slot có vật phẩm
            if (slot != null && slot.item != null)
            {
                // Hiển thị icon vật phẩm
                if (iconImg != null)
                {
                    iconImg.sprite = slot.item.icon;
                    iconImg.enabled = true;
                    iconImg.preserveAspect = true;  // Đảm bảo tỷ lệ khung hình icon
                }

                // Hiển thị số lượng chồng (stack) nếu số lượng > 1
                if (amountText != null)
                {
                    amountText.text = slot.amount > 1 ? slot.amount.ToString() : "";  // Hiển thị số lượng stack lên nút
                }

                // Cập nhật chất lượng vật phẩm
                if (qualityUI != null)
                {
                    qualityUI.itemData = slot.item;
                    qualityUI.UpdateQualityFrame();  // Cập nhật khung chất lượng
                }

                // Thiết lập sự kiện cho button
                int idx = i;
                btn.onClick.AddListener(() =>
                {
                    if (idx < inventoryItems.Count && inventoryItems[idx].item != null)
                        OnClickItem(inventoryItems[idx].item);
                });

                // Cập nhật hover cho vật phẩm
                hover.Setup(slot.item, tooltipUI);
                rightClick.UpdateItem(slot.item);  // Cập nhật sự kiện right-click cho vật phẩm
            }
            else
            {
                // Slot trống: ẩn icon và số lượng
                if (iconImg != null)
                {
                    iconImg.sprite = null;  // Đặt sprite icon là null
                    iconImg.enabled = false;  // Ẩn icon
                }

                if (amountText != null)
                {
                    amountText.text = "";  // Ẩn số lượng
                }

                if (qualityUI != null)
                {
                    qualityUI.itemData = null;
                    qualityUI.UpdateQualityFrame();  // Cập nhật chất lượng (không có vật phẩm)
                }

                hover.Setup(null, tooltipUI);  // Cập nhật hover khi không có vật phẩm trong slot
                rightClick.UpdateItem(null);  // Cập nhật sự kiện right-click khi không có vật phẩm
            }
        }
    }

    public void ShowEquipMenu(int slotIndex, Vector3 screenPosition)
    {
        var slot = inventoryManager.inventoryItems[slotIndex];
        if (slot != null && slot.item.itemType == ItemType.Currency)
        {
            equipMenuPanel.SetActive(false);
            return;
        }
        lastRightClickSlot = slotIndex;

        equipMenuPanel.SetActive(true);

        Canvas canvas = equipMenuPanel.GetComponentInParent<Canvas>();
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        RectTransform menuRect = equipMenuPanel.GetComponent<RectTransform>();

        Vector2 offset = new Vector2(24, -24);

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screenPosition + (Vector3)offset,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
            out localPoint
        );

        Vector2 size = menuRect.sizeDelta;
        Vector2 canvasSize = canvasRect.sizeDelta;

        localPoint.x = Mathf.Clamp(localPoint.x, size.x / 2, canvasSize.x / 2 - size.x / 2);
        localPoint.y = Mathf.Clamp(localPoint.y, -canvasSize.y / 2 + size.y / 2, canvasSize.y / 2 - size.y / 2);

        menuRect.anchoredPosition = localPoint;
    }

    public void HideEquipMenu()
    {
        equipMenuPanel.SetActive(false);
    }

    void OnEquipBtnClick()
    {
        var inventoryItems = inventoryManager.inventoryItems;
        if (inventoryItems != null && inventoryItems.Count > lastRightClickSlot && inventoryItems[lastRightClickSlot] != null)
        {
            var slot = inventoryItems[lastRightClickSlot];
            EquipmentManager.Instance.Equip(slot.item);
            inventoryManager.RemoveItem(slot.item, 1);
            HideEquipMenu();
        }
    }

    void OnClickItem(ItemData item)
    {
        Debug.Log("Đã chọn item: " + item.itemName);
        // Xử lý hiện detail hoặc dùng item, tuỳ nhu cầu
    }
}
