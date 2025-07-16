using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

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

        UpdateInventorySlots();
    }

    public void UpdateInventorySlots()
    {
        var inventoryItems = inventoryManager.inventoryItems;
        for (int i = 0; i < slotButtons.Count; i++)
        {
            var btn = slotButtons[i];
            var iconImg = btn.transform.Find("Icon").GetComponent<Image>();
            var amountText = btn.transform.Find("amount")?.GetComponent<TMPro.TMP_Text>();

            // Nếu dùng TMP_Text thì thay Text thành TMP_Text

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

                // Hiện số lượng nếu amount > 1, ẩn nếu <= 1
                if (amountText != null)
                {
                    if (slot.amount > 1)
                    {
                        amountText.enabled = true;
                        amountText.text = slot.amount.ToString();
                    }
                    else
                    {
                        amountText.enabled = false;
                        amountText.text = "";
                    }
                }

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
                // Slot trống: ẩn icon, ẩn text số lượng
                iconImg.sprite = null;
                iconImg.enabled = false;

                if (amountText != null)
                {
                    amountText.enabled = false;
                    amountText.text = "";
                }

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
