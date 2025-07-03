using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardIconUI : MonoBehaviour
{
    public Image qualityFrameImage; // Kéo QualityFrame vào đây
    public Image iconImage;         // Kéo Icon vào đây
    public TextMeshProUGUI amountText; // Thay vì Text

    // Các Sprite viền (cài trong Inspector)
    public Sprite commonFrame;
    public Sprite rareFrame;
    public Sprite epicFrame;
    public Sprite legendaryFrame;

    public void Setup(ItemData item, int amount = 1)
    {
        iconImage.sprite = item.icon;
        amountText.text = "x" + amount;

        switch (item.quality)
        {
            case ItemQuality.Common: qualityFrameImage.sprite = commonFrame; break;
            case ItemQuality.Rare: qualityFrameImage.sprite = rareFrame; break;
            case ItemQuality.Epic: qualityFrameImage.sprite = epicFrame; break;
            case ItemQuality.Legendary: qualityFrameImage.sprite = legendaryFrame; break;
            default: qualityFrameImage.sprite = commonFrame; break;
        }
    }
}
