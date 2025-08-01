﻿using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Assets.Scripts.Shared.Player;
using System.Collections.Generic;

public class PlayerStatsManager : MonoBehaviour
{
    public static PlayerStatsManager Instance { get; private set; }
    [SerializeField] private ManaManager manaManager;
    [SerializeField] private Damageable damageable;
    [SerializeField] private StaminaManager staminaManager;

    //public int totalPoint = 60;

    // Chỉ số thực tế đã cộng
    public int currentSTR = 0;
    public int currentINT = 0;
    public int currentDUR = 0;
    public int currentPER = 0;
    public int currentVIT = 0;

    // Level riêng từng stat
    public int strLevel = 0;
    public int intLevel = 0;
    public int durLevel = 0;
    public int perLevel = 0;
    public int vitLevel = 0;

    // Tham chiếu UI cho chỉ số thực tế
    public TMP_Text strText;
    public TMP_Text intText;
    public TMP_Text durText;
    public TMP_Text perText;
    public TMP_Text vitText;

    // Tham chiếu UI cho "level" từng stat (ô pointText cũ)
    public TMP_Text strPointText;
    public TMP_Text intPointText;
    public TMP_Text durPointText;
    public TMP_Text perPointText;
    public TMP_Text vitPointText;

    // Các nút cộng
    public Button buttonSTR, buttonINT, buttonDUR, buttonPER, buttonVIT;

    // Các chỉ số phụ, bonus...
    public int maxHP, maxMP, maxSP;
    public float manaRegen, critChance, critDamage, baseDamage;

    public TMP_Text hpText, mpText, spText, manaRegenText, critChanceText, critDamageText, baseDamageText;

    public float bonusArmorHealth = 0f;
    public float bonusAccessoryMP = 0f;
    public float bonusWeaponCritDamage = 0f;
    public float bonusArmorCritChance = 0f;
    public float bonusWeaponCritChance = 0f;
    public float bonusEquipSP = 0f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        PlayerStatsFileHandler.Load(this); // Tải dữ liệu từ file

        UpdateAllStatsUI();
        UpdateDerivedStats();
        UpdateButtonInteractable();

        if (buttonSTR != null) buttonSTR.onClick.AddListener(OnAddSTR);
        if (buttonINT != null) buttonINT.onClick.AddListener(OnAddINT);
        if (buttonDUR != null) buttonDUR.onClick.AddListener(OnAddDUR);
        if (buttonPER != null) buttonPER.onClick.AddListener(OnAddPER);
        if (buttonVIT != null) buttonVIT.onClick.AddListener(OnAddVIT);
    }

    public int GetPurpleSoulNeededForLevel(int level)
    {
        if (level >= 0 && level < 6) return 2;
        if (level >= 6 && level < 12) return 4;
        if (level >= 12 && level < 15) return 8;
        return int.MaxValue;
    }

    // ==== Chỉ nâng khi đủ point cho level tiếp theo, trừ luôn, cộng luôn ====
    public void OnAddSTR()
    {
        if (strLevel >= 15) return;
        int need = GetPurpleSoulNeededForLevel(strLevel);
        int purpleSoul = CurrencyManager.Instance.GetCurrency(CurrencyType.PurpleSoul);

        if (purpleSoul >= need)
        {
            CurrencyManager.Instance.AddCurrency(CurrencyType.PurpleSoul, -need);
            currentSTR += need;
            strLevel += 1;
            OnStatChanged();
        }
    }
    public void OnAddINT()
    {
        if (intLevel >= 15) return;
        int need = GetPurpleSoulNeededForLevel(intLevel);
        int purpleSoul = CurrencyManager.Instance.GetCurrency(CurrencyType.PurpleSoul);

        if (purpleSoul >= need)
        {
            CurrencyManager.Instance.AddCurrency(CurrencyType.PurpleSoul, -need);
            currentINT += need;
            intLevel += 1;
            OnStatChanged();
        }
    }

    public void OnAddDUR()
    {
        if (durLevel >= 15) return;
        int need = GetPurpleSoulNeededForLevel(durLevel);
        int purpleSoul = CurrencyManager.Instance.GetCurrency(CurrencyType.PurpleSoul);

        if (purpleSoul >= need)
        {
            CurrencyManager.Instance.AddCurrency(CurrencyType.PurpleSoul, -need);
            currentDUR += need;
            durLevel += 1;
            OnStatChanged();
        }
    }

    public void OnAddPER()
    {
        if (perLevel >= 15) return;
        int need = GetPurpleSoulNeededForLevel(perLevel);
        int purpleSoul = CurrencyManager.Instance.GetCurrency(CurrencyType.PurpleSoul);

        if (purpleSoul >= need)
        {
            CurrencyManager.Instance.AddCurrency(CurrencyType.PurpleSoul, -need);
            currentPER += need;
            perLevel += 1;
            OnStatChanged();
        }
    }

    public void OnAddVIT()
    {
        if (vitLevel >= 15) return;
        int need = GetPurpleSoulNeededForLevel(vitLevel);
        int purpleSoul = CurrencyManager.Instance.GetCurrency(CurrencyType.PurpleSoul);

        if (purpleSoul >= need)
        {
            CurrencyManager.Instance.AddCurrency(CurrencyType.PurpleSoul, -need);
            currentVIT += need;
            vitLevel += 1;
            OnStatChanged();
        }
    }

    public void UpdateAllStatsUI()
    {
        if (strText != null) strText.text = currentSTR.ToString();
        if (intText != null) intText.text = currentINT.ToString();
        if (durText != null) durText.text = currentDUR.ToString();
        if (perText != null) perText.text = currentPER.ToString();
        if (vitText != null) vitText.text = currentVIT.ToString();

        if (strPointText != null) strPointText.text = strLevel.ToString();
        if (intPointText != null) intPointText.text = intLevel.ToString();
        if (durPointText != null) durPointText.text = durLevel.ToString();
        if (perPointText != null) perPointText.text = perLevel.ToString();
        if (vitPointText != null) vitPointText.text = vitLevel.ToString();
    }

    public void UpdateButtonInteractable()
    {
        int purpleSoul = CurrencyManager.Instance.GetCurrency(CurrencyType.PurpleSoul);

        Debug.Log($"[UpdateButtonInteractable] PurpleSoul = {purpleSoul}");

        if (buttonSTR != null) buttonSTR.interactable = (purpleSoul >= GetPurpleSoulNeededForLevel(strLevel) && strLevel < 15);
        if (buttonINT != null) buttonINT.interactable = (purpleSoul >= GetPurpleSoulNeededForLevel(intLevel) && intLevel < 15);
        if (buttonDUR != null) buttonDUR.interactable = (purpleSoul >= GetPurpleSoulNeededForLevel(durLevel) && durLevel < 15);
        if (buttonPER != null) buttonPER.interactable = (purpleSoul >= GetPurpleSoulNeededForLevel(perLevel) && perLevel < 15);
        if (buttonVIT != null) buttonVIT.interactable = (purpleSoul >= GetPurpleSoulNeededForLevel(vitLevel) && vitLevel < 15);
    }


    public void UpdateDerivedStats()
    {
        // ==== Tổng hợp base + bonus từ trang bị ====
        int totalSTR = currentSTR;
        int totalINT = currentINT;
        int totalDUR = currentDUR;
        int totalPER = currentPER;
        int totalVIT = currentVIT;

        float totalBonusHP = 0f;
        float totalBonusMP = 0f;
        float totalBonusSP = 0f;
        float totalCritChance = 0f;
        float totalCritDamage = 0f;
        float totalBaseDamage = 0f;

        void ApplyItemBonus(ItemData item)
        {
            if (item == null) return;

            // ⚔️ Nếu là vũ khí
            if (item is WeaponData weapon)
            {
                totalBonusHP += weapon.hp;
                totalBonusMP += weapon.mp;
                totalBonusSP += weapon.sp;

                totalCritChance += weapon.critChance;
                totalCritDamage += weapon.critDamage;
                totalBaseDamage += weapon.baseDamage; // hoặc weapon.baseDamage nếu chưa cần nhân hệ số
            }

            // 🛡️ Nếu là giáp
            else if (item is ArmorData armor)
            {
                totalBonusHP += armor.GetHealthBonus();
                totalBonusMP += armor.GetMp();
                totalBonusSP += armor.GetSp();

                totalCritChance += armor.GetCritChance();
                totalCritDamage += armor.GetCritDamage();
                totalBaseDamage += armor.GetBaseDamage(); // nếu giáp cũng cộng dame
            }

            // 📌 Nếu bạn có bonus stat như STR, INT từ item: hãy thêm block riêng
            // if (item is SomeStatItem) { totalSTR += ... }
        }


        // ==== Lấy các item đang trang bị ====
        ApplyItemBonus(EquipmentManager.Instance.weaponSlotUI?.GetCurrentItem());
        ApplyItemBonus(EquipmentManager.Instance.armorSlotUI?.GetCurrentItem());
        // Apply thêm slot khác nếu có, ví dụ: boots, ring, v.v...

        // ==== Tính chỉ số từ tổng ====
        maxHP = 75 + (totalVIT * 4) + Mathf.RoundToInt(totalBonusHP);
        maxMP = 85 + (totalINT * 1) + Mathf.RoundToInt(totalBonusMP);
        maxSP = 75 + Mathf.RoundToInt(totalDUR * 1.25f) + Mathf.RoundToInt(totalBonusSP);

        manaRegen = totalINT * 0.2f;
        critChance = totalPER * 0.0075f + totalCritChance;
        critDamage = 0.2f + (totalPER / 25.0f) + totalCritDamage;
        baseDamage = 10 + (totalSTR / 3.0f) + totalBaseDamage;

        // ==== Cập nhật UI ====
        if (hpText != null) hpText.text = maxHP.ToString();
        if (mpText != null) mpText.text = maxMP.ToString();
        if (spText != null) spText.text = maxSP.ToString();
        if (manaRegenText != null) manaRegenText.text = manaRegen.ToString("0.00") + "/s";
        if (critChanceText != null) critChanceText.text = (critChance * 100).ToString("0.##") + "%";
        if (critDamageText != null) critDamageText.text = critDamage.ToString("0.00");
        if (baseDamageText != null) baseDamageText.text = baseDamage.ToString("0.0");

        // ==== Gửi sang hệ thống khác ====
        if (manaManager != null) manaManager.SetMaxMana(maxMP);
        else Debug.LogWarning("❌ ManaManager chưa được gán trong Inspector");

        if (damageable != null) damageable.SetMaxHealth(maxHP, true);
        else Debug.LogWarning("❌ Damageable chưa được gán trong Inspector");

        if (staminaManager != null) staminaManager.SetMaxStamina(maxSP);
        else Debug.LogWarning("❌ StaminaManager chưa được gán trong Inspector");
    }

    private void OnStatChanged()
    {
        UpdateAllStatsUI();
        UpdateDerivedStats();
        UpdateButtonInteractable();

        PlayerStatsFileHandler.Save(this); // Lưu dữ liệu vào file
    }
    public void ResetStats()
    {
        // Reset chỉ số cộng
        currentSTR = 0;
        currentINT = 0;
        currentDUR = 0;
        currentPER = 0;
        currentVIT = 0;

        // Reset cấp độ
        strLevel = 0;
        intLevel = 0;
        durLevel = 0;
        perLevel = 0;
        vitLevel = 0;
        OnStatChanged();
    }
}
