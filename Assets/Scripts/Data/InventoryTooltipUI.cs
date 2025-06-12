using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryTooltipUI : MonoBehaviour
{
    public GameObject tooltipPanel;        // Kéo TooltipPanel vào
    public TextMeshProUGUI descriptionText;    // Kéo Text vào

    public void ShowTooltip(string desc, Vector2 pos)
    {
        Debug.Log("ShowTooltip: " + desc); // Xem desc có đúng không
        descriptionText.text = desc;
        tooltipPanel.SetActive(true);
        Vector2 offset = new Vector2(20, -20); // lệch sang phải, xuống dưới 1 chút
        tooltipPanel.transform.position = pos + offset;
    }


    public void HideTooltip()
    {
        tooltipPanel.SetActive(false);
    }
}
