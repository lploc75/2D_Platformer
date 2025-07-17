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

    public void RespawnPlayer()
    {
        if (player != null)
        {
            player.transform.position = lastCheckpointPosition;

            // Reset m�u, tr?ng th�i s?ng, animation
            var damageable = player.GetComponent<Damageable>();
            if (damageable != null)
            {
                damageable.Health = damageable.MaxHealth;
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
