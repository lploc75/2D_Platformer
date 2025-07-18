using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ContinueButton : MonoBehaviour
{
    [Tooltip("Tên hoặc index scene muốn tải khi Continue")]
    public string sceneName = "Village";

    [Tooltip("Chờ bao lâu trước khi load (để âm thanh click phát xong)")]
    public float delay = 0.5f;

    // Gọi hàm này cho nút CONTINUE
    public void OnClickContinue()
    {
        if (GameSaveManager.Instance != null)
        {
            GameSaveManager.Instance.LoadGame();
            if (delay > 0f)
                StartCoroutine(ContinueAfterDelay());
            else
                SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("Không tìm thấy GameSaveManager.Instance!");
        }
    }

    IEnumerator ContinueAfterDelay()
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }
}
