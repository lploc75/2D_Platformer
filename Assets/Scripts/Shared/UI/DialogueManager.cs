using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Image avatarImage;
    public GameObject healthUI;

    [Header("Input System")]
    public PlayerInput playerInput;        // Kéo PlayerInput (trên Player) vào đây
    public string actionMapName = "Player"; // Hoặc "Gameplay", tuỳ tên bạn đặt

    // Dữ liệu thoại hiện tại
    private string[] currentLines;
    private string currentSpeaker;
    private Sprite currentAvatar;
    private System.Action onDialogueFinish;

    private int index = 0;
    private bool isRunning = false;

    // Input Action để next thoại
    private InputAction nextAction;

    private void Awake()
    {
        // Tạo input action và add event 1 lần duy nhất
        nextAction = new InputAction(type: InputActionType.Button);
        nextAction.AddBinding("<Keyboard>/enter");
        nextAction.AddBinding("<Keyboard>/space");
        nextAction.AddBinding("<Mouse>/rightButton");
        nextAction.performed += OnNext;
    }

    private void OnEnable()
    {
        nextAction.Enable();
    }

    private void OnDisable()
    {
        nextAction.Disable();
    }

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

        // Disable action map khi thoại bắt đầu
        if (playerInput != null)
        {
            var map = playerInput.actions.FindActionMap(actionMapName);
            if (map != null) map.Disable();
        }

        currentLines = lines;
        currentSpeaker = speakerName;
        currentAvatar = avatar;
        onDialogueFinish = onFinish;

        index = 0;
        isRunning = true;
        gameObject.SetActive(true);

        ShowNextSentence();
    }

    private void OnNext(InputAction.CallbackContext ctx)
    {
        if (isRunning)
            ShowNextSentence();
    }

    public void ShowNextSentence()
    {
        if (currentLines != null && index < currentLines.Length)
        {
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
                    avatarImage.gameObject.SetActive(false);
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

        // Enable action map lại khi thoại kết thúc
        if (playerInput != null)
        {
            var map = playerInput.actions.FindActionMap(actionMapName);
            if (map != null) map.Enable();
        }

        gameObject.SetActive(false);

        onDialogueFinish?.Invoke();
        onDialogueFinish = null;
    }

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