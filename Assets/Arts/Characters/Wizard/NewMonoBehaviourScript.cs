using UnityEngine;

public class PlayerMove2 : MonoBehaviour
{
    public float moveSpeed = 3f;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    private float moveInput = 0f;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Nhận input từ bàn phím (trái/phải)
        moveInput = Input.GetAxisRaw("Horizontal");

        // Đảo chiều sprite nếu cần
        if (moveInput != 0)
            spriteRenderer.flipX = moveInput < 0;

        // Gửi giá trị Speed cho Animator để điều khiển Animation
        animator.SetFloat("Speed", Mathf.Abs(moveInput));
    }

    void FixedUpdate()
    {
        // Di chuyển vật lý (FixedUpdate mới chuẩn)
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }
}
