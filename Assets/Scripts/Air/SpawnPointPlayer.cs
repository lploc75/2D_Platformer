using UnityEngine;

public class PlayerSpawnManager : MonoBehaviour
{
    void Start()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("[PlayerSpawnManager] Không tìm thấy player trong scene!");
            return;
        }

        Debug.Log($"[PlayerSpawnManager] HasSpawnPosition: {GameSpawnManager.HasSpawnPosition}, SpawnPointName: {GameSpawnManager.SpawnPointName}");

        if (GameSpawnManager.HasSpawnPosition)
        {
            GameObject spawnObj = GameObject.Find(GameSpawnManager.SpawnPointName);
            if (spawnObj != null)
            {
                player.transform.position = spawnObj.transform.position;
                player.transform.rotation = spawnObj.transform.rotation;
                Debug.Log($"[PlayerSpawnManager] Đặt player tại spawn point: {spawnObj.name}, vị trí: {player.transform.position}");
            }
            else
            {
                Debug.LogWarning($"[PlayerSpawnManager] Không tìm thấy spawn point: {GameSpawnManager.SpawnPointName}");
            }
            GameSpawnManager.HasSpawnPosition = false; // Reset flag
        }
        else
        {
            Debug.Log("[PlayerSpawnManager] Load scene không qua portal, giữ nguyên vị trí player.");
        }
    }

}
