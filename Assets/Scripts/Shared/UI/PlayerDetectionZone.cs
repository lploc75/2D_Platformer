using UnityEngine;

public class PlayerDetectionZone : MonoBehaviour
{
    public Husky husky;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            husky.StartChasingPlayer(other.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            husky.StopChasingPlayer();
        }
    }
}
