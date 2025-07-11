using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryTooltipUI : MonoBehaviour
{
    [Header("Main Panel")]
    public GameObject tooltipPanel;
    public TextMeshProUGUI nameText, typeText, qualityText, descText;

    [Header("Stat & Rarity/Amount Titles")]
    public GameObject statTitle;           // Stat title ở trên
    public GameObject rarityObj;           // Rarity obj ở trên (chứa qualityText)
    public TextMeshProUGUI amountTitleText; // Amount title ở dưới (Text TMP)

    [Header("Weapon Panel")]
    public GameObject weaponPanel;
    public GameObject baseDamageRow, critChanceRow, critDamageRow;
    public TextMeshProUGUI baseDamageValue, critChanceValue, critDamageValue;

    [Header("Armor Panel")]
    public GameObject armorPanel;
    public GameObject healthBonusRow, armorCritChanceRow;
    public TextMeshProUGUI healthBonusValue, armorCritChanceValue;

    [Header("Currency Panel")]
    public GameObject currencyPanel;
    public TextMeshProUGUI currencyNameText, currencyDescText, currencyAmountText, currencyTypeText;

    [Header("Tooltip Offset")]
    public Vector2 tooltipOffset = new Vector2(12, 8);
    public float tooltipHideDelay = 0.1f;

    // Internal state
    private bool pointerOnSlot = false;
    private bool pointerOnTooltip = false;
    private float hideTooltipTimer = -1f;

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

    public void OnTooltipPointerEnter() => pointerOnTooltip = true;

    public void OnTooltipPointerExit()
    {
        pointerOnTooltip = false;
        TryHideTooltip();
    }

    private void TryHideTooltip()
    {
        if (!pointerOnSlot && !pointerOnTooltip)
            hideTooltipTimer = tooltipHideDelay;
    }

    public void HideTooltipImmediate()
    {
        tooltipPanel?.SetActive(false);
        pointerOnSlot = false;
        pointerOnTooltip = false;
        hideTooltipTimer = -1f;
    }

    public void ShowTooltip(ItemData item, Vector2 pos)
    {

        if (item == null || tooltipPanel == null)
        {
            tooltipPanel?.SetActive(false);
            return;
        }

        tooltipPanel.SetActive(true);

        // Mặc định hiện lại các thành phần
        statTitle?.SetActive(true);
        rarityObj?.SetActive(true);
        amountTitleText?.gameObject.SetActive(false);

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

        weaponPanel?.SetActive(false);
        armorPanel?.SetActive(false);
        currencyPanel?.SetActive(false);

        // Chỉ bật đúng 1 panel, đồng thời ẩn/hiện các trường đặc biệt theo yêu cầu
        if (item is WeaponData weapon)
        {
            SetupWeaponTooltip(weapon);
        }
        else if (item is ArmorData armor)
        {
            SetupArmorTooltip(armor);
        }
        else if (item is CurrencyData currency)
        {
            SetupCurrencyTooltip(currency);

            // Ẩn stat và rarity trên, hiện amount title dưới
            statTitle?.SetActive(false);
            rarityObj?.SetActive(false);
            if (amountTitleText)
            {
                amountTitleText.gameObject.SetActive(true);
                amountTitleText.text = "Amount:"; // Tùy bạn set
            }

            // Thay qualityText trên thành amount value
            if (qualityText) qualityText.text = currency.amount.ToString();
        }

        Canvas canvas = tooltipPanel.GetComponentInParent<Canvas>();
        RectTransform tooltipRect = tooltipPanel.GetComponent<RectTransform>();
        if (canvas && tooltipRect)
            PositionTooltip(canvas, tooltipRect, pos);
    }

    private void SetupWeaponTooltip(WeaponData weapon)
    {
        if (!weaponPanel || weapon == null) return;

        weaponPanel.SetActive(true);

        SetRowActive(baseDamageRow, baseDamageValue, weapon.baseDamage != 0, weapon.baseDamage.ToString());
        SetRowActive(critChanceRow, critChanceValue, weapon.critChance != 0f, (weapon.critChance * 100).ToString("F0") + "%");
        SetRowActive(critDamageRow, critDamageValue, weapon.critDamage != 0f, weapon.critDamage.ToString("F1") + "x");
    }

    private void SetupArmorTooltip(ArmorData armor)
    {
        if (!armorPanel || armor == null) return;

        armorPanel.SetActive(true);

        SetRowActive(healthBonusRow, healthBonusValue, armor.healthBonus != 0f, armor.healthBonus.ToString("F0"));
        SetRowActive(armorCritChanceRow, armorCritChanceValue, armor.critChance != 0f, (armor.critChance * 100).ToString("F0") + "%");
    }

    private void SetupCurrencyTooltip(CurrencyData currency)
    {
        Debug.Log($"CurrencyManager.Instance.GetCurrency({currency.currencyType}) = {currency.amount}");

        if (!currencyPanel || currency == null) return;
        currencyPanel.SetActive(true);

        if (currencyNameText) currencyNameText.text = currency.itemName;
        if (currencyDescText) currencyDescText.text = currency.description;
        if (currencyAmountText) currencyAmountText.text = currency.amount.ToString();
        if (currencyTypeText) currencyTypeText.text = currency.currencyType.ToString();
    }

    private void SetRowActive(GameObject row, TextMeshProUGUI valueText, bool condition, string value)
    {
        if (row) row.SetActive(condition);
        if (valueText) valueText.text = condition ? value : "";
    }

    private void PositionTooltip(Canvas canvas, RectTransform tooltipRect, Vector2 pos)
    {
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        LayoutRebuilder.ForceRebuildLayoutImmediate(tooltipPanel.GetComponent<RectTransform>());

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

    private Color GetQualityColor(ItemQuality quality)
    {
        return quality switch
        {
            ItemQuality.Legendary => new Color(0.7f, 0.4f, 1.0f),
            ItemQuality.Epic => new Color(0.15f, 0.55f, 1.0f),
            ItemQuality.Rare => new Color(0.1f, 0.85f, 0.2f),
            _ => Color.white
        };
    }
}
