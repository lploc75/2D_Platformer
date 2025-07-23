using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; // Nếu dùng Input System mới
using System.Collections;

public class PauseMenuManager : MonoBehaviour
{
    [Header("Panel References")]
    public GameObject pausePanel;
    public GameObject settingsPanel;

    private bool isPaused = false;

    private AudioListener[] audioListeners;
    private PlayerInput[] playerInputs;

    void Start()
    {
        if (pausePanel != null)
            StartCoroutine(DeactivatePausePanelNextFrame());
        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        audioListeners = FindObjectsOfType<AudioListener>(true);
        playerInputs = FindObjectsOfType<PlayerInput>(true);
    }

    IEnumerator DeactivatePausePanelNextFrame()
    {
        yield return null;
        pausePanel.SetActive(false);
        isPaused = false;
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (pausePanel == null) return;

        isPaused = !isPaused;
        pausePanel.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;

        SetAudioMuted(isPaused);
        SetInputEnabled(!isPaused);

        if (!isPaused && settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    public void OpenSettings()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    public void ResumeGame()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        SetAudioMuted(false);
        SetInputEnabled(true);

        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    public void QuitGame()
    {
        // Reset trạng thái trước khi về MainMenu
        Time.timeScale = 1f;
        SetAudioMuted(false);
        SetInputEnabled(true);

        SceneManager.LoadScene("MainMenuScene");
    }

    void SetAudioMuted(bool muted)
    {
        if (audioListeners != null)
        {
            foreach (var audioListener in audioListeners)
            {
                if (audioListener != null)
                    audioListener.enabled = !muted;
            }
        }
    }

    void SetInputEnabled(bool enabled)
    {
        if (playerInputs != null)
        {
            foreach (var pi in playerInputs)
            {
                if (pi != null)
                    pi.enabled = enabled;
            }
        }
    }
}
