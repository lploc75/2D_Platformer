using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Inventory/Weapon")]
public class WeaponData : ItemData
{
    [Tooltip("Base attack power of the weapon")]
    public int baseAttack;

    [Tooltip("Attack speed or other stats")]
    public float attackSpeed;

    public int GetAttackPower()
    {
        return Mathf.RoundToInt(baseAttack * GetQualityMultiplier());
    }

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
