using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage = 10;
    public Vector2 moveSpeed = new Vector2(10f,0);
    public Vector2 knockback = new Vector2 (0,0);

    Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb.linearVelocity = new Vector2 (moveSpeed.x * transform.localScale.x , moveSpeed.y); 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageable = collision.GetComponent<Damageable>();

        if(damageable != null)
        {
            Vector2 deliveredKnockback = transform.parent.localScale.x > 0 ? 
                knockback : new Vector2(-knockback.x, knockback.y);
            //bool gotHit = damageable.Hit(damage, deliveredKnockback);
            //bool gotHit = damageable.Hit(damage);

            //if (gotHit) {
            //    Debug.Log(collision.name + "hit for " + damge); 
            //}

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
