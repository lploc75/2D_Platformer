using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneButton : MonoBehaviour
{
    [Tooltip("Tên hoặc index scene muốn tải")]
    public string sceneName = "Village";

    [Tooltip("Chờ bao lâu trước khi load (để âm thanh click phát xong)")]
    public float delay = 0.5f;

    // Gọi hàm này cho nút Load tiếp tục
    public void LoadGameScene()
    {
        if (delay > 0f)
            StartCoroutine(LoadAfterDelay());
        else
            DoLoad();
    }

    IEnumerator LoadAfterDelay()
    {
        yield return new WaitForSeconds(delay);
        DoLoad();
    }

    void DoLoad()
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            // Thêm log trước khi load scene
            Debug.Log("Starting to load scene: " + sceneName);

            // Đăng ký hàm xử lý khi scene được tải
            SceneManager.sceneLoaded += OnSceneLoaded;

            // Tải scene
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("❌ Chưa cấu hình sceneName / sceneIndex!");
        }
    }

    // Xử lý khi scene được load xong
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Đảm bảo sự kiện chỉ được xử lý một lần
        SceneManager.sceneLoaded -= OnSceneLoaded;

        // Log khi scene đã được load xong
        Debug.Log("Scene loaded: " + scene.name);

        // Cập nhật thời gian và dữ liệu
        TrophyRecordUI trophyRecordUI = FindObjectOfType<TrophyRecordUI>();
        if (trophyRecordUI != null)
        {
            // Set lại thời gian khi vào scene
            trophyRecordUI.LoadRecord();  // Load lại dữ liệu từ PlayerPrefs (hoặc JSON)
            trophyRecordUI.UpdateUI();   // Cập nhật UI với dữ liệu đã load
        }
    }

    // Gọi hàm này cho nút NEW GAME
    public void OnClickNewGame()
    {
        if (GameSaveManager.Instance != null)
        {
            // Lưu dữ liệu trước khi reset
            TrophyRecordUI trophyRecordUI = FindObjectOfType<TrophyRecordUI>();
            if (trophyRecordUI != null)
            {
                trophyRecordUI.SaveRecord();  // Lưu dữ liệu trước khi reset
            }

            // Xóa hoặc reset dữ liệu khi bắt đầu game mới
            ResetGameData();

            // Bắt đầu game mới
            GameSaveManager.Instance.StartNewGame(sceneName);
        }
        else
        {
            Debug.LogError("Không tìm thấy GameSaveManager.Instance!");
        }
    }


    // Hàm reset lại dữ liệu (có thể dùng PlayerPrefs hoặc file JSON tùy nhu cầu)
    private void ResetGameData()
    {
        // Xóa dữ liệu lưu trữ trong PlayerPrefs
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();  // Đảm bảo rằng dữ liệu đã được xóa và lưu

        Debug.Log("Dữ liệu game đã được đặt lại!");
    }
}
