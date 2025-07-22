using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using System.Collections;
using UnityEngine.SceneManagement;

public class PortalLightController : MonoBehaviour
{
    [Header("Portal Light Settings")]
    public Light2D portalLight;      // Assign Light2D in Inspector
    public float flashMax = 400f;    // Flash peak intensity
    public float flashDuration = 1f; // Total flash time (0 -> max -> 0)
    public string targetSceneName;

    [Header("Quest requirement")]
    public string requiredQuestId = "main_1_crystal";

    [Header("Is this a return portal? (Allow always)")]
    public bool isReturnPortal = false;

    [Header("Self-talk when not allowed (English)")]
    public DialogueManager dialogueManager;
    public AdvancedDialogueProfile selfTalkProfile;

    bool playerInZone = false;
    bool isFlashing = false;

    void Start()
    {
        if (portalLight != null) portalLight.intensity = 0;
    }

    void Update()
    {
        if (playerInZone && !isFlashing && Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (CanInteractWithPortal())
            {
                StartCoroutine(FlashLightAndSaveGame());
            }
            else
            {
                // Play self-talk in English when quest not in progress or not accepted
                if (dialogueManager != null && selfTalkProfile != null && !dialogueManager.IsDialoguePlaying)
                {
                    dialogueManager.StartDialogueByProfile(selfTalkProfile);
                }
            }
        }
    }

    /// <summary>
    /// Only allow teleport if:
    /// - isReturnPortal == true (always allow)
    /// - Quest is in progress (accepted but not ready to complete and not completed)
    /// </summary>
    bool CanInteractWithPortal()
    {
        if (isReturnPortal)
            return true;

        if (QuestManager.Instance == null)
            return false;

        // Only allow when quest is "In Progress"
        return QuestManager.Instance.IsQuestInProgress(requiredQuestId);
    }

    private IEnumerator FlashLightAndSaveGame()
    {
        isFlashing = true;
        float timer = 0f;

        // Flash up (0 -> max)
        while (timer < flashDuration / 2f)
        {
            float t = timer / (flashDuration / 2f);
            portalLight.intensity = Mathf.Lerp(0, flashMax, t);
            timer += Time.deltaTime;
            yield return null;
        }
        portalLight.intensity = flashMax;

        // Flash down (max -> 0)
        timer = 0f;
        while (timer < flashDuration / 2f)
        {
            float t = timer / (flashDuration / 2f);
            portalLight.intensity = Mathf.Lerp(flashMax, 0, t);
            timer += Time.deltaTime;
            yield return null;
        }
        portalLight.intensity = 0;
        isFlashing = false;

        // ===== SAVE GAME BEFORE SCENE CHANGE =====
        if (GameSaveManager.Instance != null)
        {
            GameSaveManager.Instance.SaveGame();
            yield return null;
        }

        // Load scene after flash and save
        if (!string.IsNullOrEmpty(targetSceneName))
        {
            SceneManager.LoadScene(targetSceneName);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
            playerInZone = true;
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
            playerInZone = false;
    }
}
