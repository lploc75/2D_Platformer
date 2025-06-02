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

            if (i < weaponList.Count && weaponList[i] != null)
            {
                iconImg.sprite = weaponList[i].icon;
                iconImg.enabled = true;
                iconImg.preserveAspect = true;

                btn.onClick.RemoveAllListeners();
                int weaponIndex = i;
                btn.onClick.AddListener(() => OnClickWeapon(weaponList[weaponIndex]));
            }
            else
            {
                iconImg.sprite = null;
                iconImg.enabled = false;
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
