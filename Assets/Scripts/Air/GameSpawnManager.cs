using UnityEngine;
public static class GameSpawnManager
{
    public static string NextSceneName = "";
    public static string SpawnPointName = "";
    // Vị trí spawn player khi load scene
    public static Vector3 PlayerSpawnPosition = Vector3.zero;

    // Hướng spawn player khi load scene
    public static Quaternion PlayerSpawnRotation = Quaternion.identity;

    // Flag xác định đã có vị trí spawn mới chưa
    public static bool HasSpawnPosition = false;
}
