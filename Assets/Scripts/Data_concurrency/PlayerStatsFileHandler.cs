using UnityEngine;
using System.IO;
using Assets.Scripts.Data_concurrency;

public static class PlayerStatsFileHandler
{
    private static string SavePath => Path.Combine(Application.persistentDataPath, "player_data.json");

    public static void Save(PlayerStatsManager manager)
    {
        PlayerStatsDataModel data = new PlayerStatsDataModel
        {
            totalPoint = manager.totalPoint,
            currentSTR = manager.currentSTR,
            currentINT = manager.currentINT,
            currentDUR = manager.currentDUR,
            currentPER = manager.currentPER,
            currentVIT = manager.currentVIT,
            strLevel = manager.strLevel,
            intLevel = manager.intLevel,
            durLevel = manager.durLevel,
            perLevel = manager.perLevel,
            vitLevel = manager.vitLevel
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SavePath, json);
        Debug.Log("💾 Stats saved to " + SavePath);
    }

    public static bool Load(PlayerStatsManager manager)
    {
        if (!File.Exists(SavePath))
        {
            Debug.Log("⚠️ No save file found.");
            return false;
        }

        string json = File.ReadAllText(SavePath);
        PlayerStatsDataModel data = JsonUtility.FromJson<PlayerStatsDataModel>(json);

        manager.totalPoint = data.totalPoint;
        manager.currentSTR = data.currentSTR;
        manager.currentINT = data.currentINT;
        manager.currentDUR = data.currentDUR;
        manager.currentPER = data.currentPER;
        manager.currentVIT = data.currentVIT;
        manager.strLevel = data.strLevel;
        manager.intLevel = data.intLevel;
        manager.durLevel = data.durLevel;
        manager.perLevel = data.perLevel;
        manager.vitLevel = data.vitLevel;

        Debug.Log("✅ Stats loaded from file.");
        return true;
    }
}
