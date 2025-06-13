using UnityEngine;

public class DebugSpeedUp : MonoBehaviour
{
    public float speedUpScale = 5f;

    void Update()
    {
        // Nhấn phím F1 để tăng tốc, F2 để về bình thường
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Time.timeScale = speedUpScale;
            Debug.Log("Game speed up!");
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            Time.timeScale = 1f;
            Debug.Log("Game normal speed.");
        }
    }
}
