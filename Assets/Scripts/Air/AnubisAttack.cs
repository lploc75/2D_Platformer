using UnityEngine;

public class MonsterPatrolAndChase2D : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb2d;

    public string playerTag = "Player";

    // Giới hạn vùng patrol trên trục X
    public float patrolMinX;  // biên trái
    public float patrolMaxX;  // biên phải

    public float patrolSpeed = 2f;
    public float chaseSpeed = 3.5f;

    public float attackDistance = 1f;  // khoảng cách cách player để đứng đánh
    public float attackCooldown = 2f;

    private int moveDirection = 1; // 1: đi phải, -1: đi trái

    private float lastAttackTime;
    private bool playerInRange = false;
    private Transform playerTransform;

    private bool isAttacking = false;

    private float fixedY; // giữ y cố định

    void Awake()
    {
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();

        // Khóa trục Y và xoay để giữ vị trí đứng yên và không bị rơi
        rb2d.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;

        lastAttackTime = -attackCooldown;

        fixedY = rb2d.position.y;  // lưu y ban đầu, giữ cố định
    }

    void Update()
    {
        if (playerInRange && playerTransform != null)
        {
            float distanceToPlayer = Mathf.Abs(playerTransform.position.x - transform.position.x);

            if (distanceToPlayer > attackDistance && !isAttacking)
            {
                // Chưa đến vị trí attack, tiếp tục chạy gần player
                ChasePlayer();
            }
            else
            {
                // Đã đủ gần, dừng lại và chuẩn bị đánh
                rb2d.linearVelocity = Vector2.zero;
                animator.SetBool("isWalking", false);

                if (!isAttacking && Time.time - lastAttackTime >= attackCooldown)
                {
                    Attack();
                }
            }
        }
        else
        {
            // Không phát hiện player, reset trạng thái và đi tuần tra
            isAttacking = false;
            Patrol();
        }
    }

    void Patrol()
    {
        animator.SetBool("isWalking", true);

        float newX = rb2d.position.x + moveDirection * patrolSpeed * Time.deltaTime;

        if (newX > patrolMaxX)
        {
            newX = patrolMaxX;
            moveDirection = -1;
        }
        else if (newX < patrolMinX)
        {
            newX = patrolMinX;
            moveDirection = 1;
        }

        rb2d.MovePosition(new Vector2(newX, fixedY));
        FlipSprite(moveDirection);
    }

    void ChasePlayer()
    {
        animator.SetBool("isWalking", true);

        float direction = playerTransform.position.x > transform.position.x ? 1f : -1f;

        // Vị trí mục tiêu cách player 1 khoảng attackDistance
        float targetX = playerTransform.position.x - direction * attackDistance;

        // Giới hạn vị trí trong patrol
        targetX = Mathf.Clamp(targetX, patrolMinX, patrolMaxX);

        // Di chuyển từ từ tới targetX, tránh rung lắc
        float newX = Mathf.MoveTowards(rb2d.position.x, targetX, chaseSpeed * Time.deltaTime);

        rb2d.MovePosition(new Vector2(newX, fixedY));
        FlipSprite(direction);

    }

    void Attack()
    {
        isAttacking = true;
        lastAttackTime = Time.time;

        animator.SetTrigger("AttackTrigger");
    }

    // Gọi từ animation event khi attack kết thúc
    public void OnAttackFinished()
    {
        isAttacking = false;
    }

    void FlipSprite(float dir)
    {
        Vector3 scale = transform.localScale;
        if (dir > 0)
            scale.x = -Mathf.Abs(scale.x);    // quay mặt phải
        else if (dir < 0)
            scale.x = Mathf.Abs(scale.x);     // quay mặt trái
        transform.localScale = scale;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInRange = true;
            playerTransform = other.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInRange = false;
            playerTransform = null;

            animator.SetBool("isWalking", false);

            // Reset hướng di chuyển patrol để quái đi theo hướng cố định, tránh rung lắc
            moveDirection = 1;  // hoặc giá trị bạn muốn mặc định

        }
    }

}
