using System;
using System.Collections;
using Assets.Scripts.Earth.Common_Enemies;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]
public class BigMushroom : MonoBehaviour
{
    public float walkSpeed = 4f;
    private bool canFlip = true;
    public float flipCooldown = 0.2f;

    Rigidbody2D rb;
    Animator animator;
    TouchingDirections touchingDirections;
    public DetectionZone attackZone;
    public enum WalkableDirection {Right, Left }
    private WalkableDirection _walkDireciton;
    private Vector2 walkDirectionVector = Vector2.right;

    public bool _hasTarget = false;
    public bool HasTarget
    {
        get { return _hasTarget; }
        private set
        {
            _hasTarget = value;
            animator.SetBool(EnemiesAnimationStrings.hasTarget, value);
        }
    }
    public bool CanMove
    {
        get 
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }
    public bool IsAlive
    {
        get { return animator.GetBool(AnimationStrings.isAlive); }
    }

    public WalkableDirection WalkDirection
    {
        get { return _walkDireciton; }
        set {
            if(_walkDireciton != value)
            {
                // Đổi hướng
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1,
                    gameObject.transform.localScale.y);
                if(value == WalkableDirection.Right)
                {
                    walkDirectionVector = Vector2.right;
                }else if (value == WalkableDirection.Left)
                {
                    walkDirectionVector = Vector2.left;
                }
            }
            _walkDireciton = value; }
    }

    private IEnumerator FlipCooldown()
    {
        canFlip = false;
        yield return new WaitForSeconds(flipCooldown);
        canFlip = true;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirections>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (touchingDirections.IsGrounded && touchingDirections.IsOnWall && canFlip)
        {
            FlipDirection();
            StartCoroutine(FlipCooldown());
        }
        if (CanMove && IsAlive)
        {
            rb.linearVelocity = new Vector2(walkSpeed * walkDirectionVector.x, rb.linearVelocity.y);
        }else
        {
            rb.linearVelocity= Vector2.zero;
        }

    }

    private void FlipDirection()
    {
        if(WalkDirection == WalkableDirection.Right)
        {
            WalkDirection = WalkableDirection.Left;
        }
        else if(WalkDirection == WalkableDirection.Left)
        {
            WalkDirection = WalkableDirection.Right;
        }else
        {
            Debug.Log("Current walkable direction is not set to legal value of right or left");
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //void Start()
    //{
        
    //}

    // Update is called once per frame
    void Update()
    {
        HasTarget = attackZone.detectedColliders.Count > 0;
    }
}
