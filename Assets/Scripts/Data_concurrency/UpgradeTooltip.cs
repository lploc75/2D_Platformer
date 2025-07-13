using UnityEngine;
using TMPro;

public class UpgradeTooltip : MonoBehaviour
{
    public GameObject tooltipPanel;
    public TMP_Text tooltipText;

    public void ShowTooltip(string message)
    {
        tooltipText.text = message;
        Debug.Log("Tooltip show: " + message); // Xem có log ra không
        tooltipPanel.SetActive(true);
    }


    public void HideTooltip()
    {
        tooltipPanel.SetActive(false);
    }
}
