using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerStatsManager : MonoBehaviour
{
    public static PlayerStatsManager Instance { get; private set; }

    // Stats cơ bản
    public int STR = 10;
    public int INT = 10;
    public int DUR = 10;
    public int PER = 10;
    public int VIT = 10;

    // Tham chiếu UI (kéo vào Inspector)
    public TMP_Text strText;
    public TMP_Text intText;
    public TMP_Text durText;
    public TMP_Text perText;
    public TMP_Text vitText;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateAllStatsUI();
    }

    public void AddStat(string statType, int amount)
    {
        switch (statType)
        {
            case "STR": STR += amount; break;
            case "INT": INT += amount; break;
            case "DUR": DUR += amount; break;
            case "PER": PER += amount; break;
            case "VIT": VIT += amount; break;
        }
        UpdateAllStatsUI();
    }

    public void UpdateAllStatsUI()
    {
        if (strText != null) strText.text = STR.ToString();
        if (intText != null) intText.text = INT.ToString();
        if (durText != null) durText.text = DUR.ToString();
        if (perText != null) perText.text = PER.ToString();
        if (vitText != null) vitText.text = VIT.ToString();
    }
}
