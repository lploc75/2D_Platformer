using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Animator animator; // Gắn Animator ở cửa vào đây
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool("IsPlayerNear", true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool("IsPlayerNear", false);
        }
    }

}
