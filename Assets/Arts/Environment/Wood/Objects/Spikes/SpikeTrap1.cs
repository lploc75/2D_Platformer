using UnityEngine;

public class SpikeTrap1 : MonoBehaviour
{
    public int damage = 10;           // L??ng m�u tr?
    public float knockbackForce = 10; // L?c v?ng v? ph�a sau

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Damageable damageable = collision.GetComponent<Damageable>();
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            if (damageable != null)
            {
                // X�c ??nh h??ng v?ng (t? spike sang player)
                Vector2 knockbackDir = (collision.transform.position - transform.position).normalized;
                knockbackDir.y = Mathf.Abs(knockbackDir.y); // ??m b?o v?ng l�n 1 �t n?u c?n

                // G�y s�t th??ng v� truy?n h??ng v?ng
                damageable.Hit(damage, knockbackDir * knockbackForce);

                // N?u Damageable.Hit ch?a x? l� knockback th� ??y ? ?�y
                if (rb != null)
                {
                    rb.linearVelocity = Vector2.zero; // Reset v?n t?c c?
                    rb.AddForce(knockbackDir * knockbackForce, ForceMode2D.Impulse);
                }
            }
        }
    }
}
