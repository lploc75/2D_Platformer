using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class Portal : MonoBehaviour
{
    public string sceneToLoad;
    public string spawnPointName;
    public Image fadeImage;
    public float fadeDuration = 1f;

    private bool isTransitioning = false;
    private bool playerInRange = false;
    private Collider2D playerCollider;

    private void Update()
    {
        // Nhấn E khi đang trong vùng portal mới chuyển scene
        if (playerInRange && !isTransitioning && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(Transition());
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            playerCollider = other;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            playerCollider = null;
        }
    }

    private IEnumerator Transition()
    {
        isTransitioning = true;
        Debug.Log($"[Portal] Chuyển scene: {sceneToLoad}, SpawnPointName: {spawnPointName}");

        GameSpawnManager.NextSceneName = sceneToLoad;
        GameSpawnManager.SpawnPointName = spawnPointName;
        GameSpawnManager.HasSpawnPosition = true;

        yield return StartCoroutine(Fade(0f, 1f, fadeDuration));

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad);
        while (!asyncLoad.isDone)
            yield return null;

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
