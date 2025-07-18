using UnityEngine;
using TMPro;

public class TrophyRecordUI : MonoBehaviour
{
    [Header("UI TMP_Text References")]
    public TMP_Text totalTimeText;
    public TMP_Text totalKillText;
    public TMP_Text totalDeathText;
    public TMP_Text totalGoldText;

    // Biến cần sửa lại thành public
    public float totalPlayTime; // In seconds
    public int totalKill;
    public int totalDeath;
    public int totalGold;

    private float startTime; // Time when the game starts
    private bool isGameActive = false; // Flag to track whether the game is active

    void Awake()
    {
        // Kiểm tra nếu đối tượng TrophyRecordUI đã tồn tại và hủy nó nếu cần thiết
        if (FindObjectOfType<TrophyRecordUI>() != null && FindObjectOfType<TrophyRecordUI>() != this)
        {
            Destroy(gameObject); // Hủy đối tượng trùng lặp nếu tồn tại
            Debug.LogError("TrophyRecordUI is not found in the scene!");
        }
        else
        {
            DontDestroyOnLoad(gameObject); // Đảm bảo đối tượng này không bị xóa khi scene thay đổi
            Debug.Log("TrophyRecordUI is found and active.");
        }
    }

    void Start()
    {
        Debug.Log("Start method is called");
        LoadRecord(); // Load record data when the game starts
        UpdateUI(); // Update UI to display loaded data
        StartGame();
    }

    void Update()
    {
        // Nếu game đang hoạt động, tính toán thời gian và cập nhật UI liên tục
        if (isGameActive)
        {
            totalPlayTime += Time.deltaTime; // Tính toán thời gian chơi
            UpdateUI();  // Cập nhật giao diện UI liên tục

            // Log thời gian chơi mỗi frame
            LogPlayTime();
        }
        else
        {
            Debug.Log("isGameActive is false");
        }
    }

    // Hàm log thời gian chơi khi game đang diễn ra
    void LogPlayTime()
    {
        int hours = Mathf.FloorToInt(totalPlayTime / 3600f);
        int minutes = Mathf.FloorToInt((totalPlayTime % 3600) / 60f);
        int seconds = Mathf.FloorToInt(totalPlayTime % 60);
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
            totalPlayTime += Time.time - startTime; // Add the time spent during this session
            SaveRecord(); // Save record data to PlayerPrefs
            isGameActive = false;
        }
    }


    // Load data from PlayerPrefs
    public void LoadRecord()
    {
        Debug.Log("Loading data from PlayerPrefs...");

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
        Debug.Log("Saving data to PlayerPrefs...");

        // Lưu dữ liệu vào PlayerPrefs
        PlayerPrefs.SetFloat("TotalPlayTime", totalPlayTime);
        PlayerPrefs.SetInt("TotalKill", totalKill);
        PlayerPrefs.SetInt("TotalDeath", totalDeath);
        PlayerPrefs.SetInt("TotalGold", totalGold);
        PlayerPrefs.Save(); // Save to disk

        Debug.Log("Data saved successfully.");
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
