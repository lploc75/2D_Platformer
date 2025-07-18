using System.Collections;
using UnityEngine;
using Assets.Scripts.Earth.Common_Enemies;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))]
public class HugeMushroom : MonoBehaviour
{
    /*=========== 1. SUMMON MINION ==================*/
    [Header("Summon")]
    [SerializeField] private GameObject minionPrefab;
    [SerializeField] private float summonOffsetY = 0.5f;

    private int lastSummonSegment = 10;   // 10 = 100–91 %
    private bool isSummoning = false;      // khoá khi đang chạy clip Summon

    /*=========== 2. CHASE PLAYER ===================*/
    [Header("Chase")]
    [SerializeField] private Transform player;
    [SerializeField] private float chaseSpeed = 2.5f;
    [SerializeField] private float stopDistance = 1.0f;
    [SerializeField] private bool canChase = false;

    /*=========== 3. FLIP WALL ======================*/
    [SerializeField] private float flipCooldown = 0.2f;
    private bool canFlip = true;

    /*=========== 4. COMPONENTS =====================*/
    Rigidbody2D rb;
    Animator animator;
    Damageable damageable;
    TouchingDirections touchingDirections;

    /*=========== 5. WALK STATE =====================*/
    public enum WalkDir { Right, Left }
    private WalkDir walkDir = WalkDir.Right;

    /*=========== 6. ANIM FLAGS =====================*/
    private bool _hasTarget;
    private bool HasTarget
    {
        get => _hasTarget;
        set { _hasTarget = value; animator.SetBool(EnemiesAnimationStrings.hasTarget, value); }
    }
    private bool CanMove => animator.GetBool(EnemiesAnimationStrings.canMove);
    private bool IsAlive => animator.GetBool(EnemiesAnimationStrings.isAlive);

    /*================ UNITY ========================*/
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();
        touchingDirections = GetComponent<TouchingDirections>();

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p) player = p.transform;
        }
    }

    /*================ UPDATE =======================*/
    private void Update()
    {
        TrySummon();           // kiểm tra triệu hồi
        UpdateHasTargetFlag(); // cờ Attack cho Animator
    }

    private void FixedUpdate()
    {
        ChasePlayer();         // di chuyển
    }

    /*=========== A. SUMMON LOGIC ===================*/
    private void TrySummon()
    {
        if (!IsAlive || isSummoning || minionPrefab == null) return;

        int currentSegment = damageable.Health * 10 / damageable.MaxHealth;
        if (currentSegment < lastSummonSegment)
        {
            animator.SetTrigger(EnemiesAnimationStrings.summonTrigger); // phát clip Summon
            isSummoning = true;    // khoá để chờ animation Event
            lastSummonSegment = currentSegment;
        }
    }

    public void SpawnMinionEvent()
    {
        float direction = transform.localScale.x > 0 ? 1 : -1;
        Vector3 spawnOffset = new Vector3(0.5f * direction, -0.5f, 0);
        Vector3 spawnPos = transform.position + spawnOffset;
        Instantiate(minionPrefab, spawnPos, minionPrefab.transform.rotation);
    }

    /*=========== B. CHASE + MOVEMENT ==============*/
    private void UpdateHasTargetFlag()
    {
        if (!canChase || player == null)
        {
            HasTarget = false;
            return;
        }

        HasTarget = Mathf.Abs(player.position.x - transform.position.x) <= stopDistance;
    }

    private void ChasePlayer()
    {
        if (!canChase) return;
        if (damageable.LockVelocity || !CanMove || !IsAlive || !touchingDirections.IsGrounded) return;

        if (player != null)
        {
            float dx = player.position.x - transform.position.x;
            walkDir = dx >= 0 ? WalkDir.Right : WalkDir.Left;
            FaceWalkDir();

            rb.linearVelocity = Mathf.Abs(dx) > stopDistance
                        ? new Vector2(chaseSpeed * Mathf.Sign(dx), rb.linearVelocity.y)
                        : new Vector2(0, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
    }

    private IEnumerator FlipCooldownRoutine()
    {
        canFlip = false;
        yield return new WaitForSeconds(flipCooldown);
        canFlip = true;
    }

    private void FlipDirection()
    {
        walkDir = walkDir == WalkDir.Right ? WalkDir.Left : WalkDir.Right;
        FaceWalkDir();
    }

    private void FaceWalkDir()
    {
        float sign = walkDir == WalkDir.Right ? 1 : -1;
        if (Mathf.Sign(transform.localScale.x) != sign)
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, 1);
    }

    /*=========== E. CALLBACKS =====================*/
    public void OnHit(int dmg, Vector2 kb)
    {
        rb.linearVelocity = new Vector2(kb.x, rb.linearVelocity.y + kb.y);
        canFlip = false;
        StartCoroutine(EnableFlipLater(1.2f));
    }
    private IEnumerator EnableFlipLater(float delay)
    {
        yield return new WaitForSeconds(delay);
        canFlip = true;
    }

    public void OnCliffDetected()
    {
        if (touchingDirections.IsGrounded) FlipDirection();
    }

    /*=========== PUBLIC API =======================*/
    public void ActivateChase()
    {
        canChase = true;
        Debug.Log(name + " canChase = TRUE");
    }
}
