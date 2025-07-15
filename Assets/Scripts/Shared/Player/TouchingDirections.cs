using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
public class TouchingDirections : MonoBehaviour
{
    [Header("Cast settings")]
    public ContactFilter2D castFilter;
    public float groundDistance = 0.05f;
    public float wallDistance = 0.20f;
    public float ceilingDistance = 0.05f;

    // ✨ dùng Collider2D chung
    private Collider2D col;
    private Animator animator;

    // Buffer kết quả cast
    private readonly RaycastHit2D[] groundHits = new RaycastHit2D[5];
    private readonly RaycastHit2D[] wallHits = new RaycastHit2D[5];
    private readonly RaycastHit2D[] ceilingHits = new RaycastHit2D[5];

    /* ---------- Public trạng thái ---------- */
    [SerializeField] private bool _isGrounded;
    public bool IsGrounded
    {
        get => _isGrounded;
        private set { _isGrounded = value; animator.SetBool(AnimationStrings.isGrounded, value); }
    }

    [SerializeField] private bool _isOnWall;
    public bool IsOnWall
    {
        get => _isOnWall;
        private set { _isOnWall = value; animator.SetBool(AnimationStrings.isOnWall, value); }
    }

    public bool IsOnLeftWall { get; private set; }
    public bool IsOnRightWall { get; private set; }

    [SerializeField] private bool _isOnCeiling;
    public bool IsOnCeiling
    {
        get => _isOnCeiling;
        private set { _isOnCeiling = value; animator.SetBool(AnimationStrings.isOnCelling, value); }
    }

    /* ---------- Mono ---------- */
    private void Awake()
    {
        col = GetComponent<Collider2D>(); // có thể là Box hoặc Capsule
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        // Ground
        IsGrounded = col.Cast(Vector2.down, castFilter, groundHits, groundDistance) > 0;

        // Walls (trái/phải)
        IsOnLeftWall = col.Cast(Vector2.left, castFilter, wallHits, wallDistance) > 0;
        IsOnRightWall = col.Cast(Vector2.right, castFilter, wallHits, wallDistance) > 0;
        IsOnWall = IsOnLeftWall || IsOnRightWall;

        // Ceiling
        IsOnCeiling = col.Cast(Vector2.up, castFilter, ceilingHits, ceilingDistance) > 0;
    }
}
