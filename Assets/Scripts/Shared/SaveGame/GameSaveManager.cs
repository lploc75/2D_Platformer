using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;
using Assets.Scripts.Data_concurrency;

[System.Serializable]
public class SpriteFadeStatus
{
    public string key;
    public bool faded;  // true nếu đã fade xong
}

[System.Serializable]
public class SpriteColorStatus
{
    public string key;
    public bool changedColor; // true nếu đã đổi màu xong
}

[System.Serializable]
public class GameSaveData
{
    public QuestManager.QuestSaveData questData;
    public List<string> watchedCutscenes = new List<string>();
    public List<SpriteFadeStatus> spriteFades = new List<SpriteFadeStatus>();
    public List<SpriteColorStatus> spriteColors = new List<SpriteColorStatus>();
    // Thêm các phần khác nếu cần
}

public class GameSaveManager : MonoBehaviour
{
    public static GameSaveManager Instance;

    private string saveFileName = "gamesave.json";
    private List<string> watchedCutscenes = new List<string>();

    // Lưu trạng thái từng SpriteFade/SpritesFadeToColor
    private Dictionary<string, bool> spriteFades = new Dictionary<string, bool>();
    private Dictionary<string, bool> spriteColors = new Dictionary<string, bool>();

    public event Action OnAfterSavePrompt;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Giữ lại manager khi load scene mới
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    // --- API cho SpriteFade, SpritesFadeToColor ---
    public void SetSpriteFadeStatus(string key, bool faded)
    {
        spriteFades[key] = faded;
    }
    public bool GetSpriteFadeStatus(string key)
    {
        return spriteFades.ContainsKey(key) && spriteFades[key];
    }
    public void SetSpriteColorStatus(string key, bool changed)
    {
        spriteColors[key] = changed;
    }
    public bool GetSpriteColorStatus(string key)
    {
        return spriteColors.ContainsKey(key) && spriteColors[key];
    }

    // --- MARK CUTSCENE ---
    public void MarkCutsceneWatched(string cutsceneId)
    {
        if (!watchedCutscenes.Contains(cutsceneId))
            watchedCutscenes.Add(cutsceneId);
    }
    public bool IsCutsceneWatched(string cutsceneId)
    {
        return watchedCutscenes.Contains(cutsceneId);
    }

    // SAVE GAME TO FILE
    public void SaveGame()
    {
        GameSaveData data = new GameSaveData();
        data.questData = QuestManager.Instance.GetSaveData();
        data.watchedCutscenes = new List<string>(watchedCutscenes);

        foreach (var kv in spriteFades)
            data.spriteFades.Add(new SpriteFadeStatus { key = kv.Key, faded = kv.Value });

        foreach (var kv in spriteColors)
            data.spriteColors.Add(new SpriteColorStatus { key = kv.Key, changedColor = kv.Value });

        string json = JsonUtility.ToJson(data, true);
        string path = Path.Combine(Application.persistentDataPath, saveFileName);
        File.WriteAllText(path, json);
        Debug.Log("[GameSaveManager] Đã save file tổng: " + path);
    }

    // LOAD GAME FROM FILE
    public void LoadGame()
    {
        string path = Path.Combine(Application.persistentDataPath, saveFileName);
        if (!File.Exists(path))
        {
            Debug.LogWarning("[GameSaveManager] Không tìm thấy file save tổng.");
            return;
        }

        string json = File.ReadAllText(path);
        GameSaveData data = JsonUtility.FromJson<GameSaveData>(json);
        QuestManager.Instance.LoadFromSaveData(data.questData);
        watchedCutscenes = data.watchedCutscenes ?? new List<string>();

        spriteFades.Clear();
        spriteColors.Clear();
        if (data.spriteFades != null)
            foreach (var s in data.spriteFades) spriteFades[s.key] = s.faded;
        if (data.spriteColors != null)
            foreach (var s in data.spriteColors) spriteColors[s.key] = s.changedColor;

        Debug.Log("[GameSaveManager] Đã load file tổng: " + path);
    }

    [ContextMenu("Print Save File Path")]
    public void PrintSaveFilePath()
    {
        string path = Path.Combine(Application.persistentDataPath, saveFileName);
        Debug.Log("File save path: " + path);
    }
    [ContextMenu("Open Save Folder")]
    public void OpenSaveFolder()
    {
        string path = Application.persistentDataPath;
        Debug.Log("Open folder: " + path);
        Application.OpenURL("file:///" + path);
    }

    // ----------- NEW GAME SUPPORT -----------
    public void StartNewGame(string sceneName)
    {
        DeleteAllSaveFiles();
        ResetRuntimeData();
        if (!string.IsNullOrEmpty(sceneName))
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        else
            Debug.LogError("❌ Chưa cấu hình sceneName để load!");
    }

    // ----------- PRIVATE HELPER FUNCTIONS -----------
    private void DeleteAllSaveFiles()
    {
        string dir = Application.persistentDataPath;

        string[] filesToDelete = new string[]
        {
            Path.Combine(dir, "gamesave.json"),
            Path.Combine(dir, "inventory_data.json"),
            Path.Combine(dir, "player_data.json"),
            Path.Combine(dir, "equip_status.json")
        };

        foreach (string filePath in filesToDelete)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                Debug.Log("[GameSaveManager] Đã xóa: " + filePath);
            }
            else
            {
                Debug.Log("[GameSaveManager] Không tìm thấy để xóa: " + filePath);
            }
        }
    }
    private void ResetRuntimeData()
    {
        // Reset Equipment
        if (EquipmentManager.Instance != null)
            EquipmentManager.Instance.ResetEquipment();

        // Reset Inventory
        if (InventoryManager.Instance != null)
            InventoryManager.Instance.ResetInventory();

        // Reset Currency
        if (CurrencyManager.Instance != null)
            CurrencyManager.Instance.ResetCurrency();

        // Reset Skill Tree
        if (SkillTreeManager.Instance != null)
            SkillTreeManager.Instance.ResetSkills();

        // Reset Player Stats
        if (PlayerStatsManager.Instance != null)
            PlayerStatsManager.Instance.ResetStats();

        // Reset watched cutscenes, sprite fades/colors trong chính GameSaveManager
        watchedCutscenes.Clear();
        spriteFades.Clear();
        spriteColors.Clear();

        Debug.Log("[GameSaveManager] Đã reset toàn bộ dữ liệu trong phiên chơi.");
    }
}
