using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    public Rigidbody2D spikeRb;
    public float destroyDelay = 2f;

    private bool hasDropped = false;

    public void Drop()
    {
        if (!hasDropped)
        {
            hasDropped = true;
            spikeRb.bodyType = RigidbodyType2D.Dynamic;

            // Huỷ spike sau vài giây
            Destroy(spikeRb.gameObject, destroyDelay);
        }
    }
}
