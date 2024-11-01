using UnityEngine;
using UnityEngine.AI;

public class SlenderManAI : MonoBehaviour
{
    public Transform player; // 玩家角色的參考
    public float teleportDistance = 10f; // 最大瞬移距離
    public float teleportCooldown = 5f; // 瞬移間隔時間
    public float returnCooldown = 10f; // 返回基準點的間隔時間
    [Range(0f, 1f)] public float chaseProbability = 0.65f; // 追逐玩家的機率
    public float closeRange = 8f; // 靠近範圍內播放瞬移聲音
    public AudioClip teleportSound; // 瞬移音效
    private AudioSource audioSource;

    public GameObject staticObject; // 靜電效果的遊戲物件
    public float staticActivationRange = 5f; // 靜電效果啟動的範圍

    private Vector3 baseTeleportSpot;
    private float teleportTimer;
    private bool returningToBase;
    private bool sanityDeducted = false; // 確保每次靜電效果只扣一次理智值

    private NavMeshAgent agent; // NavMeshAgent 用於界限
    private SanityManager sanityManager; // 取得理智管理的參考

    private void Start()
    {
        baseTeleportSpot = transform.position;
        teleportTimer = teleportCooldown;

        // 確保 AudioSource 存在
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.clip = teleportSound;

        // 確保靜電效果一開始是關閉的
        if (staticObject != null)
        {
            staticObject.SetActive(false);
        }

        agent = GetComponent<NavMeshAgent>();
        sanityManager = FindObjectOfType<SanityManager>(); // 找到 SanityManager
    }

    private void Update()
    {
        if (player == null) return;

        teleportTimer -= Time.deltaTime;

        if (teleportTimer <= 0f)
        {
            if (returningToBase)
            {
                TeleportToBaseSpot(); // 返回基準點
                teleportTimer = returnCooldown;
                returningToBase = false;
            }
            else
            {
                DecideTeleportAction();
                teleportTimer = teleportCooldown;
            }
        }

        FacePlayer();

        // 檢查玩家距離並啟動靜電效果
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= staticActivationRange)
        {
            if (staticObject != null && !staticObject.activeSelf)
            {
                staticObject.SetActive(true);
                // 扣除理智值
                if (sanityManager != null && !sanityDeducted)
                {
                    sanityManager.DecreaseSanityForStaticEffect(); // 扣除理智值
                    sanityDeducted = true; // 防止多次扣除
                }
            }
        }
        else
        {
            if (staticObject != null && staticObject.activeSelf)
            {
                staticObject.SetActive(false);
                sanityDeducted = false; // 重置扣除判斷
            }
        }

        if (distanceToPlayer <= closeRange)
        {
            PlayTeleportSound();
        }
    }

    private void DecideTeleportAction()
    {
        if (Random.value <= chaseProbability)
        {
            TeleportNearPlayer();
        }
        else
        {
            TeleportToBaseSpot();
        }
    }

    private void TeleportNearPlayer()
    {
        Vector3 randomPosition = player.position + Random.insideUnitSphere * teleportDistance;
        randomPosition.y = transform.position.y;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPosition, out hit, teleportDistance, NavMesh.AllAreas))
        {
            transform.position = hit.position;
        }
        else
        {
            transform.position = player.position;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= closeRange)
        {
            PlayTeleportSound();
        }
    }

    private void TeleportToBaseSpot()
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(baseTeleportSpot, out hit, 1.0f, NavMesh.AllAreas))
        {
            transform.position = hit.position;
        }
        else
        {
            transform.position = baseTeleportSpot;
        }

        returningToBase = true;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= closeRange)
        {
            PlayTeleportSound();
        }
    }

    private void FacePlayer()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0f;

        if (directionToPlayer != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = targetRotation;
        }
    }

    private void PlayTeleportSound()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}
