﻿using System;
using Assets.Scripts.Shared.Player;
using Assets.Scripts.Shared.Skill;
using UnityEngine;
using UnityEngine.InputSystem;
/*
 Máu của nhận vật được set ở bên Damageable
 */
[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))]
public class PlayerController : MonoBehaviour
{
    [Header("Điều khiển")]
    public bool canControl = true;
    public float walkSpeed = 4f;
    public float runSpeed = 8f;
    public float jumpImpulse = 10f;
    private bool canDoubleJump;
    private AudioSource audioSource;

    private float manaCost = 1f; // mana của skill default
    [Header("Skill")]
    public SkillData[] skills; // Set từ Inspector, 0 = Skill1, 1 = Skill2, 2 = Skill3...
    private SkillData currentSkillData; // Skill hiện tại được lựa chọn

    [Header("Manager tham chiếu")]
    public ManaManager manaManager;
    public StaminaManager staminaManager;

    [SerializeField]
    private ProjectileLauncher projectileLauncher;

    Vector2 moveInput;
    public TouchingDirections touchingDirections;
    Damageable damageable;
    public float CurrentMoveSpeed
    {
        get
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
    public bool IsMoving
    {
        get
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
    public bool IsFacingRight
    {
        get { return _isFacingRight; }
        private set
        {
            if (_isFacingRight != value)
            {
                transform.localScale *= new Vector2(-1, 1);
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

    Rigidbody2D rb;
    Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
        damageable = GetComponent<Damageable>();
    }
    private void Start()
    {
        currentSkillData = skills[0]; // Mặc định chọn skill đầu tiên
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

        //// Nếu đang chạm tường và không đứng dưới đất -> trượt tường
        //if (touchingDirections.IsOnWall && !touchingDirections.IsGrounded)
        //{
        //    verticalVelocity = -2f;
        //}

        if (IsRunning && staminaManager.HasStamina(1f))
        {
            staminaManager.SetUsingStamina(true);
        }
        else
        {
            IsRunning = false;
            staminaManager.SetUsingStamina(false);
        }

        if (!damageable.LockVelocity)
            rb.linearVelocity = new Vector2(horizontalVelocity, verticalVelocity);

        animator.SetFloat(AnimationStrings.yVelocity, rb.linearVelocity.y);
    }

    public void OnMove(InputAction.CallbackContext context)
    {

        if (!canControl) return; // <- THÊM DÒNG NÀY
        moveInput = context.ReadValue<Vector2>();

        if (IsAlive)
        {
            IsMoving = moveInput != Vector2.zero;

            SetFacingDirection(moveInput);
        }
        else
        {
            IsMoving = false;
        }
    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !IsFacingRight)
        {
            IsFacingRight = true;
        }
        else if (moveInput.x < 0 && IsFacingRight)
        {
            IsFacingRight = false;
        }
    }


    public void OnRun(InputAction.CallbackContext context)
    {
        if (!canControl || staminaManager.currentStamina <= 0f) return;

        if (context.started)
        {
            if (staminaManager.HasStamina(1f)) // chỉ cần có một ít
            {
                IsRunning = true;
            }
        }
        else if (context.canceled)
        {
            IsRunning = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!canControl) return; // <- THÊM DÒNG NÀY
        if (context.started)
        {
            if (touchingDirections.IsGrounded && CanMove && IsAlive)
            {
                // Nhảy lần đầu
                //animator.SetTrigger(AnimationStrings.jumpTrigger);
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpImpulse);
                canDoubleJump = true;
            }
            else if (canDoubleJump && CanMove && IsAlive)
            {
                // Nhảy lần 2 (double jump)
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpImpulse);
                canDoubleJump = false;
            }
            else if (canDoubleJump == true && !touchingDirections.IsGrounded)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpImpulse);
                canDoubleJump = false;
            }
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started && manaManager != null)
        {
            if (manaManager.ConsumeMana(manaCost))
            {
                // Gọi hàm bắn vật thể được đặt cố định trong animation
                animator.SetTrigger(AnimationStrings.attackTrigger);
                // Truyền sẵn dữ liệu cho Launcher, đợi animation gọi FireProjectile()

                projectileLauncher.SetSkillData(currentSkillData);
                // Phát SFX ngay – hoặc để Animation Event gọi cũng được
                PlaySkillSFX(currentSkillData);
            }
        }
    }

    // Bị tấn công -> nhận damage và knockback
    public void OnHit(int damage, Vector2 knockback)
    {
        rb.linearVelocity = new Vector2(knockback.x, rb.linearVelocity.y + knockback.y);
        Debug.Log("onhit");
    }
    public void OnSelectSkill1(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            currentSkillData = skills[0];
            manaCost = currentSkillData.manaCost;
            animator.SetInteger(AnimationStrings.AttackIndex, currentSkillData.animationIndex);
            animator.SetInteger(AnimationStrings.SkillID, currentSkillData.skillID);
        }
    }
    public void OnSelectSkill2(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            currentSkillData = skills[1];
            manaCost = currentSkillData.manaCost;
            animator.SetInteger(AnimationStrings.AttackIndex, currentSkillData.animationIndex);
            animator.SetInteger(AnimationStrings.SkillID, currentSkillData.skillID);
        }
    }
    public void OnSelectSkill3(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            currentSkillData = skills[2];
            manaCost = currentSkillData.manaCost;
            animator.SetInteger(AnimationStrings.AttackIndex, currentSkillData.animationIndex);
            animator.SetInteger(AnimationStrings.SkillID, currentSkillData.skillID);
        }
    }
    public void OnSelectSkill4(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            currentSkillData = skills[3];
            manaCost = currentSkillData.manaCost;
            animator.SetInteger(AnimationStrings.AttackIndex, currentSkillData.animationIndex);
            animator.SetInteger(AnimationStrings.SkillID, currentSkillData.skillID);
        }
    }
    void PlaySkillSFX(SkillData data)
    {
        if (data.shootSFX == null) return;

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();

        audioSource.PlayOneShot(data.shootSFX);
    }


}


