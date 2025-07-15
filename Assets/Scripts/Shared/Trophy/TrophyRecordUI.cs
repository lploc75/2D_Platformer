using UnityEngine;
using TMPro;

public class TrophyRecordUI : MonoBehaviour
{
    [Header("UI TMP_Text References")]
    public TMP_Text totalTimeText;
    public TMP_Text totalKillText;
    public TMP_Text totalDeathText;
    public TMP_Text totalGoldText;

    // Variables to hold record data
    private float totalPlayTime; // In seconds
    private int totalKill;
    private int totalDeath;
    private int totalGold;

    void Start()
    {
        LoadRecord();
        UpdateUI();
    }

    // Load data from PlayerPrefs (or from your own save file system)
    public void LoadRecord()
    {
        totalPlayTime = PlayerPrefs.GetFloat("TotalPlayTime", 0f); // seconds
        totalKill = PlayerPrefs.GetInt("TotalKill", 0);
        totalDeath = PlayerPrefs.GetInt("TotalDeath", 0);
        totalGold = PlayerPrefs.GetInt("TotalGold", 0);
    }

    // Update the UI display
    void UpdateUI()
    {
        int hours = Mathf.FloorToInt(totalPlayTime / 3600f);
        int minutes = Mathf.FloorToInt((totalPlayTime % 3600) / 60f);
        int seconds = Mathf.FloorToInt(totalPlayTime % 60);

        totalTimeText.text = $"Total Play Time: <b>{hours}h {minutes}m {seconds}s</b>";
        totalKillText.text = $"Monsters Defeated: <b>{totalKill}</b>";
        totalDeathText.text = $"Total Deaths: <b>{totalDeath}</b>";
        totalGoldText.text = $"Total Gold Earned: <b>{totalGold}</b>";
    }

    // Use this method to update the record and refresh the UI if you need to
    public void SetRecord(float playTime, int kill, int death, int gold)
    {
        totalPlayTime = playTime;
        totalKill = kill;
        totalDeath = death;
        totalGold = gold;
        UpdateUI();
    }
}
