using UnityEngine;

public enum ItemType
{
    Weapon,   // Vũ khí
    Armor,    // Áo giáp
    Potion,    // Bình thuốc
    Currency
}

public enum ItemQuality
{
    Common,
    Rare,
    Epic,
    Legendary
}

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public ItemType itemType;
    public ItemQuality quality;
    public Sprite icon;
    public string description;

    // Thêm trường này để lưu loại tiền nếu là ItemType.Currency
    public CurrencyType currencyType;
}
