using UnityEngine;

public class SpikeTriggerZone : MonoBehaviour
{
    public SpikeTrap spikeTrap;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            spikeTrap.Drop();
        }
    }
}
