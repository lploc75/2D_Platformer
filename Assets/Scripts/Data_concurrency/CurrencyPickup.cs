using UnityEngine;

public class CurrencyPickup : MonoBehaviour
{
    public CurrencyType currencyType;
    public int amount = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CurrencyManager.Instance.AddCurrency(currencyType, amount);
            Destroy(gameObject);
        }
    }
}
