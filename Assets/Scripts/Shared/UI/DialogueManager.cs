using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Quản lý hiển thị thoại, hỗ trợ callback khi kết thúc thoại.
/// </summary>
public class DialogueManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI nameText;        // Ô tên nhân vật
    public TextMeshProUGUI dialogueText;    // Ô thoại
    public Image avatarImage;               // Ảnh đại diện NPC
    public GameObject healthUI;             // UI máu, sẽ ẩn khi thoại
    public PlayerController playerController; // Để khóa/mở điều khiển player khi thoại

    // Dữ liệu thoại hiện tại
    private string[] currentLines;
    private string currentSpeaker;
    private Sprite currentAvatar;
    private System.Action onDialogueFinish; // Callback khi thoại xong

    private int index = 0;
    private bool isRunning = false;

    /// <summary>
    /// Bắt đầu đoạn thoại mới, có callback khi thoại xong.
    /// </summary>
    public void StartDialogueFromLines(
        string[] lines,
        string speakerName,
        Sprite avatar = null,
        System.Action onFinish = null)
    {
        if (lines == null || lines.Length == 0)
        {
            Debug.LogWarning("No dialogue lines!");
            return;
        }

        if (healthUI != null)
            healthUI.SetActive(false);

        if (playerController != null)
            playerController.canControl = false;

        currentLines = lines;
        currentSpeaker = speakerName;
        currentAvatar = avatar;
        onDialogueFinish = onFinish;

        index = 0;
        isRunning = true;
        gameObject.SetActive(true);

        ShowNextSentence();
    }

    private void Update()
    {
        if (!isRunning) return;

        // Nhấn Enter, Space hoặc chuột phải để tiếp thoại
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1))
        {
            ShowNextSentence();
        }
    }

    public void ShowNextSentence()
    {
        if (currentLines != null && index < currentLines.Length)
        {
            // Gán tên, avatar
            if (nameText != null)
                nameText.text = currentSpeaker;

            if (avatarImage != null)
            {
                if (currentAvatar != null)
                {
                    avatarImage.gameObject.SetActive(true);
                    avatarImage.sprite = currentAvatar;
                }
                else
                {
                    avatarImage.gameObject.SetActive(false); // Không có avatar thì ẩn luôn
                }
            }

            if (dialogueText != null)
                dialogueText.text = currentLines[index];

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
            healthUI.SetActive(true);

        if (playerController != null)
            playerController.canControl = true;

        gameObject.SetActive(false);

        // Gọi callback khi kết thúc thoại
        onDialogueFinish?.Invoke();
        onDialogueFinish = null;
    }

    // (Tuỳ chọn) Cho phép gọi nhanh bằng profile NPC
    public void StartDialogueByProfile(AdvancedDialogueProfile profile, string[] lines = null, System.Action onFinish = null)
    {
        StartDialogueFromLines(
            lines ?? profile.defaultLines,
            profile.characterName,
            profile.avatar,
            onFinish
        );
    }
}
