using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryStaticUIController : MonoBehaviour
{
    public List<Button> slotButtons;          // Kéo lần lượt các Button vào danh sách này theo thứ tự
    public List<WeaponData> weaponList;       // Kéo các WeaponData asset vào đây

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

            // Lấy component ItemQualityUI trong slot (hoặc icon)
            var qualityUI = btn.GetComponentInChildren<ItemQualityUI>();

            if (i < weaponList.Count && weaponList[i] != null)
            {
                var weapon = weaponList[i];

                // Set icon
                iconImg.sprite = weapon.icon;
                iconImg.enabled = true;
                iconImg.preserveAspect = true;

                // Gán itemData cho ItemQualityUI
                if (qualityUI != null)
                {
                    qualityUI.itemData = weapon;
                    qualityUI.UpdateQualityFrame();  // Cập nhật khung phẩm chất
                }
                else
                {
                    Debug.LogWarning("Không tìm thấy ItemQualityUI trên slot " + i);
                }

                btn.onClick.RemoveAllListeners();
                int weaponIndex = i;
                btn.onClick.AddListener(() => OnClickWeapon(weaponList[weaponIndex]));
            }
            else
            {
                // Slot trống
                iconImg.sprite = null;
                iconImg.enabled = false;

                if (qualityUI != null)
                {
                    qualityUI.itemData = null;
                    qualityUI.qualityFrameImage.enabled = false;  // Ẩn khung phẩm chất
                }

                btn.onClick.RemoveAllListeners();
            }
        }
    }



    void OnClickWeapon(WeaponData weapon)
    {
        Debug.Log("Đã chọn vũ khí: " + weapon.itemName);
        // TODO: Gọi panel detail hoặc trang bị vũ khí vào nhân vật
    }
}
