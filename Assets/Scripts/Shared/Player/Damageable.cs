using System;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// Thành phần quản lý máu, bất tử ngắn, Hit‑stun, và
/// phát sự kiện khi chết (OnDeath).
/// Dùng được cho cả Player lẫn Enemy.
/// </summary>
public class Damageable : MonoBehaviour
{
    [Header("Events")]
    /// <summary>Gọi khi bị đánh trúng (damage, knockback).</summary>
    public UnityEvent<int, Vector2> damageableHit;
    /// <summary>Gọi duy nhất một lần khi máu tụt về 0.</summary>
    public UnityEvent OnDeath;

    Animator animator;

    /* ───────────── Thuộc tính máu ───────────── */
    [SerializeField] private int _maxHealth;
    [SerializeField] private bool _isAlive = true;
    [SerializeField] private bool isInvincible = false;
    private int _health;
    private float timeSinceHit = 0;
    public float invincibilityTime = 0.25f;

    [Header("UI")]
    public HealthBar healthBar; // Gán trong Inspector

    public int MaxHealth
    {
        get => _maxHealth;
        set => _maxHealth = value;
    }

    public int Health
    {
        get => _health;
        set
        {
            _health = Mathf.Clamp(value, 0, MaxHealth); // ngăn âm hoặc vượt max

            if (healthBar != null)
            {
                healthBar.SetHealth(_health);
                Debug.Log("Health Slider change: " + _health);
            }

            if (_health <= 0)
            {
                IsAlive = false;
                OnDeath?.Invoke();
            }
        }
    }

    public bool IsAlive
    {
        get => _isAlive;
        set
        {
            _isAlive = value;
            animator.SetBool(AnimationStrings.isAlive, value);
            Debug.Log("IsAlive set " + value);
        }
    }

    public bool LockVelocity
    {
        get
        {
            return animator.GetBool(AnimationStrings.lockVelocity);
        }
        set
        {
            animator.SetBool(AnimationStrings.lockVelocity, value);
        }
    }
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        Health = MaxHealth; // Gán trước để trigger setter và cập nhật đúng
        Debug.Log(gameObject.name + "Có máu tối đa là" + MaxHealth);
        Debug.Log(gameObject.name + "Có máu hiện tại là" + Health);
        if (healthBar != null)
        {
            healthBar.SetMaxHealth(MaxHealth);

        }
    }

    private void Update()
    {
        if (isInvincible)
        {
            if (timeSinceHit > invincibilityTime)
            {
                isInvincible = false;
                timeSinceHit = 0;
            }
            timeSinceHit += Time.deltaTime;
        }
    }
    /* ───────────── API bị đánh ───────────── */
    public bool Hit(int damage, Vector2 knockback)
    {
        if (IsAlive && !isInvincible)
        {
            Health -= damage;
            isInvincible = true;

            animator.SetTrigger(AnimationStrings.hitTrigger);
            LockVelocity = true;
            damageableHit?.Invoke(damage, knockback);

            // Sau một khoảng thời gian, tắt lock lại
            StartCoroutine(UnlockVelocityAfterDelay(0.5f));
            return true;
        }
        return false;
    }
    // Tắt Lockvelocity cho phép di chuyển
    private System.Collections.IEnumerator UnlockVelocityAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        LockVelocity = false;
    }

    public void SetMaxHealth(int newMax, bool fill = true)
    {
        MaxHealth = Mathf.Max(newMax, 1); // an toàn
        if (fill)
        {
            Health = MaxHealth;
        }
        else
        {
            Health = Mathf.Clamp(Health, 0, MaxHealth);
        }

        if (healthBar != null)
        {
            healthBar.SetMaxHealth(MaxHealth);
            healthBar.SetHealth(Health); // cập nhật UI
        }

        Debug.Log($"[Damageable]  SetMaxHealth = {MaxHealth}, currentHealth = {Health}");
    }
    /// <summary>
    /// Hồi máu (có thể vượt quá máu hiện tại nhưng không vượt max)
    /// </summary>
    public void Heal(int amount)
    {
        if (IsAlive && amount > 0)
        {
            Health += amount; // Setter đã tự Clamp
            Debug.Log($"[Damageable] Hồi {amount} máu, máu hiện tại: {Health}");
        }
    }

}
