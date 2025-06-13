using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider; // Gán trong Inspector
    /*
     Slider của mana min là 0 và max là 100, thay đổi bằng cách trừ máu tối đa - damgage nhận vào
     */
    public void SetMaxHealth(int maxHealth)
    {
        healthSlider.maxValue = maxHealth;
         // Bỏ dòng gán value ở đây
         // healthSlider.value = maxHealth;
    }

    public void SetHealth(int currentHealth)
    {
        if (healthSlider != null)
        {
            Debug.Log("Health slider value set to " + currentHealth);
            healthSlider.value = currentHealth;
        }
    }

}
