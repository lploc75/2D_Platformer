using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Inventory/Weapon")]
public class WeaponData : ItemData
{
    [Tooltip("Sát thương cơ bản của vũ khí")]
    public int baseDamage;

    [Tooltip("Sát thương chí mạng (hệ số, ví dụ: 2.0 = x2 dame)")]
    public float critDamage;

    [Tooltip("Tỷ lệ chí mạng (0-1, ví dụ: 0.2 = 20%)")]
    public float critChance;

    [Tooltip("Bonus HP từ vũ khí")]
    public float hp;

    [Tooltip("Bonus SP (Stamina) từ vũ khí")]
    public float sp;

    [Tooltip("Bonus MP (Mana) từ vũ khí")]
    public float mp;

    //public int GetAttackPower()
    //{
    //    return Mathf.RoundToInt(baseDamage * GetQualityMultiplier());
    //}

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
