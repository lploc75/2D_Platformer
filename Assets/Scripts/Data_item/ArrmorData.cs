using UnityEngine;

[CreateAssetMenu(fileName = "NewArmor", menuName = "Inventory/Armor")]
public class ArmorData : ItemData
{
    [Tooltip("Máu cộng thêm từ giáp")]
    public float healthBonus;

    [Tooltip("Sát thương cơ bản từ giáp (nếu có)")]
    public int baseDamage;

    [Tooltip("Sát thương chí mạng thêm (hệ số, ví dụ: 0.2 = +20%)")]
    public float critDamage;

    [Tooltip("Tỷ lệ chí mạng thêm (0.1 = 10%)")]
    public float critChance;

    [Tooltip("Stamina cộng thêm")]
    public float sp;

    [Tooltip("Mana cộng thêm")]
    public float mp;

    // Các hàm Get trả về đúng giá trị gốc, không nhân hệ số phẩm chất nữa
    public float GetHealthBonus()
    {
        return healthBonus;
    }

    public int GetBaseDamage()
    {
        return baseDamage;
    }

    public float GetCritDamage()
    {
        return critDamage;
    }

    public float GetCritChance()
    {
        return critChance;
    }

    public float GetSp()
    {
        return sp;
    }

    public float GetMp()
    {
        return mp;
    }

    //// Xóa hệ số phẩm chất vì không dùng nữa
    //private float GetQualityMultiplier()
    //{
    //    switch (quality)
    //    {
    //        case ItemQuality.Common: return 1.0f;
    //        case ItemQuality.Rare: return 1.4f;
    //        case ItemQuality.Epic: return 1.8f;
    //        case ItemQuality.Legendary: return 2.5f;
    //        default: return 1.0f;
    //    }
    //}
    }
