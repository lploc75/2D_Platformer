using UnityEngine;

public class UIDontDestroy : MonoBehaviour
{
    private static UIDontDestroy instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // giữ lại Canvas và tất cả con của nó
        }
        else
        {
            Destroy(gameObject);            // nếu có duplicate khi load scene mới
        }
    }
}
