using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    public GameObject projectPrefab;

    public void FireProjectile()
    {
        Instantiate(projectPrefab, transform.position, projectPrefab.transform.rotation);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
