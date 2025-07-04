using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class SimpleCameraMove : MonoBehaviour
{
    public Vector3 startPos;
    public Vector3 endPos;
    public float moveDuration = 2f;
    public MonoBehaviour followCameraScript;
    public GameObject dialoguePanel;
    public float dialogueDelay = 1.5f;
    public DialogueManager dialogueManager;
    public AdvancedDialogueProfile cutsceneProfile;

    [Header("Cinematic Effect")]
    public bool useCinematicBars = true;
    public float barSizePercent = 0.15f;
    public float barAnimTime = 0.4f;
    public GameObject topBar, bottomBar;

    [Header("Input System")]
    public PlayerInput playerInput;
    public string actionMapName = "Player";

    [Header("Hide UI During Cutscene")]
    public GameObject healthUI;        // Kéo UI cần ẩn (Health) vào đây
    public GameObject[] otherUIs;      // Nếu muốn ẩn nhiều UI khác nữa

    private float timer = 0f;
    private bool moving = false;

    void Start()
    {
        transform.position = startPos;
        if (followCameraScript != null)
            followCameraScript.enabled = false;
        moving = true;
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        // ẨN UI lúc cinematic move bắt đầu
        if (healthUI != null)
            healthUI.SetActive(false);
        if (otherUIs != null)
            foreach (var ui in otherUIs)
                if (ui != null) ui.SetActive(false);

        // KHÓA INPUT
        if (playerInput != null)
        {
            var map = playerInput.actions.FindActionMap(actionMapName);
            if (map != null) map.Disable();
        }

        // SHOW CINEMATIC BARS nếu có
        if (useCinematicBars)
            StartCoroutine(ShowCinematicBars(true));
    }

    void Update()
    {
        if (!moving) return;

        timer += Time.deltaTime;
        float t = Mathf.Clamp01(timer / moveDuration);

        transform.position = Vector3.Lerp(startPos, endPos, t);

        if (t >= 1f)
        {
            moving = false;
            if (followCameraScript != null)
                followCameraScript.enabled = true;
            if (dialoguePanel != null)
                StartCoroutine(ShowDialogueWithFadeIn());
        }
    }

    IEnumerator ShowDialogueWithFadeIn()
    {
        // === HỆT THANH BAR TRƯỚC khi hiện thoại ===
        if (useCinematicBars)
            StartCoroutine(ShowCinematicBars(false));

        // HIỆN LẠI UI trước khi thoại (health UI, các UI khác)
        if (healthUI != null)
            healthUI.SetActive(true);
        if (otherUIs != null)
            foreach (var ui in otherUIs)
                if (ui != null) ui.SetActive(true);

        yield return new WaitForSeconds(dialogueDelay);

        // PREBIND: cập nhật nội dung UI trước khi hiện panel
        if (dialogueManager != null && cutsceneProfile != null)
        {
            dialogueManager.PreBindDialogue(
                cutsceneProfile.defaultLines,
                cutsceneProfile.characterName,
                cutsceneProfile.avatar
            );
        }

        dialoguePanel.SetActive(true);

        CanvasGroup cg = dialoguePanel.GetComponent<CanvasGroup>();
        if (cg == null)
            cg = dialoguePanel.AddComponent<CanvasGroup>();

        cg.alpha = 0f;
        float fadeDuration = 0.6f;
        float fadeTimer = 0f;

        while (fadeTimer < fadeDuration)
        {
            fadeTimer += Time.deltaTime;
            cg.alpha = Mathf.Lerp(0f, 1f, fadeTimer / fadeDuration);
            yield return null;
        }
        cg.alpha = 1f;

        // GỌI THOẠI CỦA CUTSCENE
        if (dialogueManager != null && cutsceneProfile != null)
        {
            dialogueManager.StartDialogueFromLines(
                cutsceneProfile.defaultLines,
                cutsceneProfile.characterName,
                cutsceneProfile.avatar,
                () =>
                {
                    // Kết thúc thoại, mở lại input
                    if (playerInput != null)
                    {
                        var map = playerInput.actions.FindActionMap(actionMapName);
                        if (map != null) map.Enable();
                    }
                }
            );
        }
        else
        {
            if (playerInput != null)
            {
                var map = playerInput.actions.FindActionMap(actionMapName);
                if (map != null) map.Enable();
            }
        }
    }

    // Coroutine hiệu ứng hiện/ẩn letterbox bar trên/dưới
    IEnumerator ShowCinematicBars(bool show)
    {
        if (topBar == null || bottomBar == null) yield break;

        CanvasGroup cgTop = topBar.GetComponent<CanvasGroup>();
        if (cgTop == null) cgTop = topBar.AddComponent<CanvasGroup>();
        CanvasGroup cgBot = bottomBar.GetComponent<CanvasGroup>();
        if (cgBot == null) cgBot = bottomBar.AddComponent<CanvasGroup>();

        RectTransform rtTop = topBar.GetComponent<RectTransform>();
        RectTransform rtBot = bottomBar.GetComponent<RectTransform>();

        float targetSize = show ? barSizePercent : 0f;
        float startSize = rtTop.anchorMax.y;
        float elapsed = 0f;

        while (elapsed < barAnimTime)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / barAnimTime);
            float size = Mathf.Lerp(startSize, targetSize, t);

            rtTop.anchorMin = new Vector2(0, 1f - size);
            rtTop.anchorMax = new Vector2(1, 1f);
            cgTop.alpha = t;

            rtBot.anchorMin = new Vector2(0, 0f);
            rtBot.anchorMax = new Vector2(1, size);
            cgBot.alpha = t;

            yield return null;
        }

        rtTop.anchorMin = new Vector2(0, 1f - targetSize);
        rtTop.anchorMax = new Vector2(1, 1f);
        cgTop.alpha = show ? 1f : 0f;
        rtBot.anchorMin = new Vector2(0, 0f);
        rtBot.anchorMax = new Vector2(1, targetSize);
        cgBot.alpha = show ? 1f : 0f;
    }
}
