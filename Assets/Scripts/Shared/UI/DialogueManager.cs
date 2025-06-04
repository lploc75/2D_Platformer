using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public string characterName = "Alvin";
    [TextArea(2, 5)]
    public string[] sentences;
    public GameObject healthUI; // Kéo object HealthUI vào đây trên Inspector

    // Thêm biến này:
    public PlayerController playerController; // Kéo Player vào Inspector

    private int index = 0;
    private bool isRunning = false;

    public void StartDialogue()
    {
        if (sentences == null || sentences.Length == 0)
        {
            Debug.LogWarning("No sentences set for dialogue!");
            return;
        }

        if (healthUI != null)
            healthUI.SetActive(false);   // Ẩn UI máu

        if (playerController != null)
            playerController.canControl = false;

        index = 0;
        isRunning = true;
        gameObject.SetActive(true);
        ShowNextSentence();
    }

    private void Update()
    {
        if (!isRunning) return;
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(1))
        {
            ShowNextSentence();
        }
    }

    public void ShowNextSentence()
    {
        if (index < sentences.Length)
        {
            nameText.text = characterName;
            dialogueText.text = sentences[index];
            index++;
        }
        else
        {
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        isRunning = false;

        if (healthUI != null)
            healthUI.SetActive(true);    // Hiện lại UI máu

        if (playerController != null)
            playerController.canControl = true;

        gameObject.SetActive(false);
    }
}
