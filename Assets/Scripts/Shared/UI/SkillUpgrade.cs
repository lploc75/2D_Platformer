using UnityEngine;
using UnityEngine.UI;

public class SkillUpgrade : MonoBehaviour
{
    public Image skillImage;          // Tham chiếu đến Image của kỹ năng
    public Button upgradeButton;      // Nút nâng cấp kỹ năng
    public Color lockedColor = Color.black; // Màu khi kỹ năng chưa mở khóa
    public Color unlockedColor = Color.white; // Màu khi kỹ năng đã mở khóa

    private bool isUnlocked = false;  // Trạng thái của kỹ năng

    // Hàm nâng cấp kỹ năng khi nhấn nút
    public void UpgradeSkill()
    {
        if (!isUnlocked)
        {
            isUnlocked = true;  // Đánh dấu kỹ năng đã được mở khóa
            skillImage.color = unlockedColor;  // Thay đổi màu của Image thành trắng
        }
    }

    // Hàm khởi tạo để thiết lập màu mặc định khi kỹ năng chưa mở khóa
    void Start()
    {
        skillImage.color = lockedColor;  // Ban đầu kỹ năng có màu đen
        upgradeButton.onClick.AddListener(UpgradeSkill);  // Thêm sự kiện cho nút nâng cấp
    }
}
