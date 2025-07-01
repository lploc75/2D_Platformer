using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Shared.Player
{
    public class ManaManager : MonoBehaviour
    {
        [Header("UI")]
        public Slider manaSlider;
        [Header("Thiết lập")]
        public float maxMana = 100f;
        public float manaRegenRate = 5f;   // Mana hồi mỗi giây
        public float regenDelay = 2f;   // Trễ trước khi hồi

        [HideInInspector]
        public float currentMana;

        private float regenTimer = 0f;
        private bool consumedThisFrame = false;
        // ────────────────────────────────────────────────────────────
        void Start()
        {
            currentMana = maxMana;

            if (manaSlider != null)
            {
                manaSlider.maxValue = maxMana;
                manaSlider.value = currentMana;
            }
        }

        void Update()
        {
            // Nếu frame trước có tiêu hao, reset timer và bỏ cờ
            if (consumedThisFrame)
            {
                regenTimer = 0f;
                consumedThisFrame = false;
            }
            else
            {
                // Đếm ngược và hồi mana
                if (regenTimer >= regenDelay)
                {
                    currentMana += manaRegenRate * Time.deltaTime;
                }
                else
                {
                    regenTimer += Time.deltaTime;
                }
            }

            currentMana = Mathf.Clamp(currentMana, 0f, maxMana);

            if (manaSlider != null)
                manaSlider.value = currentMana;
        }

        // ────────────────────────────────────────────────────────────
        public bool HasMana(float cost) => currentMana >= cost;

        /// <summary>Tiêu hao mana, trả về true nếu thành công.</summary>
        public bool ConsumeMana(float cost)
        {
            if (!HasMana(cost)) return false;

            currentMana -= cost;
            consumedThisFrame = true;

            if (manaSlider != null)
                manaSlider.value = currentMana;

            return true;
        }

        /// <summary>Hồi mana ngay lập tức (item, hiệu ứng…)</summary>
        public void AddMana(float amount)
        {
            currentMana = Mathf.Clamp(currentMana + amount, 0f, maxMana);

            if (manaSlider != null)
                manaSlider.value = currentMana;
        }
    }
}