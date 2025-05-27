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
        rb.linearVelocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.linearVelocity.y);

        animator.SetFloat(AnimationStrings.yVelocity, rb.linearVelocity.y);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        IsMoving = moveInput != Vector2.zero;

        SetFacingDirection(moveInput);
    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if(moveInput.x > 0 && !IsFacingRight)
        {
            // face the right
            IsFacingRight = true;

        }else if (moveInput.x < 0 && IsFacingRight)
        {
            // face the left
            IsFacingRight= false;
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
            if (touchingDirections.IsGrounded)
            {
                // Nhảy lần đầu
                animator.SetTrigger(AnimationStrings.jump);
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpImpulse);
                canDoubleJump = true;  // Cho phép nhảy thêm 1 lần nữa
            }
            else if (canDoubleJump)
            {
                // Nhảy lần 2 (double jump)
                animator.SetTrigger(AnimationStrings.jump);
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpImpulse);
                canDoubleJump = false;  // Đã dùng double jump rồi, khóa lại
            }
            else if (canDoubleJump == true && !touchingDirections.IsGrounded) // Trên không cho 1 lần nhảy
            {
                animator.SetTrigger(AnimationStrings.jump);
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpImpulse);
                canDoubleJump = false; // Khóa nhảy cho đến khi chạm đất
            }
            
        }
    }

}
