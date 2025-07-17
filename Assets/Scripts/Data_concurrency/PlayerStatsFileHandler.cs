using UnityEngine;
using System.IO;
using Assets.Scripts.Data_concurrency;
using System.Collections.Generic;

public static class PlayerStatsFileHandler
{
    private static string SavePath => Path.Combine(Application.persistentDataPath, "player_data.json");

    public static void Save(PlayerStatsManager manager)
    {
        PlayerStatsDataModel data = new PlayerStatsDataModel
        {
            // KHÔNG còn dòng totalPoint ở đây!
            currentSTR = manager.currentSTR,
            currentINT = manager.currentINT,
            currentDUR = manager.currentDUR,
            currentPER = manager.currentPER,
            currentVIT = manager.currentVIT,
            strLevel = manager.strLevel,
            intLevel = manager.intLevel,
            durLevel = manager.durLevel,
            perLevel = manager.perLevel,
            vitLevel = manager.vitLevel,

            // Tiền
            coin = CurrencyManager.Instance.GetCurrency(CurrencyType.Coin),
            gem = CurrencyManager.Instance.GetCurrency(CurrencyType.Gem),
            blueSoul = CurrencyManager.Instance.GetCurrency(CurrencyType.BlueSoul),
            purpleSoul = CurrencyManager.Instance.GetCurrency(CurrencyType.PurpleSoul),
             // Kỹ năng đã mở
            unlockedSkillIndices = new List<int>()
        };

        for (int i = 0; i < SkillTreeManager.Instance.skills.Count; i++)
        {
            if (SkillTreeManager.Instance.skills[i].isUnlocked)
            {
                data.unlockedSkillIndices.Add(i);
            }
        }
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

        // KHÔNG còn dòng manager.totalPoint = data.totalPoint;
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


        CurrencyManager.Instance.SetCurrency(CurrencyType.Coin, data.coin);
        CurrencyManager.Instance.SetCurrency(CurrencyType.Gem, data.gem);
        CurrencyManager.Instance.SetCurrency(CurrencyType.BlueSoul, data.blueSoul);
        CurrencyManager.Instance.SetCurrency(CurrencyType.PurpleSoul, data.purpleSoul);

        for (int i = 0; i < data.unlockedSkillIndices.Count; i++)
        {
            int idx = data.unlockedSkillIndices[i];
            if (idx >= 0 && idx < SkillTreeManager.Instance.skills.Count)
            {
                SkillTreeManager.Instance.skills[idx].isUnlocked = true;
            }
        }
        SkillTreeManager.Instance.UpdateUI(); // cập nhật giao diện
        Debug.Log("✅ Stats loaded from file.");
        return true;
    }
}
