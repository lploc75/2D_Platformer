using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    public Transform luanchPoint;
    public GameObject projectPrefab;

    public void FireProjectile()
    {
        GameObject projectile =  Instantiate(projectPrefab, transform.position,
            projectPrefab.transform.rotation);
        Vector3 origScale = projectile.transform.localScale;
        // Xoay hướng spawn khi bắn vật thể (phép)
        projectile.transform.localScale = new Vector3(
            origScale.x * transform.localScale.x > 0 ? 1 : -1,
            origScale.y,
            origScale.z
            );
    }
}
