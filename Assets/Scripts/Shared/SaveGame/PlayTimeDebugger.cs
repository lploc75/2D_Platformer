using UnityEngine;

public class PlayTimeDebugger : MonoBehaviour
{
    public static PlayTimeDebugger Instance;
    public static float PlayTime = 0f;    // total playtime in seconds
    private bool isCounting = false;
    public float logInterval = 10f;
    private float logTimer = 0f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            PlayTime = 0f;
            logTimer = 0f;
            isCounting = false;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Tự động bắt đầu đếm nếu vừa NEW GAME
        if (PlayerPrefs.GetInt("IsNewGame", 0) == 1)
        {
            StartCounting();
            PlayerPrefs.SetInt("IsNewGame", 0); // reset flag
        }
    }

    void Update()
    {
        if (!isCounting) return;

        PlayTime += Time.deltaTime;

        if (logInterval > 0)
        {
            logTimer += Time.deltaTime;
            if (logTimer >= logInterval)
            {
                Debug.Log($"[PlayTimeDebugger] Play time: {PlayTime:F2} seconds");
                logTimer = 0f;
            }
        }
    }

    /// <summary>
    /// Start counting playtime from zero
    /// </summary>
    public void StartCounting()
    {
        PlayTime = 0f;
        isCounting = true;
        Debug.Log("<color=yellow>[PlayTimeDebugger] Counting play time started!</color>");
    }

    public void StopCounting()
    {
        isCounting = false;
        Debug.Log($"<color=yellow>[PlayTimeDebugger] Counting stopped at {PlayTime:F2} seconds</color>");
    }

    public static void LogPlayTime()
    {
        Debug.Log($"[PlayTimeDebugger] Play time (manual call): {PlayTime:F2} seconds");
    }
}
