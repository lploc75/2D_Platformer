using UnityEngine;
using TMPro;

public class InventoryTooltipUI : MonoBehaviour
{
    [Header("Main Panel")]
    public GameObject tooltipPanel;
    public TextMeshProUGUI nameText, typeText, qualityText, descText;

    [Header("Weapon Panel")]
    public GameObject weaponPanel;
    public GameObject baseDamageRow, critChanceRow, critDamageRow;
    public TextMeshProUGUI baseDamageValue, critChanceValue, critDamageValue;

    [Header("Armor Panel")]
    public GameObject armorPanel;
    public GameObject healthBonusRow, armorCritChanceRow;
    public TextMeshProUGUI healthBonusValue, armorCritChanceValue;

    private Color GetQualityColor(ItemQuality quality)
    {
        switch (quality)
        {
            case ItemQuality.Legendary: return new Color(0.7f, 0.4f, 1.0f);    // tím (Legendary)
            case ItemQuality.Epic: return new Color(0.15f, 0.55f, 1.0f);   // xanh dương (Epic)
            case ItemQuality.Rare: return new Color(0.1f, 0.85f, 0.2f);    // xanh lá (Rare)
            default: return Color.white; // thường (Common)
        }
    }

    public void ShowTooltip(ItemData item, Vector2 pos)
    {
        if (item == null)
        {
            tooltipPanel?.SetActive(false);
            return;
        }

        tooltipPanel.SetActive(true);

        // --- Set các trường cơ bản ---
        if (nameText)
        {
            nameText.text = item.itemName;
            nameText.color = GetQualityColor(item.quality);
        }
        if (typeText) typeText.text = item.itemType.ToString();
        if (qualityText)
        {
            qualityText.text = item.quality.ToString();
            qualityText.color = GetQualityColor(item.quality);
        }
        if (descText) descText.text = item.description;

        // Ẩn panel con
        if (weaponPanel) weaponPanel.SetActive(false);
        if (armorPanel) armorPanel.SetActive(false);

        // --- Weapon ---
        if (item.itemType == ItemType.Weapon)
        {
            WeaponData weapon = item as WeaponData;
            if (weaponPanel && weapon != null)
            {
                weaponPanel.SetActive(true);

                // Base Damage
                if (baseDamageRow && baseDamageValue)
                {
                    bool show = weapon.baseDamage != 0;
                    baseDamageRow.SetActive(show);
                    if (show)
                        baseDamageValue.text = weapon.baseDamage.ToString();
                    else
                        baseDamageValue.text = ""; // clear text khi ẩn row
                }

                // Crit Chance
                if (critChanceRow && critChanceValue)
                {
                    bool show = weapon.critChance != 0f;
                    critChanceRow.SetActive(show);
                    if (show)
                        critChanceValue.text = (weapon.critChance * 100).ToString("F0") + "%";
                    else
                        critChanceValue.text = "";
                }

                // Crit Damage
                if (critDamageRow && critDamageValue)
                {
                    bool show = weapon.critDamage != 0f;
                    critDamageRow.SetActive(show);
                    if (show)
                        critDamageValue.text = weapon.critDamage.ToString("F1") + "x";
                    else
                        critDamageValue.text = "";
                }
            }
        }

        // --- Armor ---
        else if (item.itemType == ItemType.Armor)
        {
            ArmorData armor = item as ArmorData;
            if (armorPanel && armor != null)
            {
                armorPanel.SetActive(true);

                // Health Bonus
                if (healthBonusRow && healthBonusValue)
                {
                    bool show = armor.healthBonus != 0f;
                    healthBonusRow.SetActive(show);
                    if (show)
                        healthBonusValue.text = armor.healthBonus.ToString("F0");
                    else
                        healthBonusValue.text = "";
                }


                // Crit Chance
                if (armorCritChanceRow && armorCritChanceValue)
                {
                    bool show = armor.critChance != 0f;
                    armorCritChanceRow.SetActive(show);
                    if (show) armorCritChanceValue.text = (armor.critChance * 100).ToString("F0") + "%";
                }
            }
        }

        // --- Vị trí tooltip dưới chuột, clamp ---
        Canvas canvas = tooltipPanel.GetComponentInParent<Canvas>();
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        RectTransform tooltipRect = tooltipPanel.GetComponent<RectTransform>();

        float tooltipHeight = tooltipRect.rect.height;
        float tooltipWidth = tooltipRect.rect.width;

        Vector2 offset = new Vector2(0, -tooltipHeight - 10);

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect, pos + offset, canvas.renderMode == RenderMode.ScreenSpaceCamera ? canvas.worldCamera : null, out localPoint
        );

        float minX = -canvasRect.rect.width / 2f;
        float maxX = canvasRect.rect.width / 2f - tooltipWidth;
        float minY = -canvasRect.rect.height / 2f;
        float maxY = canvasRect.rect.height / 2f - tooltipHeight;

        localPoint.x = Mathf.Clamp(localPoint.x, minX, maxX);
        localPoint.y = Mathf.Clamp(localPoint.y, minY, maxY);

        tooltipRect.anchoredPosition = localPoint;
    }

    public void HideTooltip()
    {
        if (tooltipPanel) tooltipPanel.SetActive(false);
    }
}
