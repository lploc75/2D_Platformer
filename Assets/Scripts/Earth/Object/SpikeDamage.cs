using UnityEngine;

public class SpikeDamage : MonoBehaviour
{
    public int damage = 20;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Damageable dmg = collision.collider.GetComponent<Damageable>();
            if (dmg != null)
            {
                dmg.Hit(damage, Vector2.zero);
            }
        }
    }
}
