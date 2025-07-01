using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryTooltipUI : MonoBehaviour
{
    [Header("Main Panel")]
    public GameObject tooltipPanel;
    public TextMeshProUGUI nameText, typeText, qualityText, descText;

    public void ShowTooltip(ItemData item, Vector2 pos)
    {
        if (item == null)
        {
            Debug.LogWarning("ShowTooltip: ItemData is null!");
            tooltipPanel?.SetActive(false);
            return;
        }

        if (nameText) nameText.text = item.itemName;
        if (typeText) typeText.text = item.itemType.ToString();
        if (qualityText) qualityText.text = item.quality.ToString();
        if (descText) descText.text = item.description;

        if (tooltipPanel)
        {
            tooltipPanel.SetActive(true);
            Vector2 offset = new Vector2(20, -20);
            tooltipPanel.transform.position = pos + offset;
        }
    }

    public void HideTooltip()
    {
        if (tooltipPanel) tooltipPanel.SetActive(false);
    }
}
