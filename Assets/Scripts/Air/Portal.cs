using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class Portal : MonoBehaviour
{
    public string sceneToLoad;          // Tên scene sẽ load (map đích)
    public string spawnPointName;       // Tên spawn point tương ứng trên map đích
    public Image fadeImage;             // Image dùng fade màn hình
    public float fadeDuration = 1f;

    private bool isTransitioning = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isTransitioning) return;

        if (other.CompareTag("Player"))
        {
            StartCoroutine(Transition());
        }
    }

    private IEnumerator Transition()
    {
        isTransitioning = true;
        // Lưu thông tin scene + spawn point cho scene mới
        Debug.Log($"[Portal] Chuyển scene: {sceneToLoad}, SpawnPointName: {spawnPointName}");

        // Lưu thông tin scene + spawn point cho scene mới
        GameSpawnManager.NextSceneName = sceneToLoad;
        GameSpawnManager.SpawnPointName = spawnPointName;
        GameSpawnManager.HasSpawnPosition = true;
        // Fade out
        yield return StartCoroutine(Fade(0f, 1f, fadeDuration));

        // Load scene async
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad);
        while (!asyncLoad.isDone)
            yield return null;

        // Fade in
        yield return StartCoroutine(Fade(1f, 0f, fadeDuration));

        isTransitioning = false;
    }

    IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;
        Color color = fadeImage.color;
        while (elapsed < duration)
        {
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            fadeImage.color = new Color(color.r, color.g, color.b, alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }
        fadeImage.color = new Color(color.r, color.g, color.b, endAlpha);
    }
}
