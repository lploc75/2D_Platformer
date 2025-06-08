using UnityEngine;

public class NPCInteractZone : MonoBehaviour
{
    private bool playerInRange = false;
    public NPCController npcController;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            npcController.Interact();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
