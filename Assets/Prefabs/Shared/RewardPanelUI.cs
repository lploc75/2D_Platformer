using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class RewardPanelUI : MonoBehaviour
{
    public GameObject rewardIconPrefab;
    public Transform rewardListContainer;
    public InventoryTooltipUI tooltipUI;
    public float autoHideDelay = 2.0f;
    public GameObject closeButton;

    Coroutine autoHideCoroutine;

    void Awake()
    {
        gameObject.SetActive(false); // Ẩn panel khi bắt đầu

        if (closeButton != null)
        {
            var btn = closeButton.GetComponent<Button>();
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(HidePanel); // Gán đúng 1 lần duy nhất
        }
    }

    public void ShowRewards(List<(ItemData, int)> rewards)
    {
        Debug.Log("ShowRewards được gọi! activeSelf=" + gameObject.activeSelf);

        foreach (Transform child in rewardListContainer)
            Destroy(child.gameObject);

        foreach (var reward in rewards)
        {
            GameObject go = Instantiate(rewardIconPrefab, rewardListContainer);
            RewardIconUI ui = go.GetComponent<RewardIconUI>();
            ui.Setup(reward.Item1, reward.Item2);

            var hover = go.GetComponent<InventorySlotHover>();
            if (hover == null) hover = go.AddComponent<InventorySlotHover>();
            hover.Setup(reward.Item1, tooltipUI);
        }

        gameObject.SetActive(true);

        if (autoHideCoroutine != null)
            StopCoroutine(autoHideCoroutine);
        autoHideCoroutine = StartCoroutine(AutoHidePanel());
    }

    IEnumerator AutoHidePanel()
    {
        yield return new WaitForSeconds(autoHideDelay);
        HidePanel();
    }

    public void HidePanel()
    {
        if (autoHideCoroutine != null)
        {
            StopCoroutine(autoHideCoroutine);
            autoHideCoroutine = null;
        }
        gameObject.SetActive(false);

        // --- Clear selection của EventSystem! ---
        if (EventSystem.current != null)
            EventSystem.current.SetSelectedGameObject(null);
    }
}
