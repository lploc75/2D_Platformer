using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Shared.Player
{
    public class ManaManager : MonoBehaviour
    {
        [Header("UI")]
        public Slider manaSlider;

        [Header("Thiết lập")]
        private float maxMana; // Không còn public
        public float manaRegenRate = 5f;   // Mana hồi mỗi giây
        public float regenDelay = 2f;      // Trễ trước khi hồi

        [HideInInspector]
        public float currentMana;

        private float regenTimer = 0f;
        private bool consumedThisFrame = false;

        void Start()
        {
            Debug.Log($"[ManaManager] Start -> currentMana = {currentMana}");

            if (manaSlider != null)
            {
                currentMana = maxMana;
                manaSlider.maxValue = maxMana;
                manaSlider.value = currentMana;
            }
        }

        void Update()
        {
            if (consumedThisFrame)
            {
                regenTimer = 0f;
                consumedThisFrame = false;
            }
            else
            {
                if (regenTimer >= regenDelay)
                {
                    float regenAmount = manaRegenRate * Time.deltaTime;
                    currentMana += regenAmount;
                    Debug.Log($"[ManaManager] Regen +{regenAmount:F2} -> currentMana = {currentMana:F2}");
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

        public bool HasMana(float cost)
        {
            return currentMana >= cost;
        }

        public bool ConsumeMana(float cost)
        {
            if (!HasMana(cost))
            {
                Debug.LogWarning($"[ManaManager] ❌ Không đủ mana! Yêu cầu {cost}, hiện tại {currentMana}");
                return false;
            }

            currentMana -= cost;
            consumedThisFrame = true;

            if (manaSlider != null)
                manaSlider.value = currentMana;

            Debug.Log($"[ManaManager] ✅ Tiêu hao {cost} mana → còn lại: {currentMana}");

            return true;
        }

        public void AddMana(float amount)
        {
            currentMana = Mathf.Clamp(currentMana + amount, 0f, maxMana);

            if (manaSlider != null)
                manaSlider.value = currentMana;

            Debug.Log($"[ManaManager] 💧 Hồi {amount} mana → currentMana = {currentMana}");
        }

        public void SetMaxMana(float newMax)
        {
            maxMana = Mathf.Max(newMax, 1f);
            currentMana = Mathf.Clamp(maxMana, 0f, maxMana);
            if (manaSlider != null)
            {
                manaSlider.maxValue = maxMana;
                manaSlider.value = currentMana;
            }

            Debug.Log($"[ManaManager] 🔁 SetMaxMana = {maxMana}, currentMana = {currentMana}");
        }
    }
}
