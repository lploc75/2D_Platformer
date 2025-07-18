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

        // G�n s? ki?n click cho n�t
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
        // N?u c� d?ng game: Time.timeScale = 1;
    }

    public void OnRetryClicked()
    {
        HideGameOver();
        GameManager.Instance.RevivePlayerAtCheckpoint();
        // Reset m�u, tr?ng th�i... n?u c?n
    }

    public void OnReturnVillageClicked()
    {
        HideGameOver();
        // ??i "VillageSceneName" th�nh t�n scene c?a l�ng
        SceneManager.LoadScene("Village");
    }
}
