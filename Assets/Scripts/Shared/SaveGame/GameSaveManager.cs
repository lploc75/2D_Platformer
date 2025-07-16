using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;

[System.Serializable]
public class GameSaveData
{
    public QuestManager.QuestSaveData questData;
    public List<string> watchedCutscenes = new List<string>();
    // Thêm các phần khác nếu cần: inventoryData, playerData, ...
}

public class GameSaveManager : MonoBehaviour
{
    public static GameSaveManager Instance;

    private string saveFileName = "gamesave.json";
    private List<string> watchedCutscenes = new List<string>();

    private bool loadedOrSkipped = false; // Đảm bảo chỉ load/skip một lần

    // Event: bắn ra khi đã chọn L/N xong, để các hệ thống khác (ví dụ cutscene) lắng nghe
    public event Action OnAfterSavePrompt;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        Debug.Log("<color=yellow>Nhấn phím L để LOAD file save tổng, hoặc N để bỏ qua và chơi mới.</color>");
    }

    void Update()
    {
        if (!loadedOrSkipped)
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                LoadGame();
                Debug.Log("<color=cyan>Đã load file save tổng!</color>");
                loadedOrSkipped = true;
                OnAfterSavePrompt?.Invoke();
            }
            else if (Input.GetKeyDown(KeyCode.N))
            {
                Debug.Log("<color=yellow>Bạn đã chọn bỏ qua (không load file save).</color>");
                loadedOrSkipped = true;
                OnAfterSavePrompt?.Invoke();
            }
        }
    }

    // MARK CUTSCENE AS WATCHED
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

        // Lấy dữ liệu quest
        data.questData = QuestManager.Instance.GetSaveData();

        // Lấy cutscene đã xem
        data.watchedCutscenes = new List<string>(watchedCutscenes);

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

        // Load dữ liệu quest
        QuestManager.Instance.LoadFromSaveData(data.questData);

        // Load cutscene đã xem
        watchedCutscenes = data.watchedCutscenes ?? new List<string>();

        Debug.Log("[GameSaveManager] Đã load file tổng: " + path);
    }

    // TEST: In path file save
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
}
