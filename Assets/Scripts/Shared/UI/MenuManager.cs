using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public SimpleMenuButton[] buttons;

    public void SetSelectedButton(SimpleMenuButton selected)
    {
        foreach (var btn in buttons)
            btn.SetSelected(btn == selected);
    }

    void Start()
    {
        if (buttons.Length > 0)
            SetSelectedButton(buttons[0]);
    }
}
