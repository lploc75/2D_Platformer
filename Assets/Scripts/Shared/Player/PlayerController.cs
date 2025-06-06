using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]
public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 4f;
    public float runSpeed = 8f;
    public float jumpImpulse = 10f;
    private bool canDoubleJump;
    
    Vector2 moveInput;
    TouchingDirections touchingDirections;
    public float CurrentMoveSpeed { get
        {
            if (CanMove)
            {
                if (IsMoving)
                {
                    if (IsRunning)
                    {
                        return runSpeed;
                    }
                    else
                    {
                        return walkSpeed;
                    }
                }
                else
                {
                    // idle speed is 0
                    return 0;
                }
            }
            else
            {
                return 0; // khóa di chuyển
            }
           
        }
    }

    [SerializeField]
    private bool _isMoving = false;
    public bool IsMoving { get 
        {
            return _isMoving;
        }
        private set
        {
            _isMoving = value;
            animator.SetBool(AnimationStrings.isMoving, value);
        } 
    }

    [SerializeField]
    private bool _isRunning = false;
    public bool IsRunning
    {
        get
        {
            return _isRunning;
        }
        private set
        {
            _isRunning = value;
            animator.SetBool(AnimationStrings.isRunning, value);
        }
    }
    public bool _isFacingRight = true;
    public bool IsFacingRight { get { return _isFacingRight; } private set
        {
            if (_isFacingRight != value)
            {
                transform.localScale *= new Vector2(-1,1);
            }
            _isFacingRight = value;
        }
    }
    public bool CanMove
    {
        get { return animator.GetBool(AnimationStrings.canMove); }
    }
    public bool IsAlive
    {
        get { return animator.GetBool(AnimationStrings.isAlive); }
    }

    public bool LockVelocity
    {
        get
        {
            return animator.GetBool(AnimationStrings.lockVelocity);
        }
        set
        {
            animator.SetBool (AnimationStrings.lockVelocity, value);
        }
    }

    Rigidbody2D rb; 
    Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); 
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
    }

    private void FixedUpdate()
    {
        float horizontalVelocity = moveInput.x * CurrentMoveSpeed;
        float verticalVelocity = rb.linearVelocity.y;

        // Nếu đang va vào tường và cố gắng đi về phía tường thì không đẩy vào nữa
        if (touchingDirections.IsOnWall &&
            ((moveInput.x > 0 && IsFacingRight) || (moveInput.x < 0 && !IsFacingRight)))
        {
            horizontalVelocity = 0;
        }

        if (touchingDirections.IsGrounded)
        {
            canDoubleJump = true;  // Cho phép nhảy thêm 1 lần nữa
        }

        // Nếu đang chạm tường và không đứng dưới đất -> trượt tường
        if (touchingDirections.IsOnWall && !touchingDirections.IsGrounded)
        {
            verticalVelocity = -2f;
        }

        if(!LockVelocity)
            rb.linearVelocity = new Vector2(horizontalVelocity, verticalVelocity);

        animator.SetFloat(AnimationStrings.yVelocity, rb.linearVelocity.y);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        if (IsAlive)
        {
            IsMoving = moveInput != Vector2.zero;

            SetFacingDirection(moveInput);
        }else
        {
            IsMoving=false;
        }
    }

    private void SetFacingDirection(Vector2 moveInput)
    {

        if (moveInput.x > 0 && !IsFacingRight)
        {
            IsFacingRight = true;
        }else if (moveInput.x < 0 && IsFacingRight)
        {
            IsFacingRight = false;
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsRunning = true;
        }else if (context.canceled)
        {
            IsRunning = false;
        }
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (touchingDirections.IsGrounded && CanMove && IsAlive)
            {
                // Nhảy lần đầu
                //animator.SetTrigger(AnimationStrings.jumpTrigger);
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpImpulse);
                canDoubleJump = true;  // Cho phép nhảy thêm 1 lần nữa
            }
            else if (canDoubleJump && CanMove && IsAlive)
            {
                // Nhảy lần 2 (double jump)
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpImpulse);
                canDoubleJump = false;  // Đã dùng double jump rồi, khóa lại
            }
            else if (canDoubleJump == true && !touchingDirections.IsGrounded) // Trên không cho 1 lần nhảy
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpImpulse);
                canDoubleJump = false; // Khóa nhảy cho đến khi chạm đất
            }
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimationStrings.attackTrigger); 
        }
    }

    public void OnHit (int damage, Vector2 knockback)
    {
        LockVelocity = true;
        rb.linearVelocity = new Vector2(knockback.x, rb.linearVelocity.y + knockback.y);
        Debug.Log("onhit");
    }

    public void OnSelectSkill1(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetInteger(AnimationStrings.AttackIndex, 1); // 1 == default (attack animation 3)
        }
    }
    public void OnSelectSkill2(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetInteger(AnimationStrings.AttackIndex, 2); // 2 ==  attack animation ?
        }
    }
    public void OnSelectSkill3(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetInteger(AnimationStrings.AttackIndex, 3); // 3 ==  attack animation ?
        }
    }
}

