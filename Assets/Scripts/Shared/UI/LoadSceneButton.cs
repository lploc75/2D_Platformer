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
        //AudioManager.Instance?.PlayClickSound();

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
            SceneManager.LoadScene(sceneName);
        else
            Debug.LogError("❌ Chưa cấu hình sceneName / sceneIndex!");
    }

    // Gọi hàm này cho nút NEW GAME
    public void OnClickNewGame()
    {
        if (GameSaveManager.Instance != null)
        {
            GameSaveManager.Instance.StartNewGame(sceneName);
        }
        else
        {
            Debug.LogError("Không tìm thấy GameSaveManager.Instance!");
        }
    }
}
