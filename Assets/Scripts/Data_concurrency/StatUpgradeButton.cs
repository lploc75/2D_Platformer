using UnityEngine;
using UnityEngine.EventSystems;

public class StatUpgradeButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public enum StatType { STR, INT, DUR, PER, VIT }
    public StatType statType;
    public UpgradeTooltip tooltip;

    public PlayerStatsManager statsManager; // Assign in Inspector

    public void OnPointerEnter(PointerEventData eventData)
    {
        int level = 0;
        switch (statType)
        {
            case StatType.STR: level = statsManager.strLevel; break;
            case StatType.INT: level = statsManager.intLevel; break;
            case StatType.DUR: level = statsManager.durLevel; break;
            case StatType.PER: level = statsManager.perLevel; break;
            case StatType.VIT: level = statsManager.vitLevel; break;
        }
        Debug.Log($"{statType} hover, level = {level}"); // <-- Thêm dòng này

        int need = statsManager.GetPurpleSoulNeededForLevel(level);
        int purpleSoul = CurrencyManager.Instance.GetCurrency(CurrencyType.PurpleSoul);

        string msg;
        if (level >= 15)
            msg = "Maximum level reached!";
        else if (purpleSoul < need)
            msg = $"You need {need} Purple Soul(s) to upgrade to level {level + 1}.";
        else
            msg = $"Upgrade costs {need} Purple Soul(s).\nYou have enough to upgrade.";

        tooltip.ShowTooltip(msg);
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.HideTooltip();
    }
}
