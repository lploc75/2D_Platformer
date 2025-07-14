using UnityEngine;
using UnityEngine.InputSystem; // Phải import cái này!

public class NPCInteractZone : MonoBehaviour
{
    private bool playerInRange = false;
    public NPCController npcController;
    public ShopManager shopManager;


    private PlayerInput playerInput; // PlayerInput phải attach sẵn trên player!
    void Start()
    {
        if (shopManager == null)
        {
            shopManager = FindObjectOfType<ShopManager>(); // Automatically find ShopManager in the scene
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            playerInput = other.GetComponent<PlayerInput>();
            if (playerInput != null)
            {
                playerInput.actions["Interact"].performed += OnInteract; // Đăng ký event
                playerInput.actions["OpenShop"].performed += OnOpenShop;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (playerInput != null) { 
                playerInput.actions["Interact"].performed -= OnInteract; // Bỏ đăng ký
                playerInput.actions["OpenShop"].performed -= OnOpenShop;
            }
            playerInput = null;
            if (shopManager != null)
            {
                shopManager.shopPanel.SetActive(false);  // Close the shop panel
            }
        }
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (playerInRange && npcController != null)
        {
            npcController.Interact();
        }
    }
    private void OnOpenShop(InputAction.CallbackContext context)
    {
        if (playerInRange && npcController != null)
        {
            npcController.OpenShop();  // Open the corresponding shop based on NPC
        }
    }
}