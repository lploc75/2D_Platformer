using UnityEngine;
using UnityEngine.UI;

public class StaminaManager : MonoBehaviour
{
    [Header("UI")]
    public Slider staminaSlider;
    [Header("Thiết lập")]
    public float maxStamina = 100f;
    public float currentStamina;
    public float staminaDrainRate = 20f; // thể lực mất mỗi giây khi hành động
    public float staminaRegenRate = 10f; // thể lực hồi mỗi giây
    public float regenDelay = 2f; // thời gian chờ trước khi hồi

    private float regenTimer = 0f;
    private bool isUsingStamina = false;

    void Start()
    {
        currentStamina = maxStamina;
        staminaSlider.maxValue = maxStamina;
        staminaSlider.value = currentStamina;
    }

    void Update()
    {
        if (isUsingStamina)
        {
            currentStamina -= staminaDrainRate * Time.deltaTime;
            regenTimer = 0f;
        }
        else
        {
            if (regenTimer >= regenDelay)
            {
                currentStamina += staminaRegenRate * Time.deltaTime;
            }
            else
            {
                regenTimer += Time.deltaTime;
            }
        }

        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
        staminaSlider.value = currentStamina;
    }

    public bool HasStamina(float cost)
    {
        return currentStamina >= cost;
    }

    //public void UseStamina(float cost)
    //{
    //    currentStamina -= cost;
    //    regenTimer = 0f;
    //}
    public void SetUsingStamina(bool value)
    {
        isUsingStamina = value;
    }
    public void SetMaxStamina(float newMaxStamina)
    {
        maxStamina = newMaxStamina;
        currentStamina = Mathf.Clamp(maxStamina, 0, maxStamina); // đảm bảo không vượt quá giới hạn mới
        staminaSlider.maxValue = maxStamina;
        staminaSlider.value = currentStamina;
        Debug.Log($"[StaminaManager] 🔁 SetMaxStamina = {maxStamina}, currentStamina = {currentStamina}");

    }

}
