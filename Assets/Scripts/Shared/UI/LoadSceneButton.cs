using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneButton : MonoBehaviour
{
    [Tooltip("Tên hoặc index scene muốn tải")]
    public string sceneName = "Village";

    [Tooltip("Chờ bao lâu trước khi load (để âm thanh click phát xong)")]
    public float delay = 0.5f;

    public void LoadGameScene()
    {
        // Gọi âm thanh click (nếu dùng AudioManager)
        //AudioManager.Instance?.PlayClickSound();

        // Nếu muốn đợi âm thanh kết thúc
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
}
