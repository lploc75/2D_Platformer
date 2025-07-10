using UnityEngine;

using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public static HealthUI Instance;
    public Image[] skillSlots; // Kéo các slot Image trên hotbar vào đây (Inspector)
    private int[] skillIds; // Lưu lại ID của các skill đã add (tùy chọn)

    void Awake()
    {
        Instance = this;
        skillIds = new int[skillSlots.Length];
        for (int i = 0; i < skillIds.Length; i++)
        {
            skillIds[i] = -1;
            // Đặt màu trong suốt cho tất cả các slot lúc khởi động
            skillSlots[i].color = new Color(1f, 1f, 1f, 0f);
            skillSlots[i].sprite = null; // nếu muốn slot hoàn toàn trắng trơn
        }
    }


    public void AddSkillToSlot(int skillId, Sprite icon)
    {
        // Kiểm tra trùng skill (không add trùng)
        for (int i = 0; i < skillSlots.Length; i++)
            if (skillIds[i] == skillId) return;

        // Tìm slot trống để add
        for (int i = 0; i < skillSlots.Length; i++)
        {
            if (skillSlots[i].sprite == null)
            {
                skillSlots[i].sprite = icon;
                skillSlots[i].color = new Color(1f, 1f, 1f, 1f); // hiện màu trắng
                skillIds[i] = skillId;
                break;
            }
        }
    }
}

