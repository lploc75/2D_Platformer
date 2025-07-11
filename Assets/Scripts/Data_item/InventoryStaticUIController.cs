    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections.Generic;

    public class InventoryStaticUIController : MonoBehaviour
    {
        public static InventoryStaticUIController Instance;
        public List<Button> slotButtons;                 // Kéo các Button slot vào đây (phải đủ số slot)
        public InventoryTooltipUI tooltipUI;
        public GameObject equipMenuPanel;
        public Button equipBtn;
        int lastRightClickSlot = -1;

        // Reference tới InventoryManager
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

            // Đảm bảo mỗi slot đều có hover/right click script
            for (int i = 0; i < slotButtons.Count; i++)
            {
                var btn = slotButtons[i];
                var hover = btn.GetComponent<InventorySlotHover>();
                if (hover == null) hover = btn.gameObject.AddComponent<InventorySlotHover>();

                var rightClick = btn.GetComponent<InventorySlotRightClick>();
                if (rightClick == null) rightClick = btn.gameObject.AddComponent<InventorySlotRightClick>();
                rightClick.Init(i);
            }

            UpdateInventorySlots();
        }

        public void UpdateInventorySlots()
        {
            var inventoryItems = inventoryManager.inventoryItems;
            for (int i = 0; i < slotButtons.Count; i++)
            {
                var btn = slotButtons[i];
                var iconImg = btn.transform.Find("Icon").GetComponent<Image>();
                var amountText = btn.transform.Find("Amount")?.GetComponent<Text>(); // Hoặc TMP_Text
                var qualityUI = btn.GetComponentInChildren<ItemQualityUI>();
                var hover = btn.GetComponent<InventorySlotHover>();
                var rightClick = btn.GetComponent<InventorySlotRightClick>();

                InventoryManager.InventorySlot slot = (inventoryItems != null && i < inventoryItems.Count) ? inventoryItems[i] : null;

                btn.onClick.RemoveAllListeners();

                if (slot != null && slot.item != null)
                {
                    iconImg.sprite = slot.item.icon;
                    iconImg.enabled = true;
                    iconImg.preserveAspect = true;

                    // Hiển thị số lượng (stack)
                    if (amountText != null)
                        amountText.text = slot.amount > 1 ? slot.amount.ToString() : "";

                    if (qualityUI != null)
                    {
                        qualityUI.itemData = slot.item;
                        qualityUI.UpdateQualityFrame();
                    }

                    int idx = i;
                    btn.onClick.AddListener(() =>
                    {
                        if (idx < inventoryItems.Count && inventoryItems[idx].item != null)
                            OnClickItem(inventoryItems[idx].item);
                    });

                    hover.Setup(slot.item, tooltipUI);
                    rightClick.UpdateItem(slot.item);
                }
                else
                {
                    // Slot trống
                    iconImg.sprite = null;
                    iconImg.enabled = false;

                    if (amountText != null)
                        amountText.text = "";

                    if (qualityUI != null)
                    {
                        qualityUI.itemData = null;
                        qualityUI.UpdateQualityFrame();
                    }

                    hover.Setup(null, tooltipUI);
                    rightClick.UpdateItem(null);
                }
            }
        }

    public void ShowEquipMenu(int slotIndex, Vector3 screenPosition)
    {
        var slot = inventoryManager.inventoryItems[slotIndex];
        if (slot != null && slot.item.itemType == ItemType.Currency)
        {
            equipMenuPanel.SetActive(false); // Không hiện menu Equip cho currency
            return;
        }
        lastRightClickSlot = slotIndex;

        equipMenuPanel.SetActive(true);

        Canvas canvas = equipMenuPanel.GetComponentInParent<Canvas>();
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        RectTransform menuRect = equipMenuPanel.GetComponent<RectTransform>();

        // Thêm offset cho popup để không che vào chuột
        Vector2 offset = new Vector2(24, -24); // X lệch sang phải, Y lệch xuống, có thể chỉnh

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screenPosition + (Vector3)offset,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
            out localPoint
        );

        Vector2 size = menuRect.sizeDelta;
        Vector2 canvasSize = canvasRect.sizeDelta;

        // Giới hạn để menu không tràn ra ngoài canvas
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
                inventoryManager.RemoveItem(slot.item, 1);   // Giảm số lượng hoặc xóa khỏi inventory nếu amount = 1
                HideEquipMenu();
            }
        }

        void OnClickItem(ItemData item)
        {
            Debug.Log("Đã chọn item: " + item.itemName);
            // Xử lý hiện detail hoặc dùng item, tuỳ nhu cầu
        }
    }