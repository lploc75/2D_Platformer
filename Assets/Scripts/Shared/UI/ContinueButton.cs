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
        if (GameSaveManagerTrophy.Instance != null)
        {
            // Nếu Instance đã được khởi tạo, thực hiện load game
            GameSaveManagerTrophy.Instance.LoadGame();
            UpdateUI();

            if (delay > 0f)
                StartCoroutine(ContinueAfterDelay());
            else
                SceneManager.LoadScene(sceneName);
        }
        else
        {
            // Nếu Instance chưa được khởi tạo, in ra lỗi hoặc thông báo cho người dùng
            Debug.LogError("GameSaveManagerTrophy.Instance không tồn tại!");
        }
    }


    IEnumerator ContinueAfterDelay()
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }

    // Cập nhật UI với dữ liệu đã load
    private void UpdateUI()
    {
        // Tìm đối tượng TrophyRecordUI trong scene để cập nhật thông tin
        TrophyRecordUI trophyRecordUI = FindObjectOfType<TrophyRecordUI>();

        if (trophyRecordUI != null)
        {
            // Cập nhật UI với dữ liệu đã load từ GameSaveManagerTrophy
            trophyRecordUI.totalPlayTime = GameSaveManagerTrophy.Instance.totalPlayTime;
            trophyRecordUI.totalKill = GameSaveManagerTrophy.Instance.totalKill;
            trophyRecordUI.totalDeath = GameSaveManagerTrophy.Instance.totalDeath;
            trophyRecordUI.totalGold = GameSaveManagerTrophy.Instance.totalGold;

            // Cập nhật giao diện
            trophyRecordUI.UpdateUI();
        }
        else
        {
            Debug.LogError("TrophyRecordUI not found in the scene!");
        }
    }
}
