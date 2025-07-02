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
    public AdvancedDialogueProfile cutsceneProfile; // Kéo asset thoại cutscene vào đây

    [Header("Input System")]
    public PlayerInput playerInput;           // Kéo PlayerInput (trên Player) vào đây
    public string actionMapName = "Player";   // Đặt đúng tên ActionMap điều khiển

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

        // KHÓA INPUT bằng PlayerInput
        if (playerInput != null)
        {
            var map = playerInput.actions.FindActionMap(actionMapName);
            if (map != null) map.Disable();
        }
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
        yield return new WaitForSeconds(dialogueDelay);

        // === PREBIND: cập nhật nội dung UI trước khi hiện panel ===
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
        {
            cg = dialoguePanel.AddComponent<CanvasGroup>();
        }

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

        // GỌI THOẠI CỦA CUTSCENE (đã truyền avatar đúng)
        if (dialogueManager != null && cutsceneProfile != null)
        {
            dialogueManager.StartDialogueFromLines(
                cutsceneProfile.defaultLines,
                cutsceneProfile.characterName,
                cutsceneProfile.avatar,
                () =>
                {
                    // MỞ LẠI INPUT khi kết thúc thoại
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
            // Nếu không có dialogueManager thì bật input luôn
            if (playerInput != null)
            {
                var map = playerInput.actions.FindActionMap(actionMapName);
                if (map != null) map.Enable();
            }
        }
    }
}
