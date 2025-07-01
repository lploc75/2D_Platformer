using System;
using System.Collections;
using Assets.Scripts.Earth.Common_Enemies;
using UnityEngine;
using UnityEngine.XR;
using static UnityEngine.Rendering.DebugUI;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))]
public class BigMushroom : MonoBehaviour
{
    [Header("Coin Drop Settings")]
    public GameObject coinPrefab; // Gán từ Inspector
    public int coinCount = 1; // Số lượng coin rớt
    public float coinSpread = 0.5f; // Phạm vi random vị trí rớt
    private bool hasDroppedCoin = false;

    [Header("Moving Settings")]
    public float walkSpeed = 4f;
    private bool canFlip = true;
    public float flipCooldown = 0.2f;

    Rigidbody2D rb;
    Animator animator;
    Damageable damageable;
    TouchingDirections touchingDirections;
    public DetectionZone attackZone;
    public DetectionZone cliffDetectionzone;
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
        damageable = GetComponent<Damageable>();
    }
    void Update()
    {
        HasTarget = attackZone.detectedColliders.Count > 0;

        if (!IsAlive && !hasDroppedCoin)
        {
            DropCoins();
            hasDroppedCoin = true;
        }
    }
    private void FixedUpdate()
    {
        if (touchingDirections.IsGrounded && touchingDirections.IsOnWall && canFlip)
        {
            FlipDirection();
            StartCoroutine(FlipCooldown());
        }
        if (!damageable.LockVelocity)
        {
            if (CanMove && IsAlive && touchingDirections.IsGrounded)
            {
                rb.linearVelocity = new Vector2(walkSpeed * walkDirectionVector.x,
                    rb.linearVelocity.y);
            }
            else
            {
                rb.linearVelocity = Vector2.zero;
            }
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

    public void OnHit(int damage, Vector2 knockback)
    {
        rb.linearVelocity = new Vector2(knockback.x, rb.linearVelocity.y + knockback.y);
    }

    public void OnCliffDetected()
    {
        if (touchingDirections.IsGrounded)
        {
            FlipDirection();
        }
    }
    void DropCoins()
    {
        for (int i = 0; i < coinCount; i++)
        {
            Vector2 dropOffset = new Vector2(UnityEngine.Random.Range(-coinSpread, coinSpread), 0.5f);
            Vector3 dropPosition = transform.position + (Vector3)dropOffset;

            GameObject coin = Instantiate(coinPrefab, dropPosition, Quaternion.identity);

            // Coin bay nhẹ ra ngoài
            Rigidbody2D rb = coin.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                float forceX = UnityEngine.Random.Range(-2f, 2f);
                float forceY = UnityEngine.Random.Range(2f, 4f);
                rb.AddForce(new Vector2(forceX, forceY), ForceMode2D.Impulse);
            }
        }
    }

}
