using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private Vector3 lastCheckpointPosition;
    [HideInInspector] public GameObject player;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Gi? qua c�c scene
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // T�m player l�c b?t ??u game
        player = GameObject.FindGameObjectWithTag("Player");
        // Kh?i t?o checkpoint m?c ??nh l� v? tr� spawn ban ??u
        if (player != null)
            lastCheckpointPosition = player.transform.position;
    }

    public void SetCheckpoint(Vector3 checkpointPos)
    {
        lastCheckpointPosition = checkpointPos;
    }

    public void RespawnPlayer() // Ch? d?ch chuy?n, kh�ng h?i m�u
    {
        if (player != null)
        {
            player.transform.position = lastCheckpointPosition;
            var damageable = player.GetComponent<Damageable>();
            if (damageable != null)
            {
                damageable.IsAlive = true;
            }

            var pc = player.GetComponent<PlayerController>();
            if (pc != null)
            {
                pc.ResetActionState();
            }
        }
    }
    public void RevivePlayerAtCheckpoint()
    {
        if (player != null)
        {
            player.transform.position = lastCheckpointPosition;

            var damageable = player.GetComponent<Damageable>();
            if (damageable != null)
            {
                damageable.Health = damageable.MaxHealth; // H?i m�u!
                damageable.IsAlive = true;
            }

            var pc = player.GetComponent<PlayerController>();
            if (pc != null)
            {
                pc.ResetActionState();
            }
        }
    }

}
