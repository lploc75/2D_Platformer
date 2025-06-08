using UnityEngine;

public class PlayerSpawnManager : MonoBehaviour
{
    void Start()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
           
            return;
        }

       

        if (GameSpawnManager.HasSpawnPosition)
        {
            GameObject spawnObj = GameObject.Find(GameSpawnManager.SpawnPointName);
            if (spawnObj != null)
            {
                player.transform.position = spawnObj.transform.position;
                player.transform.rotation = spawnObj.transform.rotation;
              
            }
            else
            {
               
            }
            GameSpawnManager.HasSpawnPosition = false; // Reset flag
        }
        else
        {
            Debug.Log("[PlayerSpawnManager] Load scene không qua portal, giữ nguyên vị trí player.");
        }
    }

}
