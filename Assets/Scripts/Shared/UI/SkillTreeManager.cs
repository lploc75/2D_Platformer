using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class SkillTreeManager : MonoBehaviour
{
    public static SkillTreeManager Instance { get; private set; }

    public List<Skill> skills;
    int playerSouls => CurrencyManager.Instance.GetCurrency(CurrencyType.PurpleSoul);

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        for (int i = 0; i < skills.Count; i++)
        {
            int idx = i;
            skills[i].upgradeButton.onClick.AddListener(() => UpgradeSkill(idx));
        }
        UpdateUI();
    }
    public void UpgradeSkill(int skillIndex)
    {
        if (!CanUpgrade(skillIndex)) return;

        CurrencyManager.Instance.RemoveCurrency(CurrencyType.PurpleSoul, Skill.RequiredSouls);
        skills[skillIndex].isUnlocked = true;
        skills[skillIndex].icon.color = Color.white;
        // Gọi lưu
        PlayerStatsFileHandler.Save(PlayerStatsManager.Instance);
        UpdateUI();
    }
    bool CanUpgrade(int skillIndex)
    {
        var skill = skills[skillIndex];
        if (skill.isUnlocked) return false;
        if (CurrencyManager.Instance.GetCurrency(CurrencyType.PurpleSoul) < Skill.RequiredSouls) return false;

        // OR logic cho prerequisite
        if (skill.prerequisiteSkillIndices == null || skill.prerequisiteSkillIndices.Count == 0)
            return true;
        foreach (var prereqIdx in skill.prerequisiteSkillIndices)
            if (skills[prereqIdx].isUnlocked) return true;
        return false;
    }
    public void UpdateUI()
    {
        int purpleSouls = CurrencyManager.Instance.GetCurrency(CurrencyType.PurpleSoul);

        for (int i = 0; i < skills.Count; i++)
        {
            var skill = skills[i];
            if (skill.isUnlocked)
                skill.soulText.text = "Opened";
            else
                skill.soulText.text = $"{purpleSouls}/{Skill.RequiredSouls}";

            skill.upgradeButton.interactable = CanUpgrade(i);
            skill.icon.color = skill.isUnlocked ? Color.white : Color.gray1;
        }
    }
    // Trong SkillTreeManager.cs
    public bool IsSkillUnlocked(int index)
    {
        if (index < 0 || index >= skills.Count)
            return false;

        return skills[index].isUnlocked;
    }

}
