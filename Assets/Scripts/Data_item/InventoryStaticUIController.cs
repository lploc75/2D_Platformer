using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryStaticUIController : MonoBehaviour
{
    public List<Button> slotButtons;          // Kéo lần lượt các Button vào danh sách này theo thứ tự
    public List<WeaponData> weaponList;       // Kéo các WeaponData asset vào đây
    public InventoryTooltipUI tooltipUI;

    void Start()
    {
        UpdateInventorySlots();
    }

    public void UpdateInventorySlots()
    {
        for (int i = 0; i < slotButtons.Count; i++)
        {
            var btn = slotButtons[i];
            var iconImg = btn.transform.Find("Icon").GetComponent<Image>();
            var qualityUI = btn.GetComponentInChildren<ItemQualityUI>();
            var hover = btn.GetComponent<InventorySlotHover>();
            if (hover == null) hover = btn.gameObject.AddComponent<InventorySlotHover>();

            if (i < weaponList.Count && weaponList[i] != null)
            {
                var weapon = weaponList[i];
                iconImg.sprite = weapon.icon;
                iconImg.enabled = true;
                iconImg.preserveAspect = true;

                if (qualityUI != null)
                {
                    qualityUI.itemData = weapon;
                    qualityUI.UpdateQualityFrame();
                }

                btn.onClick.RemoveAllListeners();
                int weaponIndex = i;
                btn.onClick.AddListener(() => OnClickWeapon(weaponList[weaponIndex]));

                // **Gắn mô tả vào hover**
                hover.Setup(weapon, tooltipUI);
            }
            else
            {
                iconImg.sprite = null;
                iconImg.enabled = false;
                if (qualityUI != null)
                {
                    qualityUI.itemData = null;
                    qualityUI.qualityFrameImage.enabled = false;
                }
                btn.onClick.RemoveAllListeners();

                // Slot trống truyền null như cũ:
                hover.Setup(null, tooltipUI);
            }
        }
    }




    void OnClickWeapon(WeaponData weapon)
    {
        Debug.Log("Đã chọn vũ khí: " + weapon.itemName);
        // TODO: Gọi panel detail hoặc trang bị vũ khí vào nhân vật
    }
}