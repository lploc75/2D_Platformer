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

    private float startTime; // Time when the game starts
    private bool isGameActive = false; // Flag to track whether the game is active

    void Start()
    {
        Debug.Log("Start method is called");
        LoadRecord(); // Load record data when the game starts
        UpdateUI(); // Update UI to display loaded data

        Debug.Log("Checking PlayerPrefs for TotalPlayTime");
        float totalPlayTime = PlayerPrefs.GetFloat("TotalPlayTime", -1);
        if (totalPlayTime == -1)
        {
            Debug.Log("No data found for TotalPlayTime");
        }
        else
        {
            Debug.Log("TotalPlayTime: " + totalPlayTime);
        }
    }

    // Call this when the game starts (e.g., when the player presses the "Start" button)
    public void StartGame()
    {
        startTime = Time.time; // Record the start time
        isGameActive = true;
    }

    // Call this when the game ends or when switching scenes (e.g., when the player exits)
    public void EndGame()
    {
        if (isGameActive)
        {
            totalPlayTime += Time.time - startTime;  // Thêm thời gian đã chơi vào tổng thời gian chơi
            SaveRecord();  // Lưu dữ liệu vào PlayerPrefs
            isGameActive = false;
        }
    }


    // Load data from PlayerPrefs
    public void LoadRecord()
    {
        // Load dữ liệu từ PlayerPrefs
        totalPlayTime = PlayerPrefs.GetFloat("TotalPlayTime", 0f); // seconds
        totalKill = PlayerPrefs.GetInt("TotalKill", 0);
        totalDeath = PlayerPrefs.GetInt("TotalDeath", 0);
        totalGold = PlayerPrefs.GetInt("TotalGold", 0);

        // Debug để kiểm tra giá trị
        Debug.Log("Loaded Record: ");
        Debug.Log("Total Play Time: " + totalPlayTime);
        Debug.Log("Monsters Defeated: " + totalKill);
        Debug.Log("Total Deaths: " + totalDeath);
        Debug.Log("Total Gold Earned: " + totalGold);

        UpdateUI();  // Cập nhật lại UI với dữ liệu đã load
    }


    // Save data to PlayerPrefs
    public void SaveRecord()
    {
        PlayerPrefs.SetFloat("TotalPlayTime", totalPlayTime);
        PlayerPrefs.SetInt("TotalKill", totalKill);
        PlayerPrefs.SetInt("TotalDeath", totalDeath);
        PlayerPrefs.SetInt("TotalGold", totalGold);
        PlayerPrefs.Save(); // Save to disk
    }

    // Update the UI display
    public void UpdateUI()
    {
        int hours = Mathf.FloorToInt(totalPlayTime / 3600f);
        int minutes = Mathf.FloorToInt((totalPlayTime % 3600) / 60f);
        int seconds = Mathf.FloorToInt(totalPlayTime % 60);

        totalTimeText.text = $"Total Play Time: <b>{hours}h {minutes}m {seconds}s</b>";
        totalKillText.text = $"Monsters Defeated: <b>{totalKill}</b>";
        totalDeathText.text = $"Total Deaths: <b>{totalDeath}</b>";
        totalGoldText.text = $"Total Gold Earned: <b>{totalGold}</b>";
    }

    // Use this method to update the record and refresh the UI
    public void SetRecord(float playTime, int kill, int death, int gold)
    {
        totalPlayTime = playTime;
        totalKill = kill;
        totalDeath = death;
        totalGold = gold;
        UpdateUI();
    }

    // Call this method to update the kill, death, and gold values during the game
    public void UpdateGameStats(int kill, int death, int gold)
    {
        totalKill = kill;
        totalDeath = death;
        totalGold = gold;
        UpdateUI();
    }
}
