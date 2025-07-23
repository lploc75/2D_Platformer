using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Damage này được chỉnh theo từng phép + với base damage
    public int damage { get; private set; }
    public Vector2 moveSpeed = new Vector2(15f,0);
    public Vector2 knockback = new Vector2 (2,2);

    [SerializeField] private float lifetime = 1.2f;
    [SerializeField] private float destroyDelayAfterCollide = 0.2f; // cho clip kịp phát
    [SerializeField] private bool autoMove = true;

    [SerializeField] private BoxCollider2D lightingBoltCollider; // cái này cho riêng LightningBolt có thể tối ưu sau

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
        if (autoMove)
        {
            rb.linearVelocity = new Vector2(moveSpeed.x * transform.localScale.x, moveSpeed.y);
        }
        Destroy(gameObject, lifetime);
    }

    // Hàm khởi tạo mở rộng: damage, knockback, autoMove
    public void Init(int setDamage, Vector2 newKnockback, bool setAutoMove)
    {
        damage = setDamage;
        knockback = newKnockback;
        autoMove = setAutoMove;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageable = collision.GetComponent<Damageable>();

        if(damageable != null)
        {
            bool gotHit = damageable.Hit(damage, knockback);
            //bool gotHit = damageable.Hit(damage);
            Debug.Log("Damge và knockback từ Projectile " + damage + " -- " + knockback);
            if (gotHit)
            {
                Debug.Log(collision.name + "hit for " + damage);
                rb.linearVelocity = Vector2.zero;               // đứng lại
                rb.isKinematic = true;                    // ngừng vật lý
                GetComponent<Collider2D>().enabled = false; // không gây hit tiếp
                if (autoMove)
                {
                    animator.SetTrigger(AnimationStrings.collideTrigger);
                }
                Destroy(gameObject, destroyDelayAfterCollide);
            }

        }
    }
    public void DisableCollider()
    {
        if (lightingBoltCollider != null)
        {
            lightingBoltCollider.enabled = false;
            Debug.Log("❌ Collider đã bị tắt bởi animation");
        }
    }

    //public void SetColliderSize(Vector2 newSize)
    //{
    //    if (lightingBoltCollider != null)
    //    {
    //        lightingBoltCollider.size = newSize;
    //        Debug.Log($"✅ Đổi collider size thành {newSize}");
    //    }
    //}
    //public void SetColliderSize_Small()
    //{
    //    SetColliderSize(new Vector2(1f, 1f));
    //}

    //public void SetColliderSize_Medium()
    //{
    //    SetColliderSize(new Vector2(2f, 2f));
    //}

    //public void SetColliderSize_Large()
    //{
    //    SetColliderSize(new Vector2(3f, 3f));
    //}
}
