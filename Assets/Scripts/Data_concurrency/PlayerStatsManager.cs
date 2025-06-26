using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStatsManager : MonoBehaviour
{
    public static PlayerStatsManager Instance { get; private set; }

    // Point gốc (số lần cộng còn lại cho từng chỉ số)
    public int STR = 10;
    public int INT = 10;
    public int DUR = 10;
    public int PER = 10;
    public int VIT = 10;

    // Chỉ số thực tế đã cộng
    public int currentSTR = 0;
    public int currentINT = 0;
    public int currentDUR = 0;
    public int currentPER = 0;
    public int currentVIT = 0;

    // Tham chiếu UI cho chỉ số thực tế
    public TMP_Text strText;
    public TMP_Text intText;
    public TMP_Text durText;
    public TMP_Text perText;
    public TMP_Text vitText;

    // Tham chiếu UI cho point còn lại
    public TMP_Text strPointText;
    public TMP_Text intPointText;
    public TMP_Text durPointText;
    public TMP_Text perPointText;
    public TMP_Text vitPointText;

    // Các nút cộng
    public Button buttonSTR, buttonINT, buttonDUR, buttonPER, buttonVIT;

    // --- Chỉ số phụ và tham chiếu UI ---
    public int maxHP, maxMP, maxSP;
    public float manaRegen, critChance, critDamage, baseDamage;

    public TMP_Text hpText, mpText, spText, manaRegenText, critChanceText, critDamageText, baseDamageText;

    // --- Biến bonus từ trang bị (hiện tại mặc định 0) ---
    public float bonusArmorHealth = 0f;        // Tăng HP từ giáp
    public float bonusAccessoryMP = 0f;        // Tăng MP từ phụ kiện
    public float bonusWeaponCritDamage = 0f;   // Bonus crit damage từ vũ khí
    public float bonusArmorCritChance = 0f;    // Bonus crit chance từ giáp
    public float bonusWeaponCritChance = 0f;   // Bonus crit chance từ vũ khí
    public float bonusEquipSP = 0f;            // Bonus SP từ trang bị

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateAllStatsUI();
        UpdateDerivedStats();

        if (buttonSTR != null) buttonSTR.onClick.AddListener(OnAddSTR);
        if (buttonINT != null) buttonINT.onClick.AddListener(OnAddINT);
        if (buttonDUR != null) buttonDUR.onClick.AddListener(OnAddDUR);
        if (buttonPER != null) buttonPER.onClick.AddListener(OnAddPER);
        if (buttonVIT != null) buttonVIT.onClick.AddListener(OnAddVIT);
    }

    public void OnAddSTR()
    {
        if (STR <= 0) return;
        currentSTR += 1;
        STR -= 1;
        UpdateAllStatsUI();
        UpdateDerivedStats();
    }
    public void OnAddINT()
    {
        if (INT <= 0) return;
        currentINT += 1;
        INT -= 1;
        UpdateAllStatsUI();
        UpdateDerivedStats();
    }
    public void OnAddDUR()
    {
        if (DUR <= 0) return;
        currentDUR += 1;
        DUR -= 1;
        UpdateAllStatsUI();
        UpdateDerivedStats();
    }
    public void OnAddPER()
    {
        if (PER <= 0) return;
        currentPER += 1;
        PER -= 1;
        UpdateAllStatsUI();
        UpdateDerivedStats();
    }
    public void OnAddVIT()
    {
        if (VIT <= 0) return;
        currentVIT += 1;
        VIT -= 1;
        UpdateAllStatsUI();
        UpdateDerivedStats();
    }

    public void UpdateAllStatsUI()
    {
        // Hiện chỉ số thực tế (current)
        if (strText != null) strText.text = currentSTR.ToString();
        if (intText != null) intText.text = currentINT.ToString();
        if (durText != null) durText.text = currentDUR.ToString();
        if (perText != null) perText.text = currentPER.ToString();
        if (vitText != null) vitText.text = currentVIT.ToString();

        // Hiện số point còn lại để cộng cho từng chỉ số
        if (strPointText != null) strPointText.text = STR.ToString();
        if (intPointText != null) intPointText.text = INT.ToString();
        if (durPointText != null) durPointText.text = DUR.ToString();
        if (perPointText != null) perPointText.text = PER.ToString();
        if (vitPointText != null) vitPointText.text = VIT.ToString();
    }

    public void UpdateDerivedStats()
    {
        // HP tối đa: 75 + (VIT × 5) + bonus từ giáp
        maxHP = 75 + (currentVIT * 5) + Mathf.RoundToInt(bonusArmorHealth);

        // MP tối đa: 85 + (INT × 3) + bonus từ phụ kiện
        maxMP = 85 + (currentINT * 3) + Mathf.RoundToInt(bonusAccessoryMP);

        // SP tối đa: 75 + bonus từ trang bị
        maxSP = 75 + Mathf.RoundToInt(bonusEquipSP);

        // Mana regen: INT × 0.2 mỗi giây
        manaRegen = currentINT * 0.2f;

        // Crit Chance: PER × 0.02 (%) + bonus từ vũ khí và giáp
        critChance = currentPER * 0.02f + bonusArmorCritChance + bonusWeaponCritChance;

        // Crit Damage: 0.2 + (INT × 0.2) + bonus từ vũ khí
        critDamage = 0.2f + (currentINT * 0.2f) + bonusWeaponCritDamage;

        // Base Damage: 10 + (currentSTR * 0.5)
        baseDamage = 10 + (currentSTR * 0.5f);

        // Update UI
        if (hpText != null) hpText.text = maxHP.ToString();
        if (mpText != null) mpText.text = maxMP.ToString();
        if (spText != null) spText.text = maxSP.ToString();
        if (manaRegenText != null) manaRegenText.text = manaRegen.ToString("0.00") + "/s";
        if (critChanceText != null) critChanceText.text = (critChance * 100).ToString("0.##") + "%";
        if (critDamageText != null) critDamageText.text = critDamage.ToString("0.00");
        if (baseDamageText != null) baseDamageText.text = baseDamage.ToString("0.0");
    }
}
