using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class StonePillarInteract : MonoBehaviour
{
    [Header("Effect Objects")]
    public GameObject square;
    public ParticleSystem particle;
    public Light2D portalLight;
    public LightBreath breathScript;

    [Header("Drop Effect")]
    public float normalOuter = 3f;
    public float highlightOuter = 50f;
    public float effectDuration = 1f;
    public float dropDuration = 1.2f;
    public float dropOffsetY = 10f;

    [Header("Camera, UI, Input")]
    public CameraFollows cameraFollows;        // Kéo script CameraFollows vào đây
    public float shakeDuration = 0.3f;
    public float shakeMagnitude = 0.5f;

    public UnityEngine.UI.Image topBar;        // Kéo image bar trên vào đây
    public UnityEngine.UI.Image bottomBar;     // Kéo image bar dưới vào đây

    public PlayerInput playerInput;            // Kéo PlayerInput vào đây

    [Header("Quest check & Self-talk")]
    public string requiredQuestId = "main_2_crystal";         // Quest phải "ready to complete" mới cho dùng trụ
    public DialogueManager dialogueManager;                   // Kéo DialogueManager vào đây
    public AdvancedDialogueProfile selfTalkProfile;           // Kéo prefab thoại vào đây

    private bool playerInZone = false;
    private bool hasActivated = false;
    private Vector3 squareTargetPosition;

    // Cài đặt hiệu ứng bar
    private float barSizePercent = 0.15f;
    private float barAnimTime = 0.4f;

    void Awake()
    {
        if (portalLight == null)
            portalLight = GetComponentInChildren<Light2D>();
        if (breathScript == null)
            breathScript = GetComponentInChildren<LightBreath>();
    }

    void Start()
    {
        if (square)
        {
            squareTargetPosition = square.transform.position;
            square.transform.position = squareTargetPosition + Vector3.up * dropOffsetY;
            square.SetActive(true);
        }
        if (particle) particle.Stop();
        if (breathScript) breathScript.enabled = false;

        if (portalLight)
        {
            portalLight.enabled = false;
            portalLight.pointLightOuterRadius = normalOuter;
        }

        // Đảm bảo 2 thanh đen ẩn mặc định
        if (topBar) topBar.gameObject.SetActive(false);
        if (bottomBar) bottomBar.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!hasActivated && playerInZone && Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (CanInteractWithPillar())
            {
                StartCoroutine(DoInteractEffectAndCompleteQuest());
            }
            else
            {
                // --- Chỉ hiện thoại prefab nếu không đủ điều kiện (quest chưa ready to complete) ---
                if (dialogueManager != null && selfTalkProfile != null && !dialogueManager.IsDialoguePlaying)
                {
                    dialogueManager.StartDialogueByProfile(selfTalkProfile);
                }
            }
        }
    }

    bool CanInteractWithPillar()
    {
        if (QuestManager.Instance == null)
            return false;
        // Chỉ cho kích hoạt nếu quest đang ở trạng thái ready to complete
        return QuestManager.Instance.IsQuestReadyToComplete(requiredQuestId);
    }

    IEnumerator DoInteractEffectAndCompleteQuest()
    {
        hasActivated = true;

        // (1) Camera shake + hiện bar NGAY LẬP TỨC
        if (cameraFollows) StartCoroutine(CameraShake());
        StartCoroutine(ShowBars(true));
        if (playerInput) playerInput.DeactivateInput();

        // (2) Đá rơi trong lúc đang shake & bar hiện
        if (square)
        {
            float timer = 0f;
            Vector3 startPos = square.transform.position;
            Vector3 endPos = squareTargetPosition;
            while (timer < dropDuration)
            {
                float t = timer / dropDuration;
                t = Mathf.SmoothStep(0, 1, t);
                square.transform.position = Vector3.Lerp(startPos, endPos, t);
                timer += Time.deltaTime;
                yield return null;
            }
            square.transform.position = endPos;
        }

        // (3) Đợi hết shake
        yield return new WaitForSeconds(shakeDuration);

        // (4) Các hiệu ứng khác
        if (particle) particle.Play();
        if (portalLight) portalLight.enabled = true;
        if (breathScript) breathScript.enabled = false;

        // Light nở ra
        float timer2 = 0f;
        float startOuter = portalLight.pointLightOuterRadius;
        while (timer2 < effectDuration / 2f)
        {
            float t = timer2 / (effectDuration / 2f);
            portalLight.pointLightOuterRadius = Mathf.Lerp(startOuter, highlightOuter, t);
            timer2 += Time.deltaTime;
            yield return null;
        }
        portalLight.pointLightOuterRadius = highlightOuter;

        // Co lại về normalOuter
        timer2 = 0f;
        startOuter = portalLight.pointLightOuterRadius;
        while (timer2 < effectDuration / 2f)
        {
            float t = timer2 / (effectDuration / 2f);
            portalLight.pointLightOuterRadius = Mathf.Lerp(highlightOuter, normalOuter, t);
            timer2 += Time.deltaTime;
            yield return null;
        }
        portalLight.pointLightOuterRadius = normalOuter;

        if (breathScript) breathScript.enabled = true;

        // === (NEW) Mark quest as completed if it was ready ===
        if (QuestManager.Instance != null && QuestManager.Instance.IsQuestReadyToComplete(requiredQuestId))
        {
            QuestManager.Instance.CompleteQuest(requiredQuestId);
            QuestManager.Instance.RemoveReadyToComplete(requiredQuestId);
            Debug.Log($"[StonePillarInteract] Quest '{requiredQuestId}' marked as completed!");
        }

        // (5) Tắt bar và bật lại input sau hiệu ứng
        if (playerInput) playerInput.ActivateInput();
        StartCoroutine(ShowBars(false));
    }

    IEnumerator CameraShake()
    {
        float elapsed = 0.0f;
        while (elapsed < shakeDuration)
        {
            float offsetX = Random.Range(-1f, 1f) * shakeMagnitude;
            float offsetY = Random.Range(-1f, 1f) * shakeMagnitude;
            if (cameraFollows) cameraFollows.shakeOffset = new Vector3(offsetX, offsetY, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }
        if (cameraFollows) cameraFollows.shakeOffset = Vector3.zero;
    }

    IEnumerator ShowBars(bool show)
    {
        if (topBar == null || bottomBar == null) yield break;

        float duration = barAnimTime;
        float percent = barSizePercent;
        RectTransform rtTop = topBar.GetComponent<RectTransform>();
        RectTransform rtBot = bottomBar.GetComponent<RectTransform>();

        // Bật object bar lên để chắc chắn luôn hiện!
        topBar.gameObject.SetActive(true);
        bottomBar.gameObject.SetActive(true);

        float from = show ? 0f : percent;
        float to = show ? percent : 0f;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            float size = Mathf.Lerp(from, to, t);

            rtTop.anchorMin = new Vector2(0, 1f - size);
            rtTop.anchorMax = new Vector2(1, 1f);
            rtBot.anchorMin = new Vector2(0, 0f);
            rtBot.anchorMax = new Vector2(1, size);

            elapsed += Time.deltaTime;
            yield return null;
        }
        rtTop.anchorMin = new Vector2(0, 1f - to);
        rtTop.anchorMax = new Vector2(1, 1f);
        rtBot.anchorMin = new Vector2(0, 0f);
        rtBot.anchorMax = new Vector2(1, to);

        // Ẩn object bar hoàn toàn sau khi thu về 0
        if (!show)
        {
            topBar.gameObject.SetActive(false);
            bottomBar.gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player")) playerInZone = true;
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player")) playerInZone = false;
    }
}
