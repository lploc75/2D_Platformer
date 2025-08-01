﻿using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    // Quản lý số dư từng loại tiền
    private Dictionary<CurrencyType, int> currencyAmounts = new Dictionary<CurrencyType, int>();

    // UI cho từng loại tiền (kéo vào Inspector)
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI gemText;
    public TextMeshProUGUI blueSoulText;
    public TextMeshProUGUI purpleSoulText;

    // Nếu thêm loại mới, khai báo UI ở đây

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

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
        // Chỉ cộng trophy nếu là Coin
        if (type == CurrencyType.Coin)
            TrophyRecordUI.Instance.AddGoldToTrophy(amount);
    }

    public int GetCurrency(CurrencyType type)
    {
        return currencyAmounts[type];
    }
    public void SetCurrency(CurrencyType type, int amount)
    {
        currencyAmounts[type] = amount;
        UpdateCurrencyUI(type);
    }
    public bool RemoveCurrency(CurrencyType type, int amount)
    {
        if (!currencyAmounts.ContainsKey(type))
            return false;

        if (currencyAmounts[type] < amount)
            return false; // Không đủ tiền

        currencyAmounts[type] -= amount;
        UpdateCurrencyUI(type);
        return true;
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
    public void ResetCurrency()
    {
        currencyAmounts.Clear();

        // Đảm bảo tất cả các loại tiền được khởi tạo lại sau khi clear
        foreach (CurrencyType type in System.Enum.GetValues(typeof(CurrencyType)))
        {
            currencyAmounts[type] = 0;
            UpdateCurrencyUI(type);
        }

        Debug.Log("[CurrencyManager] Đã reset currency.");
    }



}
