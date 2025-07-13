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

    [Header("Soul Drop Settings")]
    public GameObject soulPrefab;
    public int soulCount = 1;                 // số soul muốn rớt
    public float soulSpread = 0.4f;           // phạm vi random vị trí soul
    [Range(0f, 1f)]
    public float soulDropChance = 1f;         // 1 = luôn rớt, <1 = % rớt

    private bool hasDroppedLoot = false;      // đảm bảo chỉ rớt 1 lần

    [Header("Moving Settings")]
    public float walkSpeed = 2f;
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

        if (!IsAlive && !hasDroppedLoot)
        {
            DropCoins();
            DropSouls();
            hasDroppedLoot = true;
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
    /* ───────────── Item‑Drop helpers ───────────── */
    void DropCoins()
    {
        for (int i = 0; i < coinCount; i++)
        {
            Vector3 pos = transform.position + (Vector3)new Vector2(
                UnityEngine.Random.Range(-coinSpread, coinSpread), 0.5f);

            SpawnWithImpulse(coinPrefab, pos);
        }
    }
    void DropSouls()
    {
        if (soulPrefab == null) return;                          // quên gán Prefab
        if (UnityEngine.Random.value > soulDropChance) return;   // rớt theo % 

        for (int i = 0; i < soulCount; i++)
        {
            Vector3 pos = transform.position + (Vector3)new Vector2(
                UnityEngine.Random.Range(-soulSpread, soulSpread), 0.6f);

            SpawnWithImpulse(soulPrefab, pos);
        }
    }
    /* ───────────── tiện ích chung ───────────── */
    void SpawnWithImpulse(GameObject prefab, Vector3 position)
    {
        GameObject obj = Instantiate(prefab, position, Quaternion.identity);

        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(new Vector2(
                UnityEngine.Random.Range(-2f, 2f),              // X force
                UnityEngine.Random.Range(2f, 4f)),              // Y force
                ForceMode2D.Impulse);
        }
    }
}
