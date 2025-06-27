using UnityEngine;

public class TrapTile : MonoBehaviour
{
    public float delay = 0.5f;           // Thời gian chờ trước khi rơi
    public float destroyAfter = 3.5f;      // Thời gian sau khi rơi thì biến mất

    private bool isTriggered = false;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isTriggered && collision.collider.CompareTag("Player"))
        {
            isTriggered = true;
            Invoke(nameof(Fall), delay);
        }
    }

    void Fall()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        Destroy(gameObject, destroyAfter); // Biến mất sau 2 giây kể từ khi rơi
    }
}
