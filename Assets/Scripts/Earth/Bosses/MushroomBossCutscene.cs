using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Assets.Scripts.Earth.Common_Enemies;

public class MushroomBossCutscene : MonoBehaviour
{
    public HugeMushroom boss;
    public Transform player;
    public Transform playerBody;
    public GameObject magicStone;
    public string questIdToComplete = "main_1_crystal";

    [Header("Cinematic Bars")]
    public RectTransform topBar;     // Kéo RectTransform của Image trên vào đây
    public RectTransform bottomBar;  // Kéo RectTransform của Image dưới vào đây
    public float barSlideTime = 0.4f;
    public float barTargetHeight = 100f; // Chiều cao bar khi hiện

    [Header("Fly Config")]
    public float flyUpHeight = 2f;
    public float flyUpDuration = 0.7f;
    public float delayBetween = 1.0f;
    public float flyToPlayerDuration = 1.2f;

    private bool cutsceneStarted = false;

    void Start()
    {
        if (magicStone != null)
            magicStone.SetActive(false);

        // Hide bar (set height = 0)
        if (topBar != null) SetBarHeight(topBar, 0f);
        if (bottomBar != null) SetBarHeight(bottomBar, 0f);
    }

    void Update()
    {
        if (!cutsceneStarted && boss != null && !IsBossAlive())
        {
            StartCoroutine(PlayStoneFlyCutscene());
        }
    }

    bool IsBossAlive()
    {
        return boss.GetComponent<Animator>().GetBool(EnemiesAnimationStrings.isAlive);
    }

    IEnumerator PlayStoneFlyCutscene()
    {
        cutsceneStarted = true;

        // Slide In Cinema Bars
        if (topBar != null && bottomBar != null)
            yield return StartCoroutine(SlideCinemaBars(barTargetHeight, barSlideTime));

        // Disable player control
        PlayerInput input = player.GetComponent<PlayerInput>();
        if (input != null) input.enabled = false;

        // Viên đá xuất hiện tại đúng vị trí
        magicStone.SetActive(true);
        Vector3 start = magicStone.transform.position;
        magicStone.transform.SetParent(null);
        Vector3 up = start + Vector3.up * flyUpHeight;
        magicStone.transform.position = start;

        // Bay lên cao
        float timer = 0f;
        while (timer < flyUpDuration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / flyUpDuration);
            magicStone.transform.position = Vector3.Lerp(start, up, t);
            yield return null;
        }
        magicStone.transform.position = up;

        // Đợi trên không
        yield return new WaitForSeconds(delayBetween);

        // Bay vào playerBody
        if (playerBody == null)
        {
            Debug.LogError("playerBody not set!");
            yield break;
        }
        Vector3 end = playerBody.position;
        timer = 0f;
        while (timer < flyToPlayerDuration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / flyToPlayerDuration);
            magicStone.transform.position = Vector3.Lerp(up, end, t);
            yield return null;
        }
        magicStone.transform.position = end;

        yield return new WaitForSeconds(0.3f);
        magicStone.SetActive(false);

        // Đánh dấu hoàn thành nhiệm vụ
        if (QuestManager.Instance != null)
            QuestManager.Instance.CompleteQuest(questIdToComplete);

        // Save game nếu muốn
        if (GameSaveManager.Instance != null)
            GameSaveManager.Instance.SaveGame();

        // Slide Out Cinema Bars
        if (topBar != null && bottomBar != null)
            yield return StartCoroutine(SlideCinemaBars(0f, barSlideTime));

        // Enable player control
        if (input != null) input.enabled = true;
    }

    // Helper: set height bar
    void SetBarHeight(RectTransform rt, float h)
    {
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, h);
    }

    IEnumerator SlideCinemaBars(float targetHeight, float duration)
    {
        float t = 0f;
        float startTop = topBar.sizeDelta.y;
        float startBot = bottomBar.sizeDelta.y;
        while (t < duration)
        {
            t += Time.deltaTime;
            float h = Mathf.Lerp(startTop, targetHeight, t / duration);
            if (topBar != null) SetBarHeight(topBar, h);
            if (bottomBar != null) SetBarHeight(bottomBar, h);
            yield return null;
        }
        if (topBar != null) SetBarHeight(topBar, targetHeight);
        if (bottomBar != null) SetBarHeight(bottomBar, targetHeight);
    }
}
