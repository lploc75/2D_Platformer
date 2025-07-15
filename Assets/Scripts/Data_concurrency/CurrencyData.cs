using UnityEngine;

[CreateAssetMenu(fileName = "NewCurrency", menuName = "Inventory/CurrencyData")]
public class CurrencyData : ItemData
{
    // KHÔNG khai báo lại currencyType nếu đã có ở ItemData
    public CurrencyType currencyType;
    // Luôn lấy số dư động từ CurrencyManager
    public int amount => CurrencyManager.Instance.GetCurrency(currencyType);
}
