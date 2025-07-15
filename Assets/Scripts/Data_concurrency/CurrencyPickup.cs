using UnityEngine;

public class CurrencyPickup : MonoBehaviour
{
    public CurrencyType currencyType;
    public int amount = 1;
    private void Start()
    {
        Destroy(gameObject, 20f); // tự hủy sau 20s nếu không ai nhặt
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CurrencyManager.Instance.AddCurrency(currencyType, amount);
            InventoryManager.Instance.AddCurrency(currencyType, amount); // <-- THÊM DÒNG NÀY!

            Destroy(gameObject);
        }
    }


}
