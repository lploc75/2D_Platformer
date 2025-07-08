using Assets.Scripts.Shared.Player;
using UnityEngine;
using UnityEngine.InputSystem;
[System.Serializable]
public class SkillPrefab
{
    public string skillName;
    public GameObject prefab;
}
public class ProjectileLauncher : MonoBehaviour
{
    public Transform luanchPoint;   // Điểm xuất phát của đạn    
    public GameObject projectPrefab;     // Kỹ năng hiện tại, được set từ PlayerController
    [Header("Danh sách prefab ứng với từng kỹ năng")]
    public SkillPrefab[] skillPrefabs; // Gán từ Inspector

    private SkillData currentSkillData;  // Kỹ năng hiện tại, được set từ PlayerController

    /// <summary>
    /// Được gọi từ PlayerController khi người chơi chọn skill.
    /// Lưu lại dữ liệu kỹ năng để sử dụng khi animation gọi FireProjectile().
    /// </summary>
    /// <param name="skillData">Kỹ năng được chọn</param>
    public void SetSkillData(SkillData skillData)
    {
        currentSkillData = skillData;
    }
    private GameObject GetPrefabForCurrentSkill()
    {
        foreach (var item in skillPrefabs)
        {
            if (item.skillName == currentSkillData.skillName)
            {
                return item.prefab;
            }
        }

        Debug.LogWarning($"⚠️ Không tìm thấy prefab cho skill: {currentSkillData.skillName}");
        return null;
    }

    // Hàm được animation gọi
    public void FireProjectile()
    {
        // Kiểm tra nếu chưa có dữ liệu kỹ năng
        if (currentSkillData == null)
        {
            Debug.LogWarning("❌ SkillData chưa được set!");
            return;
        }
        // Tạo đạn tại vị trí launchPoint
        GameObject projectile = Instantiate(projectPrefab, luanchPoint.position, projectPrefab.transform.rotation);
        // Xoay hướng đạn theo hướng của nhân vật
        Vector3 origScale = projectile.transform.localScale;
        projectile.transform.localScale = new Vector3(
            origScale.x * transform.localScale.x > 0 ? 1 : -1,
            origScale.y,
            origScale.z
        );

        // 🎯 Tính sát thương cuối cùng
        int rolledDamage = RollDamage();
        float finalDamage = rolledDamage + currentSkillData.magicDamage;
        // Knockback theo hướng nhân vật
        Vector2 kb = transform.localScale.x > 0
            ? currentSkillData.knockback
            : new Vector2(-currentSkillData.knockback.x, currentSkillData.knockback.y);
        // Gửi thông tin vào script Projectile
        projectile.GetComponent<Projectile>().Init(Mathf.RoundToInt(finalDamage), kb, true); // AutoMove = true
        Debug.Log($"✅ Đã bắn Projectile với damage: {finalDamage}, knockback: {kb} từ skill {currentSkillData.skillName}");
    }
    public void SpawnProjectileAtMouse()
    {
        if (currentSkillData == null)
        {
            Debug.LogWarning("❌ SkillData chưa được set!");
            return;
        }

        GameObject prefab = GetPrefabForCurrentSkill();
        if (prefab == null) return;

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mouseWorldPos.z = 0;

        GameObject projectile = Instantiate(prefab, mouseWorldPos, Quaternion.identity);

        Vector3 origScale = projectile.transform.localScale;
        projectile.transform.localScale = new Vector3(
            transform.localScale.x > 0 ? Mathf.Abs(origScale.x) : -Mathf.Abs(origScale.x),
            origScale.y,
            origScale.z
        );

        int rolledDamage = RollDamage();
        float finalDamage = rolledDamage + currentSkillData.magicDamage;
        Vector2 kb = transform.localScale.x > 0
            ? currentSkillData.knockback
            : new Vector2(-currentSkillData.knockback.x, currentSkillData.knockback.y);

        projectile.GetComponent<Projectile>().Init(Mathf.RoundToInt(finalDamage), kb, false); // AutoMove = false

        Debug.Log($"✅ Spawned {prefab.name} with damage {finalDamage} at {mouseWorldPos}");
    }

    private int RollDamage()
    {
        float baseDamage = PlayerStatsManager.Instance.baseDamage;
        float critChance = PlayerStatsManager.Instance.critChance;
        float critDamageBonus = PlayerStatsManager.Instance.critDamage; // Ví dụ: 0.1f nghĩa là +10%

        float damageAfterRolling = baseDamage;
        bool isCritical = Random.value < critChance;

        if (isCritical)
        {
            float bonus = baseDamage * critDamageBonus;
            damageAfterRolling += bonus;
            Debug.Log($"💥 Chí mạng! Base: {baseDamage}, Bonus: {bonus}, Total: {damageAfterRolling} (Chưa tính sát thương của phép)");
        }
        else
        {
            Debug.Log($"🟢 Sát thương thường: {damageAfterRolling} (Chưa tính sát thương của phép)");
        }

        return Mathf.RoundToInt(damageAfterRolling);
    }

}
