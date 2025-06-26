using UnityEngine;
using UnityEngine.InputSystem; // Phải import cái này!

public class NPCInteractZone : MonoBehaviour
{
    private bool playerInRange = false;
    public NPCController npcController;

    private PlayerInput playerInput; // PlayerInput phải attach sẵn trên player!

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            playerInput = other.GetComponent<PlayerInput>();
            if (playerInput != null)
                playerInput.actions["Interact"].performed += OnInteract; // Đăng ký event
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (playerInput != null)
                playerInput.actions["Interact"].performed -= OnInteract; // Bỏ đăng ký
            playerInput = null;
        }
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (playerInRange && npcController != null)
        {
            npcController.Interact();
        }
    }
}
