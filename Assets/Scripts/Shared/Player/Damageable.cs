﻿using System;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    public UnityEvent<int, Vector2> damageableHit;

    Animator animator;

    [SerializeField] private int _maxHealth;
    [SerializeField] private bool _isAlive = true;
    [SerializeField] private bool isInvincible = false;

    private int _health;
    private float timeSinceHit = 0;
    public float invincibilityTime = 0.25f;

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
                Debug.Log("Slider change: " + _health);
            }

            if (_health <= 0)
            {
                IsAlive = false;
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

        Debug.Log($"[Damageable] 🔁 SetMaxHealth = {MaxHealth}, currentHealth = {Health}");
    }
}
