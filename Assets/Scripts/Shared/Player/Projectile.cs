using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Damage này được chỉnh theo từng phép + với base damage
    public int damage { get; private set; }      // bỏ “=10” mặc định
    public Vector2 moveSpeed = new Vector2(15f,0);
    public Vector2 knockback = new Vector2 (2,2);

    [SerializeField] private float lifetime = 1.4f;
    [SerializeField] private float destroyDelayAfterCollide = 0.2f; // cho clip kịp phát

    Rigidbody2D rb;
    private Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb.linearVelocity = new Vector2 (moveSpeed.x * transform.localScale.x , moveSpeed.y);
        Destroy(gameObject, lifetime);
    }

    // Hàm khởi tạo (setter) – gọi ngay sau Instantiate
    public void Init(int setDamage, Vector2 newKnockback)
    {
        damage = setDamage;
        knockback = newKnockback;
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
            Debug.Log("Damge và knockback từ Projectile " + damage + " -- " + deliveredKnockback);
            if (gotHit)
            {
                Debug.Log(collision.name + "hit for " + damage);
                rb.linearVelocity = Vector2.zero;               // đứng lại
                rb.isKinematic = true;                    // ngừng vật lý
                GetComponent<Collider2D>().enabled = false; // không gây hit tiếp
                animator.SetTrigger(AnimationStrings.collideTrigger);
                Destroy(gameObject, destroyDelayAfterCollide);
            }

        }
    }

}
