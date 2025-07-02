using UnityEngine;

[CreateAssetMenu(fileName = "NewArmor", menuName = "Inventory/Armor")]
public class ArmorData : ItemData
{
    [Tooltip("Tăng HP từ giáp")]
    public float healthBonus;

    [Tooltip("Sát thương cơ bản")]
    public int baseDamage;

    [Tooltip("Sát thương chí mạng (hệ số, ví dụ: 2.0 = x2 dame)")]
    public float critDamage;

    [Tooltip("Tỷ lệ chí mạng (0-1, ví dụ: 0.2 = 20%)")]
    public float critChance;

    [Tooltip("Bonus SP (Stamina) từ giáp")]
    public float sp;

    [Tooltip("Bonus MP (Mana) từ giáp")]
    public float mp;

    // --- Getter đã nhân hệ số phẩm chất ---
    public float GetHealthBonus()
    {
        return healthBonus * GetQualityMultiplier();
    }
    public int GetBaseDamage()
    {
        return Mathf.RoundToInt(baseDamage * GetQualityMultiplier());
    }
    public float GetCritDamage()
    {
        return critDamage * GetQualityMultiplier();
    }
    public float GetCritChance()
    {
        return critChance * GetQualityMultiplier();
    }
    public float GetSp()
    {
        return sp * GetQualityMultiplier();
    }
    public float GetMp()
    {
        return mp * GetQualityMultiplier();
    }

    // --- Hệ số phẩm chất ---
    private float GetQualityMultiplier()
    {
        switch (quality)
        {
            case ItemQuality.Common: return 1.0f;
            case ItemQuality.Rare: return 1.4f;
            case ItemQuality.Epic: return 1.8f;
            case ItemQuality.Legendary: return 2.5f;
            default: return 1.0f;
        }
    }
}
