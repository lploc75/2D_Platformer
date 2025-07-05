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

    [Header("Tooltip Offset")]
    public Vector2 tooltipOffset = new Vector2(12, 8);
    public float tooltipHideDelay = 0.1f;

    // --- Internal state ---
    private bool pointerOnSlot = false;
    private bool pointerOnTooltip = false;
    private float hideTooltipTimer = -1f;

    // --- Update function dùng cho timer ---
    void Update()
    {
        if (hideTooltipTimer >= 0f)
        {
            hideTooltipTimer -= Time.unscaledDeltaTime;
            if (hideTooltipTimer <= 0f)
            {
                if (!pointerOnSlot && !pointerOnTooltip)
                    tooltipPanel.SetActive(false);
                hideTooltipTimer = -1f;
            }
        }
    }

    public void OnSlotPointerEnter(ItemData item, Vector2 pos)
    {
        pointerOnSlot = true;
        ShowTooltip(item, pos);
        hideTooltipTimer = -1f;
    }
    public void OnSlotPointerExit()
    {
        pointerOnSlot = false;
        TryHideTooltip();
    }
    public void OnTooltipPointerEnter() { pointerOnTooltip = true; }
    public void OnTooltipPointerExit() { pointerOnTooltip = false; TryHideTooltip(); }

    private void TryHideTooltip()
    {
        if (!pointerOnSlot && !pointerOnTooltip)
            hideTooltipTimer = tooltipHideDelay; // Bắt đầu đếm lùi
    }

    private Color GetQualityColor(ItemQuality quality)
    {
        switch (quality)
        {
            case ItemQuality.Legendary: return new Color(0.7f, 0.4f, 1.0f);
            case ItemQuality.Epic: return new Color(0.15f, 0.55f, 1.0f);
            case ItemQuality.Rare: return new Color(0.1f, 0.85f, 0.2f);
            default: return Color.white;
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

        if (weaponPanel) weaponPanel.SetActive(false);
        if (armorPanel) armorPanel.SetActive(false);

        if (item.itemType == ItemType.Weapon)
        {
            WeaponData weapon = item as WeaponData;
            if (weaponPanel && weapon != null)
            {
                weaponPanel.SetActive(true);
                if (baseDamageRow && baseDamageValue)
                {
                    bool show = weapon.baseDamage != 0;
                    baseDamageRow.SetActive(show);
                    baseDamageValue.text = show ? weapon.baseDamage.ToString() : "";
                }
                if (critChanceRow && critChanceValue)
                {
                    bool show = weapon.critChance != 0f;
                    critChanceRow.SetActive(show);
                    critChanceValue.text = show ? (weapon.critChance * 100).ToString("F0") + "%" : "";
                }
                if (critDamageRow && critDamageValue)
                {
                    bool show = weapon.critDamage != 0f;
                    critDamageRow.SetActive(show);
                    critDamageValue.text = show ? weapon.critDamage.ToString("F1") + "x" : "";
                }
            }
        }
        else if (item.itemType == ItemType.Armor)
        {
            ArmorData armor = item as ArmorData;
            if (armorPanel && armor != null)
            {
                armorPanel.SetActive(true);
                if (healthBonusRow && healthBonusValue)
                {
                    bool show = armor.healthBonus != 0f;
                    healthBonusRow.SetActive(show);
                    healthBonusValue.text = show ? armor.healthBonus.ToString("F0") : "";
                }
                if (armorCritChanceRow && armorCritChanceValue)
                {
                    bool show = armor.critChance != 0f;
                    armorCritChanceRow.SetActive(show);
                    armorCritChanceValue.text = show ? (armor.critChance * 100).ToString("F0") + "%" : "";
                }
            }
        }

        // Vị trí tooltip (trên canvas riêng)
        Canvas canvas = tooltipPanel.GetComponentInParent<Canvas>();
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        RectTransform tooltipRect = tooltipPanel.GetComponent<RectTransform>();

        float tooltipHeight = tooltipRect.rect.height;
        float tooltipWidth = tooltipRect.rect.width;

        Vector2 offset = tooltipOffset;
        Vector2 anchoredPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            pos + offset,
            canvas.renderMode == RenderMode.ScreenSpaceCamera ? canvas.worldCamera : null,
            out anchoredPos
        );

        float minX = -canvasRect.rect.width / 2f;
        float maxX = canvasRect.rect.width / 2f - tooltipWidth;
        float minY = -canvasRect.rect.height / 2f;
        float maxY = canvasRect.rect.height / 2f - tooltipHeight;

        anchoredPos.x = Mathf.Clamp(anchoredPos.x, minX, maxX);
        anchoredPos.y = Mathf.Clamp(anchoredPos.y, minY, maxY);

        tooltipRect.anchoredPosition = anchoredPos;
    }
    public void HideTooltipImmediate()
    {
        tooltipPanel.SetActive(false);
        pointerOnSlot = false;
        pointerOnTooltip = false;
        hideTooltipTimer = -1f;
    }

}
