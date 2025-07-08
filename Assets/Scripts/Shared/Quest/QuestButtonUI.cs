using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuestListButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Image selectedHighlight;
    public Image hoverHighlight;
    public GameObject questDetailPanel; // Panel hiện chi tiết quest này
    public Image completedTick; // Kéo icon tick vào Inspector

    private bool isHovering = false;
    private bool isSelected = false;
    private QuestListUIManager manager;

    void Awake()
    {
        manager = GetComponentInParent<QuestListUIManager>();
    }
    public void SetCompleted(bool completed)
    {
        if (completedTick != null)
            completedTick.gameObject.SetActive(completed);
    }
    public void SetSelected(bool selected)
    {
        isSelected = selected;
        UpdateState();
        if (questDetailPanel != null)
            questDetailPanel.SetActive(selected);
    }

    void UpdateState()
    {
        if (selectedHighlight != null)
            selectedHighlight.enabled = isSelected;
        if (hoverHighlight != null)
            hoverHighlight.enabled = isHovering || isSelected;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
        UpdateState();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        UpdateState();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (manager != null)
            manager.SelectButton(this);
    }
}
