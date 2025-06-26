using UnityEngine;

public class CurrencyPickup : MonoBehaviour
{
    public CurrencyType currencyType;
    public int amount = 1;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CurrencyManager.Instance.AddCurrency(currencyType, amount);
            Destroy(gameObject);
        }
    }
}
