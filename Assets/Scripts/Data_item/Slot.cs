using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Image icon;  // Icon vật phẩm trong slot
    private Sprite currentItem;  // Vật phẩm hiện tại trong slot

    // Cập nhật icon của vật phẩm trong slot
    public void SetItem(Sprite itemIcon)
    {
        currentItem = itemIcon;
        icon.sprite = itemIcon;
    }

    // Kiểm tra nếu slot đang trống
    public bool IsSlotEmpty()
    {
        return currentItem == null;
    }
}
