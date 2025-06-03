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

    private int index = 0;
    private bool isRunning = false;

    public void StartDialogue()
    {
        if (sentences == null || sentences.Length == 0)
        {
            Debug.LogWarning("No sentences set for dialogue!");
            return;
        }

        index = 0;
        isRunning = true;
        gameObject.SetActive(true); // Bật Panel nếu chưa bật
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
        gameObject.SetActive(false); // Ẩn luôn chính Panel!
    }
}
