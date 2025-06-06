using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI nameText;        // Ô hiển thị tên nhân vật
    public TextMeshProUGUI dialogueText;    // Ô hiển thị câu thoại
    public Image avatarImage;               // Ô hiển thị avatar nhân vật (Sprite)
    public GameObject healthUI;             // UI máu/Health, ẩn khi thoại
    public PlayerController playerController; // Để khóa/mở di chuyển player khi thoại

    // Nội dung thoại hiện tại
    private string[] currentLines;
    private string currentSpeaker;
    private Sprite currentAvatar;

    private int index = 0;
    private bool isRunning = false;

    // Khởi động một đoạn thoại mới (truyền đủ tên, avatar, thoại)
    public void StartDialogueFromLines(string[] lines, string speakerName, Sprite avatar = null)
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

        index = 0;
        isRunning = true;
        gameObject.SetActive(true);

        ShowNextSentence();
    }

    private void Update()
    {
        if (!isRunning) return;
        // Nhấn Enter hoặc chuột phải để tiếp thoại
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(1))
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
    }

    //// (Tuỳ chọn) Cho phép gọi nhanh bằng profile NPC
    //public void StartDialogueByProfile(AdvancedDialogueProfile profile, string[] lines = null)
    //{
    //    // lines == null thì dùng defaultLines
    //    StartDialogueFromLines(
    //        lines ?? profile.defaultLines,
    //        profile.characterName,
    //        profile.avatar
    //    );
    //}
}
