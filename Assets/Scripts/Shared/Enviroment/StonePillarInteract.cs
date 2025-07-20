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
    public CameraFollows cameraFollows;
    public float shakeDuration = 0.3f;
    public float shakeMagnitude = 0.5f;

    public UnityEngine.UI.Image topBar;
    public UnityEngine.UI.Image bottomBar;
    public PlayerInput playerInput;

    [Header("Quest check & Self-talk")]
    public string requiredQuestId = "main_2_crystal";
    public DialogueManager dialogueManager;
    public AdvancedDialogueProfile selfTalkProfile;

    [Header("Save Key")]
    public string stoneKey = "pillar_001";   // Nhớ đặt key khác nhau cho mỗi viên đá!

    private bool playerInZone = false;
    private bool hasActivated = false;
    private Vector3 squareTargetPosition;
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
        // Lưu vị trí đỉnh cột ngay từ đầu để luôn đảm bảo đúng vị trí
        if (square)
            squareTargetPosition = square.transform.position;

        // Đã trả đá => Đá & hiệu ứng luôn hiện trên cột vĩnh viễn
        if (GameSaveManager.Instance != null && GameSaveManager.Instance.GetSpriteFadeStatus(stoneKey))
        {
            if (square)
            {
                square.transform.position = squareTargetPosition;
                square.SetActive(true);
            }
            if (particle && !particle.isPlaying)
                particle.Play();
            if (portalLight)
            {
                portalLight.enabled = true;
                portalLight.pointLightOuterRadius = normalOuter;
            }
            if (breathScript)
                breathScript.enabled = true;

            if (topBar) topBar.gameObject.SetActive(false);
            if (bottomBar) bottomBar.gameObject.SetActive(false);

            // Không cho tương tác lại nhưng vẫn giữ hiệu ứng
            enabled = false;
            return;
        }

        // Nếu chưa trả đá, setup như cũ
        if (square)
        {
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
                if (dialogueManager != null && selfTalkProfile != null && !dialogueManager.IsDialoguePlaying)
                    dialogueManager.StartDialogueByProfile(selfTalkProfile);
            }
        }
    }

    bool CanInteractWithPillar()
    {
        if (QuestManager.Instance == null)
            return false;
        return QuestManager.Instance.IsQuestReadyToComplete(requiredQuestId);
    }

    IEnumerator DoInteractEffectAndCompleteQuest()
    {
        hasActivated = true;

        if (cameraFollows) StartCoroutine(CameraShake());
        StartCoroutine(ShowBars(true));
        if (playerInput) playerInput.DeactivateInput();

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

        yield return new WaitForSeconds(shakeDuration);

        if (particle) particle.Play();
        if (portalLight) portalLight.enabled = true;
        if (breathScript) breathScript.enabled = false;

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

        // Đánh dấu hoàn thành quest
        if (QuestManager.Instance != null && QuestManager.Instance.IsQuestReadyToComplete(requiredQuestId))
        {
            QuestManager.Instance.CompleteQuest(requiredQuestId);
            QuestManager.Instance.RemoveReadyToComplete(requiredQuestId);
            Debug.Log($"[StonePillarInteract] Quest '{requiredQuestId}' marked as completed!");
        }

        // Lưu trạng thái đã trả đá vĩnh viễn và SAVE GAME
        if (GameSaveManager.Instance != null)
        {
            GameSaveManager.Instance.SetSpriteFadeStatus(stoneKey, true);
            GameSaveManager.Instance.SaveGame();
        }

        // Sau khi trả đá, viên đá và particle sẽ luôn hiện, tắt script tương tác
        if (square)
        {
            square.transform.position = squareTargetPosition;
            square.SetActive(true);
        }
        if (particle && !particle.isPlaying) particle.Play();
        if (portalLight)
        {
            portalLight.enabled = true;
            portalLight.pointLightOuterRadius = normalOuter;
        }
        if (breathScript) breathScript.enabled = true;

        if (playerInput) playerInput.ActivateInput();
        StartCoroutine(ShowBars(false));

        enabled = false; // Không cho tương tác lại nhưng vẫn giữ effect
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
