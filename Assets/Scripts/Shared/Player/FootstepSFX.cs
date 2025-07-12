using UnityEngine;

[RequireComponent(typeof(AudioSource), typeof(PlayerController))]
public class FootstepSFX : MonoBehaviour
{
    [Header("Bộ SFX bước chân")]
    public AudioClip[] walkClips;   // SFX khi đi bộ
    public AudioClip[] runClips;    // SFX khi chạy
    [Range(0f, 0.5f)] public float pitchVariance = 0.05f;

    [Header("Nhịp bước")]
    public float walkInterval = 0.56f;   // khoảng cách giữa 2 tiếng bước chân
    public float runInterval = 0.25f;   // 4   bước / giây

    [Header("SFX đáp đất")]
    public AudioClip[] landClips;               // NEW
    public float minLandVelocity = 1f;         // ngưỡng tốc độ rơi để phát âm thanh, hiện tại là 0 nghĩa là đáp đất nhẹ cũng phát âm thanh

    private AudioSource source;
    private PlayerController pc;
    Rigidbody2D rb;
    private float nextStepTime;
    bool wasGrounded;

    void Awake()
    {
        source = GetComponent<AudioSource>();
        source.playOnAwake = false;
        pc = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>(); // lấy rigidbody để biết tốc độ rơi
    }

    void Update()
    {
        bool grounded = pc.touchingDirections.IsGrounded;

        // 1. Kiểm tra đáp đất
        if (grounded && !wasGrounded)
        {
            // chỉ phát nếu rơi đủ mạnh
            if (rb.linearVelocity.y <= minLandVelocity)
                PlayRandom(landClips);

            // delay 1 chút trước khi cho bước chân tiếp tục
            nextStepTime = Time.time + 0.1f;
        }

        // 2. Phát bước chân bình thường
        if (grounded && pc.IsMoving && Time.time >= nextStepTime)
        {
            PlayRandom(pc.IsRunning ? runClips : walkClips);
            nextStepTime = Time.time + (pc.IsRunning ? runInterval : walkInterval);
        }

        wasGrounded = grounded;
    }

    void PlayRandom(AudioClip[] bank)
    {
        if (bank == null || bank.Length == 0) return;

        int idx = Random.Range(0, bank.Length);
        source.pitch = 1f + Random.Range(-pitchVariance, pitchVariance);
        source.PlayOneShot(bank[idx]);
    }
}
