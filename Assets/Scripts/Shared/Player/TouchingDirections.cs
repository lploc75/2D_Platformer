using UnityEngine;

public class TouchingDirections : MonoBehaviour
{
    public ContactFilter2D castFilter;
    public float groundDistance = 0.05f; // Khoảng cách kiểm tra mặt đất
    public float wallDistance = 0.05f; // khoảng cách kiểm tra tường

    CapsuleCollider2D touchingCol;
    Animator animator;

    RaycastHit2D[] groundHits = new RaycastHit2D[5];
    RaycastHit2D[] wallHits = new RaycastHit2D[5];

    [SerializeField]
    public bool _isGrounded = true;

    public bool IsGrounded
    {
        get
        {
            return _isGrounded;
        }

        private set
        {
            _isGrounded = value;
            animator.SetBool(AnimationStrings.isGrounded, value); // "IsGrounded"
        }
    }

    [SerializeField]
    private bool _isOnWall = false;

    public bool IsOnWall
    {
        get { return _isOnWall; }
        private set
        {
            _isOnWall = value;
            animator.SetBool(AnimationStrings.isOnWall, value); // "IsOnWall"
        }
    }
    public bool IsOnLeftWall { get; private set; }
    public bool IsOnRightWall { get; private set; }

    private void Awake()
    {
        touchingCol = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        IsGrounded = touchingCol.Cast(Vector2.down, castFilter, groundHits, groundDistance) > 0;

        // Kiểm tra tường trái hoặc phải
        IsOnLeftWall = touchingCol.Cast(Vector2.left, castFilter, wallHits, wallDistance) > 0;
        IsOnRightWall = touchingCol.Cast(Vector2.right, castFilter, wallHits, wallDistance) > 0;

        IsOnWall = IsOnLeftWall || IsOnRightWall;
    }
}
