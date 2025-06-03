using UnityEngine;

public class SimpleCameraMove : MonoBehaviour
{
    public Vector3 startPos;
    public Vector3 endPos;
    public float moveDuration = 2f;
    public MonoBehaviour followCameraScript;
    public GameObject dialoguePanel;            // Kéo hộp thoại UI vào đây
    public float dialogueDelay = 1.5f;          // Thời gian delay trước khi show hộp thoại (giây)

    private float timer = 0f;
    private bool moving = false;

    void Start()
    {
        transform.position = startPos;
        if (followCameraScript != null)
            followCameraScript.enabled = false;
        moving = true;
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);     // Tắt hộp thoại lúc đầu
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

    System.Collections.IEnumerator ShowDialogueWithFadeIn()
    {
        yield return new WaitForSeconds(dialogueDelay);

        // Bật hộp thoại (nếu dùng CanvasGroup, sẽ fade)
        dialoguePanel.SetActive(true);

        CanvasGroup cg = dialoguePanel.GetComponent<CanvasGroup>();
        if (cg == null)
        {
            cg = dialoguePanel.AddComponent<CanvasGroup>();
        }

        cg.alpha = 0f;
        float fadeDuration = 0.6f; // Thời gian fade-in (giây)
        float fadeTimer = 0f;

        while (fadeTimer < fadeDuration)
        {
            fadeTimer += Time.deltaTime;
            cg.alpha = Mathf.Lerp(0f, 1f, fadeTimer / fadeDuration);
            yield return null;
        }
        cg.alpha = 1f;
    }
}
