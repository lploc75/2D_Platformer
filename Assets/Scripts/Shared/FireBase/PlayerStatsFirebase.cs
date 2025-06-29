using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Firebase.Extensions;

public static class PlayerStatsFirebase
{
    // Dùng UID mặc định để lưu theo người chơi cố định (không cần login)
    private static readonly string defaultUID = "guest";

    public static void SaveStats(PlayerStatsManager stats)
    {
        DatabaseReference dbRef = FirebaseDatabase.DefaultInstance
            .GetReference("users")
            .Child(defaultUID);

        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            { "currentSTR", stats.currentSTR },
            { "currentINT", stats.currentINT },
            { "currentDUR", stats.currentDUR },
            { "currentPER", stats.currentPER },
            { "currentVIT", stats.currentVIT },
            { "strLevel", stats.strLevel },
            { "intLevel", stats.intLevel },
            { "durLevel", stats.durLevel },
            { "perLevel", stats.perLevel },
            { "vitLevel", stats.vitLevel },
            { "totalPoint", stats.totalPoint }
        };

        dbRef.UpdateChildrenAsync(data).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompletedSuccessfully)
            {
                Debug.Log("✅ Đã lưu chỉ số lên Firebase (guest).");
            }
            else
            {
                Debug.LogError("❌ Lỗi lưu Firebase: " + task.Exception);
            }
        });
    }

    public static void LoadStats(System.Action<bool> callback)
    {
        DatabaseReference dbRef = FirebaseDatabase.DefaultInstance
            .GetReference("users")
            .Child(defaultUID);

        dbRef.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompletedSuccessfully)
            {
                DataSnapshot snapshot = task.Result;
                PlayerStatsManager stats = PlayerStatsManager.Instance;

                if (!snapshot.Exists)
                {
                    Debug.LogWarning("⚠️ Firebase chưa có dữ liệu. Tạo mới với giá trị mặc định.");
                    PlayerStatsFirebase.SaveStats(stats);
                    callback?.Invoke(true);
                    return;
                }

                stats.currentSTR = TryGetInt(snapshot, "currentSTR");
                stats.currentINT = TryGetInt(snapshot, "currentINT");
                stats.currentDUR = TryGetInt(snapshot, "currentDUR");
                stats.currentPER = TryGetInt(snapshot, "currentPER");
                stats.currentVIT = TryGetInt(snapshot, "currentVIT");

                stats.strLevel = TryGetInt(snapshot, "strLevel");
                stats.intLevel = TryGetInt(snapshot, "intLevel");
                stats.durLevel = TryGetInt(snapshot, "durLevel");
                stats.perLevel = TryGetInt(snapshot, "perLevel");
                stats.vitLevel = TryGetInt(snapshot, "vitLevel");

                stats.totalPoint = TryGetInt(snapshot, "totalPoint", 60);

                callback?.Invoke(true);
            }
            else
            {
                Debug.LogError("❌ Lỗi tải Firebase: " + task.Exception);
                callback?.Invoke(false);
            }
        });
    }

    // Hàm hỗ trợ đọc số nguyên an toàn từ Firebase
    private static int TryGetInt(DataSnapshot snapshot, string key, int defaultValue = 0)
    {
        try
        {
            var val = snapshot.Child(key).Value;
            return val != null ? int.Parse(val.ToString()) : defaultValue;
        }
        catch
        {
            Debug.LogWarning($"⚠️ Thiếu dữ liệu hoặc không hợp lệ: {key}, dùng mặc định {defaultValue}");
            return defaultValue;
        }
    }


}
