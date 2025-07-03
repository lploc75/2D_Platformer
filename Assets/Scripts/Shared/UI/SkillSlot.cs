using UnityEngine;

using UnityEngine.UI;

public class SkillSlot : MonoBehaviour
{
    public int skillId;
    public Sprite skillIcon;
    public Button openButton; // K�o Button t? Inspector

    private void Start()
    {
        openButton.onClick.AddListener(OnOpenSkill);
    }

    public void OnOpenSkill()
    {
        // Khi b?m m? skill, g?i sang HealthUI ?? add skill n�y v�o slot tr?ng
        HealthUI.Instance.AddSkillToSlot(skillId, skillIcon);
    }
}

