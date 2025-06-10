using UnityEngine;

public class QuestListUIManager : MonoBehaviour
{
    private QuestListButton currentSelected;

    public void SelectButton(QuestListButton button)
    {
        if (currentSelected != null)
            currentSelected.SetSelected(false);

        currentSelected = button;
        if (currentSelected != null)
            currentSelected.SetSelected(true);
    }

    // Tự động chọn quest đầu tiên khi tạo mới UI (nếu muốn)
    public void SelectFirstButton()
    {
        var btn = GetComponentInChildren<QuestListButton>();
        if (btn != null)
            SelectButton(btn);
    }
}
