using UnityEngine;
using System.Collections.Generic;

public class GamePauseManager : MonoBehaviour
{
    public static GamePauseManager Instance { get; private set; }

    [Header("Panels sẽ kiểm tra để pause game")]
    public List<GameObject> panelsToCheck;

    private bool isPaused = false;
    private PlayerController playerController;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Gán đúng player (nếu player có thể bị spawn lại thì cần gán lại khi cần)
        playerController = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        CheckPanelsAndPause();
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        if (playerController != null)
        {
            playerController.canControl = false;
            playerController.ResetActionState();
        }
        Debug.Log("[GamePauseManager] Game Paused & Player Action Disabled");
    }


    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        if (playerController != null) playerController.canControl = true; // ENABLE LẠI ACTION GAMEPLAY
        Debug.Log("[GamePauseManager] Game Resumed & Player Action Enabled");
    }

    public void CheckPanelsAndPause()
    {
        bool shouldPause = false;
        foreach (var panel in panelsToCheck)
        {
            if (panel != null && panel.activeSelf)
            {
                shouldPause = true;
                break;
            }
        }
        if (shouldPause && !isPaused)
        {
            PauseGame();
        }
        else if (!shouldPause && isPaused)
        {
            ResumeGame();
        }
    }
}
