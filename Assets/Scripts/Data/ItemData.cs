using UnityEngine;

public enum ItemType { Weapon, Armor, Potion, Misc }
public enum ItemQuality { Common, Rare, Epic, Legendary }

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public ItemType itemType;
    public ItemQuality quality;
    public Sprite icon;
    public string description;
}
