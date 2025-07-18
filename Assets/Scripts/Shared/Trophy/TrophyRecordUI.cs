using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;  // Để sử dụng SceneManager

public class TrophyRecordUI : MonoBehaviour
{
    [Header("UI TMP_Text References")]
    public TMP_Text totalTimeText;
    public TMP_Text totalKillText;
    public TMP_Text totalDeathText;
    public TMP_Text totalGoldText;

    public float totalPlayTime; // Thời gian chơi tính bằng giây
    public int totalKill;
    public int totalDeath;
    public int totalGold;

    private float startTime;
    private bool isGameActive = false;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);  // Đảm bảo đối tượng này không bị xóa khi chuyển scene
        Debug.Log("TrophyRecordUI is found and active.");
        StartGame();  // Bắt đầu đếm thời gian ngay khi script chạy
    }

    void Start()
    {
        LoadRecord(); // Load dữ liệu khi game bắt đầu
        UpdateUI();   // Cập nhật UI với dữ liệu đã load
    }

    void Update()
    {
        // Nếu game đang hoạt động, tính toán thời gian và cập nhật UI liên tục
        if (isGameActive)
        {
            totalPlayTime += Time.deltaTime; // Cập nhật thời gian chơi
            UpdateUI();  // Cập nhật giao diện UI liên tục
            LogPlayTime();
        }
    }

    void LogPlayTime()
    {
        int hours = Mathf.FloorToInt(totalPlayTime / 3600f);
        int minutes = Mathf.FloorToInt((totalPlayTime % 3600) / 60f);
        int seconds = Mathf.FloorToInt(totalPlayTime % 60);
        Debug.Log($"Time Played: {hours}h {minutes}m {seconds}s");
    }

    public void StartGame()
    {
        startTime = Time.time; // Lưu lại thời gian bắt đầu
        isGameActive = true;
    }

    public void EndGame()
    {
        if (isGameActive)
        {
            totalPlayTime += Time.time - startTime; // Cộng thời gian đã chơi vào tổng thời gian
            SaveRecord();  // Lưu dữ liệu vào PlayerPrefs
            isGameActive = false;
        }
    }

    public void SaveRecord()
    {
        Debug.Log("Saving data to PlayerPrefs...");
        PlayerPrefs.SetFloat("TotalPlayTime", totalPlayTime);
        PlayerPrefs.SetInt("TotalKill", totalKill);
        PlayerPrefs.SetInt("TotalDeath", totalDeath);
        PlayerPrefs.SetInt("TotalGold", totalGold);
        PlayerPrefs.Save(); // Lưu vào disk

        Debug.Log("Data saved successfully.");
    }

    public void LoadRecord()
    {
        Debug.Log("Loading data from PlayerPrefs...");
        totalPlayTime = PlayerPrefs.GetFloat("TotalPlayTime", 0f);  // seconds
        totalKill = PlayerPrefs.GetInt("TotalKill", 0);
        totalDeath = PlayerPrefs.GetInt("TotalDeath", 0);
        totalGold = PlayerPrefs.GetInt("TotalGold", 0);

        Debug.Log("Loaded Record: ");
        Debug.Log("Total Play Time: " + totalPlayTime);
        Debug.Log("Monsters Defeated: " + totalKill);
        Debug.Log("Total Deaths: " + totalDeath);
        Debug.Log("Total Gold Earned: " + totalGold);

        UpdateUI();  // Cập nhật lại UI với dữ liệu đã load
    }

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

    // Dùng phương thức này để cập nhật giá trị và làm mới UI
    public void SetRecord(float playTime, int kill, int death, int gold)
    {
        totalPlayTime = playTime;
        totalKill = kill;
        totalDeath = death;
        totalGold = gold;
        UpdateUI();
    }

    // Cập nhật stats trong game (khi người chơi giết quái, chết hoặc nhận vàng)
    public void UpdateGameStats(int kill, int death, int gold)
    {
        totalKill = kill;
        totalDeath = death;
        totalGold = gold;
        UpdateUI();
    }

    // Gọi phương thức này trước khi chuyển scene
    public void SaveTimeBeforeSceneChange()
    {
        PlayerPrefs.SetFloat("TotalPlayTime", totalPlayTime);
        PlayerPrefs.Save();  // Lưu thời gian khi chuyển scene
        Debug.Log("Time saved before scene change: " + totalPlayTime);
    }

    // Gọi phương thức này khi vào map mới
    public void LoadTimeAfterSceneChange()
    {
        totalPlayTime = PlayerPrefs.GetFloat("TotalPlayTime", 0f);  // Lấy thời gian đã lưu từ PlayerPrefs
        Debug.Log("Time loaded after scene change: " + totalPlayTime);
    }

    // Phương thức này sẽ được gọi khi game tắt
    void OnApplicationQuit()
    {
        Debug.Log("Game is quitting. Saving data...");
        SaveRecord(); // Lưu dữ liệu khi game tắt
    }

    // Phương thức này sẽ được gọi khi chuyển scene
    public void ChangeScene()
    {
        SaveTimeBeforeSceneChange();  // Lưu thời gian trước khi chuyển scene
        SceneManager.LoadScene("NextScene");  // Chuyển sang scene mới
    }
}
