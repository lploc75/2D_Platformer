using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public LayerMask groundLayer; // layer mặt đất
    public Transform groundCheck; // điểm kiểm tra dưới chân
    public float groundCheckRadius = 0.2f;

    private Rigidbody2D rb;
    private bool isGrounded;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }
    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal"); // A/D hoặc ←/→
        float moveY = Input.GetAxisRaw("Vertical");   // W/S hoặc ↑/↓

        Vector3 move = new Vector3(moveX, moveY, 0f);
        transform.position += move * moveSpeed * Time.deltaTime;
        // Kiểm tra player đang chạm đất chưa
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Nhấn phím Space để nhảy khi đang đứng trên đất
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }
}