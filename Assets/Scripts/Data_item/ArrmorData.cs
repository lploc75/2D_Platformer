using UnityEngine;

[CreateAssetMenu(fileName = "NewArmor", menuName = "Inventory/Armor")]
public class ArmorData : ItemData
{

    [Tooltip("Tăng HP từ giáp")]
    public float healthBonus; // Tăng HP từ giáp

    [Tooltip("Tỷ lệ chí mạng (0-1, ví dụ: 0.2 = 20%)")]
    public float critChance; // Tỷ lệ chí mạng

    // Tính toán tỷ lệ chí mạng với hệ số phẩm chất
    public float GetCritChance()
    {
        return critChance * GetQualityMultiplier();
    }

    // Tính toán bonus HP với hệ số phẩm chất
    public float GetHealthBonus()
    {
        return healthBonus * GetQualityMultiplier();
    }

    // Hệ số phẩm chất cho giáp (với các phẩm chất khác nhau)
    private float GetQualityMultiplier()
    {
        switch (quality)
        {
            case ItemQuality.Common: return 1.0f;       // Common: hệ số 1.0
            case ItemQuality.Rare: return 1.4f;         // Rare: hệ số 1.4
            case ItemQuality.Epic: return 1.8f;         // Epic: hệ số 1.8
            case ItemQuality.Legendary: return 2.5f;    // Legendary: hệ số 2.5
            default: return 1.0f;
        }
    }
}
