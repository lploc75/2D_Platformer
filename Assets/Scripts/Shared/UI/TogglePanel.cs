using UnityEngine;

public class TogglePanel : MonoBehaviour
{
    public GameObject panel;

    public void Toggle()
    {
        panel.SetActive(!panel.activeSelf);
    }
}
