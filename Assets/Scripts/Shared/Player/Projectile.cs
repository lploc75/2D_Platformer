using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage = 10;
    public Vector2 moveSpeed = new Vector2(15f,0);
    public Vector2 knockback = new Vector2 (2,2);

    [SerializeField] private float lifetime = 1.4f;

    Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb.linearVelocity = new Vector2 (moveSpeed.x * transform.localScale.x , moveSpeed.y);
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageable = collision.GetComponent<Damageable>();

        if(damageable != null)
        {
            Vector2 deliveredKnockback = transform.localScale.x > 0 ? 
                knockback : new Vector2(-knockback.x, knockback.y);
            bool gotHit = damageable.Hit(damage, deliveredKnockback);
            //bool gotHit = damageable.Hit(damage);

            if (gotHit)
            {
                Debug.Log(collision.name + "hit for " + damage);
                Destroy(gameObject); //
            }

        }
    }

}
