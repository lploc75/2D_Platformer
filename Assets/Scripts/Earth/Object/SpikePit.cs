using UnityEngine;

public class SpikePit : MonoBehaviour
{
    public Transform respawnPoint;   // Kéo điểm hồi sinh vào đây
    public int damage = 10;          // Lượng máu trừ

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Damageable damageable = collision.GetComponent<Damageable>();
            if (damageable != null)
            {
                // Gây sát thương
                damageable.Hit(damage, Vector2.zero);
            }

            // Dịch chuyển người chơi về điểm spawn
            collision.transform.position = respawnPoint.position;

            // Nếu có Rigidbody2D thì reset velocity
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
            }
        }
    }
}
