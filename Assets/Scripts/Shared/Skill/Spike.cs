using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Shared.Skill
{
    public class Spike : MonoBehaviour
    {
        [Header("Multi Spike Settings")]
        public bool spawnMultiple = false;
        public int spikeCount = 5;
        public float spikeSpacing = 1.0f;
        public float spikeDelay = 0.2f;


        private Collider2D _collider;
        private int damage;
        public Vector2 moveSpeed = new Vector2(15f, 0);
        private Vector2 knockback;
        private bool initialized = false;

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
        }
        private void Start()
        {
            Debug.Log($"[Spike] Start() called. spawnMultiple = {spawnMultiple}, initialized = {initialized}");

            if (spawnMultiple)
            {
                Debug.Log("[Spike] 👉 Bắt đầu SpawnSpikeLine()");
                StartCoroutine(SpawnSpikeLine());
            }
        }

        public void Init(int dmg, Vector2 kb)
        {
            damage = dmg;
            knockback = kb;
            StartCoroutine(SelfDestruct());
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            //if (!initialized) return; // tránh spike mẹ gây sát thương

            Damageable damageable = collision.GetComponent<Damageable>();
            if (damageable != null)
            {
                Vector2 deliveredKnockback = transform.localScale.x > 0
                    ? knockback
                    : new Vector2(-knockback.x, knockback.y);
                bool gotHit = damageable.Hit(damage, deliveredKnockback);

                Debug.Log($"⚔️ Projectile damage: {damage} -- Knockback: {deliveredKnockback}");
                if (gotHit)
                {
                    Debug.Log($"✅ {collision.name} bị trúng {damage} damage");
                }
            }
        }

        private IEnumerator SelfDestruct()
        {
            yield return new WaitForSeconds(1.5f);
            Destroy(gameObject);
        }

        // Gọi từ animation event
        public void DisableCollider()
        {
            if (_collider != null)
            {
                _collider.enabled = false;
                Debug.Log("❌ Collider đã bị tắt bởi animation");
            }
        }

        private IEnumerator SpawnSpikeLine()
        {
            Vector3 direction = transform.localScale.x > 0 ? Vector3.right : Vector3.left;

            for (int i = 0; i < spikeCount; i++)
            {
                Vector3 offset = direction * i * spikeSpacing;
                Vector3 spawnPos = transform.position + offset;

                Debug.Log($"[Spike] 🔁 Spawning spike {i + 1}/{spikeCount} tại {spawnPos}");

                GameObject spike = Instantiate(gameObject, spawnPos, Quaternion.identity);

                Spike spikeScript = spike.GetComponent<Spike>();
                spikeScript.spawnMultiple = false; // quan trọng!
                spikeScript.Init(damage, knockback);

                yield return new WaitForSeconds(spikeDelay);
            }

            Debug.Log("[Spike] ✅ Spawn xong, phá hủy spike mẹ");
            Destroy(gameObject);
        }

    }
}
