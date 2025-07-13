using UnityEngine;

public class BossGate : MonoBehaviour
{
    [Header("Điểm spawn trong cùng scene")]
    [SerializeField] private Transform bossSpawnPoint;   // Kéo Empty (ReSpawn) vào đây
    [SerializeField] private Animator animator;         // Animator của cổng (tuỳ chọn)
    [SerializeField] private HugeMushroom boss;   // kéo Boss vào đây

    private bool playerInside;
    private Transform playerTf;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            playerTf = col.transform;
            playerInside = true;
            animator?.SetBool("IsPlayerNear", true);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            playerInside = false;
            animator?.SetBool("IsPlayerNear", false);
        }
    }

    void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.E) && bossSpawnPoint != null)
        {
            // Dịch chuyển Player đến vị trí Spawn trong cùng scene
            playerTf.position = bossSpawnPoint.position;

            // Bật đuổi
            boss?.ActivateChase();
            // (tuỳ ý) play FX, âm thanh, khoá điều khiển tạm, v.v.
        }
    }
}
