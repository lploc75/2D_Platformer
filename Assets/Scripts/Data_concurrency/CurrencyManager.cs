﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    // Quản lý số dư từng loại tiền
    private Dictionary<CurrencyType, int> currencyAmounts = new Dictionary<CurrencyType, int>();

    // UI cho từng loại tiền (kéo vào Inspector)
    public Text coinText;
    public Text gemText;
    public Text blueSoulText;
    public Text purpleSoulText;

    // Nếu thêm loại mới, khai báo UI ở đây

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Khởi tạo số dư mặc định
            foreach (CurrencyType type in System.Enum.GetValues(typeof(CurrencyType)))
                currencyAmounts[type] = 0;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddCurrency(CurrencyType type, int amount)
    {
        if (!currencyAmounts.ContainsKey(type))
            currencyAmounts[type] = 0;
        currencyAmounts[type] += amount;
        UpdateCurrencyUI(type);
    }

    public int GetCurrency(CurrencyType type)
    {
        return currencyAmounts[type];
    }

    private void UpdateCurrencyUI(CurrencyType type)
    {
        switch (type)
        {
            case CurrencyType.Coin:
                if (coinText != null) coinText.text = currencyAmounts[type].ToString();
                break;
            case CurrencyType.Gem:
                if (gemText != null) gemText.text = currencyAmounts[type].ToString();
                break;
            case CurrencyType.BlueSoul:
                if (blueSoulText != null) blueSoulText.text = currencyAmounts[type].ToString();
                break;
            case CurrencyType.PurpleSoul:
                if (purpleSoulText != null) purpleSoulText.text = currencyAmounts[type].ToString();
                break;
        }
    }

}
