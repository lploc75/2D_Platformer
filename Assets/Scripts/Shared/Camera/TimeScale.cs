using UnityEngine;

public class DebugSpeedUp : MonoBehaviour
{
    public float speedUpScale = 5f;

    void Update()
    {

            Time.timeScale = speedUpScale;           
        
        if (Input.GetKeyDown(KeyCode.F2))
        {
            Time.timeScale = 1f;
        }
    }
}
