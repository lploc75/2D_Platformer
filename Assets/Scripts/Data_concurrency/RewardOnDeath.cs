using UnityEngine;

[RequireComponent(typeof(Damageable))]
public class RewardOnDeath : MonoBehaviour
{
    [Header("Currency Reward")]
    [Tooltip("Các loại tiền sẽ cộng khi quái chết (cùng một lượng).")]
    public CurrencyType[] currencies = { CurrencyType.Coin,
                                         CurrencyType.PurpleSoul,
                                         CurrencyType.BlueSoul };

    [Min(0)] public int minAmount = 1;
    [Min(0)] public int maxAmount = 3;

    void Awake()
    {
        GetComponent<Damageable>().OnDeath.AddListener(GiveReward);
    }

    void GiveReward()
    {
        int amount = Random.Range(minAmount, maxAmount + 1);

        foreach (var cur in currencies)
        {
            CurrencyManager.Instance.AddCurrency(cur, amount);
            InventoryManager.Instance.AddCurrency(cur, amount);
        }

        // AudioManager.Play("CoinPickup"); …
    }
}
