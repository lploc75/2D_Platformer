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
        Debug.LogWarning("GiveReward đã được gọi");
        foreach (var cur in currencies)
        {
            int amount = Random.Range(minAmount, maxAmount + 1);
            CurrencyManager.Instance.AddCurrency(cur, amount);
            PlayerStatsManager.Instance?.UpdateButtonInteractable();
        }
        TrophyRecordUI.Instance.AddKill();

        // Lưu sau khi nhận thưởng
        PlayerStatsManager playerStats = FindObjectOfType<PlayerStatsManager>();
        if (playerStats != null)
        {
            PlayerStatsFileHandler.Save(playerStats);
        }
        else
        {
            Debug.LogWarning("⚠️ Không tìm thấy PlayerStatsManager để lưu dữ liệu!");
        }
    }



}
