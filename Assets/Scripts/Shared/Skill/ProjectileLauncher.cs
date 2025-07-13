using Assets.Scripts.Shared.Player;
using Assets.Scripts.Shared.Skill;
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
    public Transform luanchPoint;   // Điểm xuất phát của phép  
    public Transform groundLuanchPoint;   // Điểm xuất phát của phép dưới đất   
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
        if (currentSkillData == null)
        {
            Debug.LogWarning("❌ SkillData chưa được set!");
            return;
        }

        GameObject prefab = GetPrefabForCurrentSkill();
        if (prefab == null) return;

        // Tạo đạn tại vị trí launchPoint
        GameObject projectile = Instantiate(prefab, luanchPoint.position, prefab.transform.rotation);

        // Xoay hướng đạn theo hướng của nhân vật
        Vector3 origScale = projectile.transform.localScale;
        projectile.transform.localScale = new Vector3(
            transform.localScale.x > 0 ? Mathf.Abs(origScale.x) : -Mathf.Abs(origScale.x),
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

        // Gửi dữ liệu vào Projectile
        projectile.GetComponent<Projectile>().Init(Mathf.RoundToInt(finalDamage), kb, true);

        Debug.Log($"✅ Bắn {prefab.name} với damage {finalDamage}, knockback: {kb}, từ skill {currentSkillData.skillName}");
    }

    // Hàm được animation gọi
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
    public void FireSpike()
    {
        if (currentSkillData == null)
        {
            Debug.LogWarning("❌ SkillData chưa được set!");
            return;
        }

        GameObject prefab = GetPrefabForCurrentSkill();
        if (prefab == null) return;

        Vector3 spawnPos = groundLuanchPoint.position;
        Quaternion rotation = prefab.transform.rotation;

        /*―― Spawn spike gốc ――*/
        GameObject spikeObject = Instantiate(prefab, spawnPos, rotation);

        /*──────🔑 LẬT SCALE THEO HƯỚNG NGƯỜI CHƠI 🔑──────*/
        float dirX = transform.localScale.x > 0 ? 1f : -1f;   // nhân vật đang nhìn phải hay trái
        Vector3 localScale = spikeObject.transform.localScale;
        localScale.x = Mathf.Abs(localScale.x) * dirX;        // +1 hoặc -1
        spikeObject.transform.localScale = localScale;
        /*──────────────────────────────────────────────────*/

        int rolledDamage = RollDamage();
        float finalDamage = rolledDamage + currentSkillData.magicDamage;

        Vector2 kb = transform.localScale.x > 0
            ? currentSkillData.knockback
            : new Vector2(-currentSkillData.knockback.x, currentSkillData.knockback.y);

        Spike spike = spikeObject.GetComponent<Spike>();
        if (spike != null)
        {
            spike.spawnMultiple = true;
            spike.Init(Mathf.RoundToInt(finalDamage), kb);
        }

        Debug.Log($"🪨 Spike skill {currentSkillData.skillName} spawned with {finalDamage} damage at {spawnPos}");
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
