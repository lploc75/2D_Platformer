using UnityEngine;

public class DeadZone : MonoBehaviour
{
    public int damage = 20; // S? m�u b? tr? khi r?t h?

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // D?ch chuy?n player v? checkpoint th�ng qua GameManager
            if (GameManager.Instance != null)
            {
                GameManager.Instance.RespawnPlayer();
            }

            // Sau ?� tr? m�u (player ?� "h?i sinh" l?i)
            Damageable damageable = other.GetComponent<Damageable>();
            if (damageable != null)
            {
                damageable.Hit(damage, Vector2.zero);
            }
        }
    }
}
