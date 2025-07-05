using UnityEngine;
using UnityEngine.UI;

public class ItemQualityUI : MonoBehaviour
{
    public Image qualityFrameImage;  // Image component for the quality frame

    public Sprite commonFrame;  // Sprite for common items (white frame)
    public Sprite rareFrame;  // Sprite for rare items (blue frame)
    public Sprite epicFrame;  // Sprite for epic items (purple frame)
    public Sprite legendaryFrame;  // Sprite for legendary items (gold frame)

    public ItemData itemData;  // The item (can be WeaponData or any other derived class)

    void Start()
    {
        // Kiểm tra xem qualityFrameImage có được gán trong Inspector không
        if (qualityFrameImage == null)
        {
            Debug.LogError("Quality Frame Image is not assigned in the Inspector.");
            return;
        }

        // Kiểm tra nếu không có vật phẩm hoặc sprite, đặt khung phẩm chất mặc định và màu nền
        if (itemData != null)
        {
            UpdateQualityFrame();  // Cập nhật khung phẩm chất
            qualityFrameImage.enabled = true;  // Bật khung phẩm chất
        }
        else
        {
            // Nếu không có item, giữ màu nền khung phẩm chất
            qualityFrameImage.color = new Color(0, 0, 0, 0);  // Màu nền mặc định (xám)
            qualityFrameImage.sprite = commonFrame;  // Gán khung phẩm chất mặc định
        }
    }

    public void UpdateQualityFrame()
    {
        if (qualityFrameImage == null)
            return;

        if (itemData == null)
        {
            qualityFrameImage.enabled = false; // Không có item thì ẩn khung
            return;
        }

        qualityFrameImage.enabled = true;

        switch (itemData.quality)
        {
            case ItemQuality.Common:
                qualityFrameImage.sprite = commonFrame;
                break;
            case ItemQuality.Rare:
                qualityFrameImage.sprite = rareFrame;
                break;
            case ItemQuality.Epic:
                qualityFrameImage.sprite = epicFrame;
                break;
            case ItemQuality.Legendary:
                qualityFrameImage.sprite = legendaryFrame;
                break;
            default:
                qualityFrameImage.sprite = commonFrame;
                break;
        }
        qualityFrameImage.color = Color.white;
    }

}
