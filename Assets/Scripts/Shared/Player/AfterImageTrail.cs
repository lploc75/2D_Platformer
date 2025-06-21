//using UnityEngine;

//public class AfterImageTrail : MonoBehaviour
//{
//    public GameObject shadowPrefab;
//    public float spawnInterval = 0.05f;
//    public float shadowLifespan = 0.3f;
//    private float timer;
//    private SpriteRenderer playerSr;
//    public PlayerController playerController; // Kéo PlayerController vào inspector!

//    void Start()
//    {
//        playerSr = GetComponent<SpriteRenderer>();
//    }

//    void Update()
//    {
//        // CHỈ tạo bóng khi Running
//        if (playerController.IsRunning)
//        {
//            timer += Time.deltaTime;
//            if (timer >= spawnInterval)
//            {
//                SpawnShadow();
//                timer = 0f;
//            }
//        }
//        else
//        {
//            timer = spawnInterval;
//        }
//    }

//    void SpawnShadow()
//    {
//        GameObject shadow = Instantiate(shadowPrefab, transform.position, Quaternion.identity);
//        var shadowSr = shadow.GetComponent<SpriteRenderer>();
//        shadowSr.sprite = playerSr.sprite;
//        shadowSr.flipX = playerSr.flipX;
//        shadowSr.flipY = playerSr.flipY;
//        shadowSr.transform.localScale = playerSr.transform.localScale;

//        // Chỉ màu trắng, alpha rõ ràng (ví dụ 0.7 là khá sáng, rõ)
//        shadowSr.color = new Color(1f, 1f, 1f, 0.7f); // R, G, B, A

//        Destroy(shadow, shadowLifespan);
//    }

//}
