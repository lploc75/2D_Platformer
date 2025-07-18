using UnityEngine;

public class GameSaveManagerTrophy : MonoBehaviour
{
    public static GameSaveManagerTrophy Instance;

    // Dữ liệu cần load
    public float totalPlayTime;
    public int totalKill;
    public int totalDeath;
    public int totalGold;

    // Đảm bảo chỉ có một phương thức Awake duy nhất
    void Awake()
    {
        // Kiểm tra nếu chưa có instance, gán this làm instance
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Đảm bảo không bị xóa khi chuyển scene
            Debug.Log("GameSaveManagerTrophy instance created and set to DontDestroyOnLoad.");
        }
        else
        {
            Destroy(gameObject);  // Nếu đã có instance thì hủy đối tượng này
            Debug.Log("Duplicate GameSaveManagerTrophy instance destroyed.");
        }
    }

    // Phương thức để load dữ liệu
    public void LoadGame()
    {
        // Load dữ liệu từ PlayerPrefs
        totalPlayTime = PlayerPrefs.GetFloat("TotalPlayTime", 0f);
        totalKill = PlayerPrefs.GetInt("TotalKill", 0);
        totalDeath = PlayerPrefs.GetInt("TotalDeath", 0);
        totalGold = PlayerPrefs.GetInt("TotalGold", 0);

        Debug.Log("Game data loaded: " +
                  "Total Play Time: " + totalPlayTime +
                  " | Total Kill: " + totalKill +
                  " | Total Death: " + totalDeath +
                  " | Total Gold: " + totalGold);
    }
}
