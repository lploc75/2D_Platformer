using UnityEngine;
using TMPro;
using System.Collections;

public class TrophyUIMap : MonoBehaviour
{
    public TMP_Text totalTimeText;
    public TMP_Text totalKillText;
    public TMP_Text totalDeathText;
    public TMP_Text totalGoldText;

    IEnumerator Start()
    {
        yield return null;
        if (TrophyRecordUI.Instance != null)
        {
            TrophyRecordUI.Instance.totalTimeText = totalTimeText;
            TrophyRecordUI.Instance.totalKillText = totalKillText;
            TrophyRecordUI.Instance.totalDeathText = totalDeathText;
            TrophyRecordUI.Instance.totalGoldText = totalGoldText;
            TrophyRecordUI.Instance.UpdateUI(); // <-- Chỉ gọi tại đây!
        }
    }

}
