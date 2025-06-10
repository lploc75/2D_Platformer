using System.Collections.Generic;
using UnityEngine;

public class SkillInfoPopupManager : MonoBehaviour
{
    public static List<SkillInfoPopupManager> allPopups = new List<SkillInfoPopupManager>();

    public GameObject popupPanel;

    void Awake()
    {
        allPopups.Add(this);
    }

    void OnDestroy()
    {
        allPopups.Remove(this);
    }

    public void TogglePopup()
    {
        // Nếu popup này đang mở thì đóng lại
        if (popupPanel.activeSelf)
        {
            popupPanel.SetActive(false);
        }
        else
        {
            // Đóng tất cả popup khác trước khi mở popup này
            foreach (var popup in allPopups)
                if (popup != this) popup.ClosePopup();

            popupPanel.SetActive(true);
        }
    }


    public void ClosePopup()
    {
        popupPanel.SetActive(false);
    }
}
