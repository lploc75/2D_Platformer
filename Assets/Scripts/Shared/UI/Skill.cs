using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Skill
{
    public string skillName;
    public Image icon;
    public Button upgradeButton;
    public List<int> prerequisiteSkillIndices; // Dùng index thay cho reference
    public TextMeshProUGUI soulText;
    public static int RequiredSouls => 20;

    [HideInInspector] public bool isUnlocked = false;

    // Không cần CanUpgrade trong class Skill, chuyển hết sang SkillTreeManager
}
