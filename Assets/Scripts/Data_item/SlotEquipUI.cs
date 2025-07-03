using UnityEngine;
using UnityEngine.UI;

public class SlotEquipUI : MonoBehaviour
{
    public Image iconImage;
    private ItemData currentItem; // lưu lại item đang trang bị

    public void SetItem(ItemData item)
    {
        currentItem = item;
        if (item != null)
        {
            iconImage.sprite = item.icon;
            iconImage.enabled = true;
        }
        else
        {
            iconImage.sprite = null;
            iconImage.enabled = false;
        }
    }

    public ItemData GetCurrentItem()
    {
        return currentItem;
    }

}
