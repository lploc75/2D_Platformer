using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Assets.Scripts.Earth.Common_Enemies;

public class HuskyCutscene : MonoBehaviour
{
    public Husky husky;
    public Transform player;
    public Transform playerBody;
    public GameObject magicStone;
    public string questIdToReady = "main_3_crystal";

    [Header("Cinematic Bars")]
    public RectTransform topBar;
    public RectTransform bottomBar;
    public float barSlideTime = 0.4f;
    public float barTargetHeight = 100f;

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

        if (topBar != null) SetBarHeight(topBar, 0f);
        if (bottomBar != null) SetBarHeight(bottomBar, 0f);
    }

    void Update()
    {
        if (!cutsceneStarted && husky != null && !IsHuskyAlive())
        {
            Debug.Log("<color=yellow>[Cutscene] Husky is dead. Starting cutscene.</color>");
            StartCoroutine(PlayStoneFlyCutscene());
        }
    }

    bool IsHuskyAlive()
    {
        var anim = husky.GetComponent<Animator>();
        if (anim == null)
        {
            Debug.LogError("Husky Animator not found!");
            return false;
        }
        bool alive = anim.GetBool(EnemiesAnimationStrings.isAlive);
        return alive;
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

        // Magic stone fly up
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

        // Quest mark ready
        if (QuestManager.Instance != null)
        {
            QuestManager.Instance.SetQuestReadyToComplete(questIdToReady);
        }

        // Save game nếu muốn
        if (GameSaveManager.Instance != null)
            GameSaveManager.Instance.SaveGame();

        // Slide Out Cinema Bars
        if (topBar != null && bottomBar != null)
            yield return StartCoroutine(SlideCinemaBars(0f, barSlideTime));

        // Enable player control
        if (input != null) input.enabled = true;
    }

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
