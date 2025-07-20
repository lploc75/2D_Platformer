using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class QuestListUIManager : MonoBehaviour
{
    private QuestListButton currentSelected;
    public VerticalLayoutGroup layoutGroup;  // Kéo VerticalLayoutGroup này vào Inspector

    // Các spacing cho từng quest (quest 3 là index 0, quest 4 là index 1, ...)
    private readonly List<float> customSpacings = new List<float> { -191.6f, -144.2f, -87.4f, -41.4f };

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

    /// <summary>
    /// Điều chỉnh spacing theo số lượng object con là QuestListButton
    /// </summary>
    public void AdjustSpacingByChildren()
    {
        if (layoutGroup == null)
            return;

        int count = 0;
        foreach (Transform child in transform)
        {
            if (child.GetComponent<QuestListButton>() != null)
                count++;
        }

        if (count < 3)
        {
            layoutGroup.spacing = -240f;
        }
        else
        {
            // Quest 3 (count=3) dùng index 0, Quest 4 (count=4) dùng index 1, v.v.
            int spacingIdx = Mathf.Clamp(count - 3, 0, customSpacings.Count - 1);
            layoutGroup.spacing = customSpacings[spacingIdx];
        }
    }

    // Gợi ý: Gọi hàm này mỗi khi cập nhật số lượng button quest (ví dụ sau khi tạo hoặc xóa quest)
    private void OnEnable()
    {
        AdjustSpacingByChildren();
    }
}
