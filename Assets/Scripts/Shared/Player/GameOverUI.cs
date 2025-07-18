using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    public GameObject gameOverPanel;
    public Button retryButton;
    public Button returnVillageButton;

    private void Awake()
    {
        // ?n Panel khi start
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        // Gán s? ki?n click cho nút
        retryButton.onClick.AddListener(OnRetryClicked);
        returnVillageButton.onClick.AddListener(OnReturnVillageClicked);
    }

    public void ShowGameOver()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
        // D?ng game n?u mu?n: Time.timeScale = 0;
    }

    public void HideGameOver()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
        // N?u có d?ng game: Time.timeScale = 1;
    }

    public void OnRetryClicked()
    {
        HideGameOver();
        GameManager.Instance.RevivePlayerAtCheckpoint();
        // Reset máu, tr?ng thái... n?u c?n
    }

    public void OnReturnVillageClicked()
    {
        HideGameOver();
        // ??i "VillageSceneName" thành tên scene c?a làng
        SceneManager.LoadScene("Village");
    }
}
