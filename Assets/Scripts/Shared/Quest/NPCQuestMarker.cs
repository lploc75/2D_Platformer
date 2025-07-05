using UnityEngine;
using TMPro;

public class NPCQuestMarker : MonoBehaviour
{
    public TextMeshPro markerTMP;   // Kéo TMP (World) hoặc TMP (UI - Canvas WorldSpace) vào đây
    public string npcId;

    [Header("Text marker settings")]
    public string exclamationText = "!";
    public string questionText = "?";

    [Header("Bounce Animation")]
    public float floatAmplitude = 0.12f; // Độ cao nhún lên xuống
    public float floatFrequency = 1.4f;  // Tốc độ nhún

    private Vector3 basePos;

    void Start()
    {
        if (markerTMP != null)
            basePos = markerTMP.transform.localPosition;
        UpdateMarker();
        if (QuestManager.Instance != null)
            QuestManager.Instance.OnQuestChanged += UpdateMarker;
    }
    void OnDestroy()
    {
        if (QuestManager.Instance != null)
            QuestManager.Instance.OnQuestChanged -= UpdateMarker;
    }

    void Update()
    {
        // Nếu marker đang hiện, thực hiện anim nhún
        if (markerTMP != null && markerTMP.enabled && !string.IsNullOrEmpty(markerTMP.text))
        {
            float offset = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
            markerTMP.transform.localPosition = basePos + new Vector3(0, offset, 0);
        }
    }

    public void UpdateMarker()
    {
        string questId = "main_1_crystal"; // Đổi đúng quest id của bạn!
        var qm = QuestManager.Instance;

        if (!qm.IsQuestAccepted(questId))
        {
            markerTMP.text = "";
            markerTMP.enabled = false;
            return;
        }

        if (qm.IsQuestInProgress(questId))
        {
            if (npcId == "chief")
            {
                if (!qm.HasTalkedWithNpc(questId, "chief"))
                {
                    markerTMP.text = exclamationText;
                    markerTMP.enabled = true;
                }
                else
                {
                    markerTMP.text = questionText;
                    markerTMP.enabled = true;
                }
            }
            else if (System.Array.Exists(qm.GetQuestById(questId).requiredNpcIds, id => id == npcId))
            {
                if (qm.HasTalkedWithNpc(questId, "chief") && !qm.HasTalkedWithNpc(questId, npcId))
                {
                    markerTMP.text = exclamationText;
                    markerTMP.enabled = true;
                }
                else
                {
                    markerTMP.text = "";
                    markerTMP.enabled = false;
                }
            }
            else
            {
                markerTMP.text = "";
                markerTMP.enabled = false;
            }
            return;
        }

        if (qm.IsQuestReadyToComplete(questId))
        {
            if (npcId == "chief")
            {
                markerTMP.text = exclamationText;
                markerTMP.enabled = true;
            }
            else
            {
                markerTMP.text = "";
                markerTMP.enabled = false;
            }
            return;
        }

        // Quest đã hoàn thành
        markerTMP.text = "";
        markerTMP.enabled = false;
    }
}
