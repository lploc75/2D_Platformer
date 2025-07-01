using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class SkillTreeManager : MonoBehaviour
{
    public List<Skill> skills;
    public int playerSouls = 200;

    



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

        playerSouls -= Skill.RequiredSouls;
        skills[skillIndex].isUnlocked = true;
        skills[skillIndex].icon.color = Color.white;
        UpdateUI();
    }

    bool CanUpgrade(int skillIndex)
    {
        var skill = skills[skillIndex];
        if (skill.isUnlocked) return false;
        if (playerSouls < Skill.RequiredSouls) return false;

        // OR logic cho prerequisite
        if (skill.prerequisiteSkillIndices == null || skill.prerequisiteSkillIndices.Count == 0)
            return true;
        foreach (var prereqIdx in skill.prerequisiteSkillIndices)
            if (skills[prereqIdx].isUnlocked) return true;
        return false;
    }


    void UpdateUI()
    {
        for (int i = 0; i < skills.Count; i++)
        {
            var skill = skills[i];
            if (skill.isUnlocked)
                skill.soulText.text = "Opened";
            else
                skill.soulText.text = $"{playerSouls}/{Skill.RequiredSouls}";

            skill.upgradeButton.interactable = CanUpgrade(i);
            skill.icon.color = skill.isUnlocked ? Color.white : Color.gray1;
        }
    }

    public void AddSouls(int amount)
    {
        playerSouls += amount;
        UpdateUI();
    }
}
