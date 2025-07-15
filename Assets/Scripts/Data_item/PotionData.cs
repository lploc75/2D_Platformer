using UnityEngine;

[CreateAssetMenu(fileName = "NewPotion", menuName = "Inventory/Potion")]
public class PotionData : ItemData
{
    [Tooltip("Hi?u qu? h?i ph?c c?a l? thu?c")]
    public int restoreAmount;

    [Tooltip("Mô t? hi?u qu? c?a l? thu?c")]
    public string effect;

}