using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 4f;
    public float runSpeed = 8f;

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

    Vector2 moveInput;

    [SerializeField]
    private bool _IsMoving = false;
    public bool IsMoving { get 
        {
            return _IsMoving;
        }
        private set
        {
            _IsMoving = value;
            animator.SetBool("IsMoving", value);
        } 
    }

    [SerializeField]
    private bool _IsRunning = false;
    public bool IsRunning
    {
        get
        {
            return _IsRunning;
        }
        private set
        {
            _IsRunning = value;
            animator.SetBool("IsRunning", value);
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
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.linearVelocity.y); 
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
}
