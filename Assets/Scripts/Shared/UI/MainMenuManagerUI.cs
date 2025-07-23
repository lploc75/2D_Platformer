using UnityEngine;
using System.Collections;

public class MainMenuManager : MonoBehaviour
{
    [Header("Panel References")]
    public GameObject mainMenuPanel;            // <-- PANEL CHA
    public GameObject inventoryContentPanel;
    public GameObject skillContentPanel;
    public GameObject statsContentPanel;
    public GameObject questContentPanel;
    public GameObject trophyContentPanel;

    void Start()
    {
        StartCoroutine(DeactivatePanelsNextFrame());
    }

    IEnumerator DeactivatePanelsNextFrame()
    {
        yield return null; // Đợi xong 1 frame

        if (inventoryContentPanel != null) inventoryContentPanel.SetActive(false);
        if (skillContentPanel != null) skillContentPanel.SetActive(false);
        if (statsContentPanel != null) statsContentPanel.SetActive(false);
        if (questContentPanel != null) questContentPanel.SetActive(false);
        if (trophyContentPanel != null) trophyContentPanel.SetActive(false);

        // Tắt panel cha cuối cùng, SAU khi đã đảm bảo mọi script con đều Awake/Start xong
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
    }

    void Update()
    {
        // Toggle main menu panel bằng phím M (ví dụ)
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (mainMenuPanel != null)
                mainMenuPanel.SetActive(!mainMenuPanel.activeSelf);
        }

        // Toggle Inventory bằng phím I
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (inventoryContentPanel != null)
            {
                bool isActive = inventoryContentPanel.activeSelf;
                inventoryContentPanel.SetActive(!isActive);

                // Nếu InventoryPanel vừa bị tắt, ẩn luôn tooltip (nếu có)
                var tooltip = inventoryContentPanel.GetComponentInChildren<InventoryTooltipUI>();
                if (isActive && tooltip != null)
                {
                    tooltip.HideTooltipImmediate();
                }
            }
        }
    }
}
