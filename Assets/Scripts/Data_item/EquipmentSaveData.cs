using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class EquipmentSaveData
{
    public List<SlotEquipUIData> equipSlots;

    // Constructor để chuyển đổi từ các slot trang bị thành dữ liệu cần thiết
    public EquipmentSaveData(List<SlotEquipUIData> allSlots)
    {
        equipSlots = allSlots;
    }
}

[System.Serializable]
public class SlotEquipUIData
{
    public string itemName;   // Tên vật phẩm trang bị
    public string itemType;   // Loại vật phẩm (Vũ khí, Giáp, v.v.)
    public string iconPath;   // Đường dẫn của Sprite icon (Ví dụ: "Icons/Chaos_Wand")
    public string quality;    // Phẩm chất vật phẩm
    public string description; // Mô tả vật phẩm
    public float baseDamage;  // Sát thương cơ bản
    public float critDamage;  // Sát thương chí mạng
    public float critChance;  // Tỷ lệ chí mạng
    public float hp;          // Sức khỏe (HP)
    public float sp;          // Sức bền (SP)
    public float mp;          // Mana (MP)
    public float healthBonus; // Thêm HP từ giáp

    // Constructor để lấy thông tin của slot trang bị
    public SlotEquipUIData(SlotEquipUI slot)
    {
        if (slot.GetCurrentItem() != null)
        {
            itemName = slot.GetCurrentItem().itemName;
            itemType = slot.GetCurrentItem().itemType.ToString();  // Chuyển kiểu ItemType thành string
            iconPath = "Icons/" + slot.GetCurrentItem().icon.name;  // Lưu đường dẫn của Sprite icon
            quality = slot.GetCurrentItem().quality.ToString();  // Lưu phẩm chất


            description = slot.GetCurrentItem().description;  // Lưu mô tả vật phẩm

            // Kiểm tra nếu vật phẩm là Weapon hoặc Armor và lưu các giá trị liên quan
            if (slot.GetCurrentItem() is WeaponData weapon)
            {
                baseDamage = weapon.baseDamage;
                critDamage = weapon.critDamage;
                critChance = weapon.critChance;
                hp = weapon.hp;
                sp = weapon.sp;
                mp = weapon.mp;
                healthBonus = 0.0f;  // Giáp không có
            }
            else if (slot.GetCurrentItem() is ArmorData armor)
            {
                baseDamage = armor.GetBaseDamage();  // Lưu sát thương cơ bản từ giáp
                critDamage = armor.GetCritDamage();  // Lưu sát thương chí mạng từ giáp
                critChance = armor.GetCritChance();  // Lưu tỷ lệ chí mạng từ giáp
                sp = armor.GetSp();                 // Lưu stamina từ giáp
                mp = armor.GetMp();                 // Lưu mana từ giáp
                hp = 0.0f;                          // Vũ khí không có
                healthBonus = armor.GetHealthBonus();  // Lưu máu cộng thêm từ giáp
            }
        }
        else
        {
            itemName = "";
            itemType = "";
            iconPath = "";
            quality = "";
            description = "";
            baseDamage = 0.0f;
            critDamage = 0.0f;
            critChance = 0.0f;
            hp = 0.0f;
            sp = 0.0f;
            mp = 0.0f;
            healthBonus = 0.0f;
        }
    }
}





