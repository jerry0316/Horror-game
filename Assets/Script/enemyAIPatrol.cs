using System.Collections;
using StarterAssets;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIPatrol : MonoBehaviour
{
    GameObject player;
    NavMeshAgent agent;
    [SerializeField] LayerMask groundLayer, playerLayer;
    [SerializeField] float patrolSpeed = 1.0f; // 巡邏速度
    [SerializeField] float chaseSpeed = 3.5f; // 追擊速度
    [SerializeField] AudioClip chaseAudioClip; // 追擊音效
    [SerializeField] AudioClip caughtAudioClip; // 被抓住的音效
    Animator animator;
    BoxCollider boxCollider;

    // 巡邏點
    public Transform[] patrolPoints;
    int currentPatrolIndex = 0;

    // 狀態變更
    [SerializeField] float sightRange, attackRange;
    bool playerInSight, playerInAttackRange;

    // 是否玩家拿到鑰匙
    bool playerHasKey = false;

    // 追擊計時器
    float chaseTimer;
    public float chaseDuration = 10f; // 追擊時間（秒）

    // 音效
    private AudioSource bgmAudioSource;
    private AudioSource chaseAudioSource;

    // "You Died" UI
    public GameObject youDiedUI;

    // 怪物脸图像
    public GameObject monsterFaceImage;
    public float shakeDuration = 0.5f; // 震动时间
    public float shakeMagnitude = 0.1f; // 震动幅度
    private ScreenShake screenShake;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");
        animator = GetComponent<Animator>();
        boxCollider = GetComponentInChildren<BoxCollider>();
        boxCollider.isTrigger = true;
        youDiedUI.SetActive(false);

        if (patrolPoints.Length == 0)
        {
            Debug.LogError("No patrol points set!");
            enabled = false;
            return;
        }

        agent.SetDestination(patrolPoints[currentPatrolIndex].position);

        // 初始化音效來源
        bgmAudioSource = GameObject.Find("BackgroundMusic").GetComponent<AudioSource>();
        chaseAudioSource = gameObject.AddComponent<AudioSource>();
        // 設置 chaseAudioSource 的 clip
        chaseAudioSource.clip = chaseAudioClip;
        chaseAudioSource.volume = 0.03f; // 設置音量
        chaseAudioSource.loop = true; // 重複播放追擊音效

        // 初始化屏幕震动
        screenShake = Camera.main.GetComponent<ScreenShake>();
        if (screenShake == null)
        {
            screenShake = Camera.main.gameObject.AddComponent<ScreenShake>();
        }

        // 初始化怪物脸图像
        monsterFaceImage.SetActive(false);
    }

    void Update()
    {
        playerInSight = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);

        if (playerHasKey)
        {
            Chase();
        }
        else
        {
            if (!playerInSight && !playerInAttackRange)
            {
                Patrol();
            }
            else if (playerInSight && !playerInAttackRange)
            {
                Chase();
            }
            else if (playerInAttackRange)
            {
                Attack();
            }
        }

        // 如果正在追擊，則遞增計時器
        if (playerHasKey)
        {
            chaseTimer += Time.deltaTime;
            if (chaseTimer >= chaseDuration)
            {
                StopChasing();
            }
        }
    }

    void Patrol()
    {
        agent.speed = patrolSpeed;
        if (agent.remainingDistance < 0.5f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        }

        animator.SetBool("isWalking", true);
        animator.SetBool("isRunning", false);
    }

    void Chase()
    {
        agent.SetDestination(player.transform.position);
        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", true);

        // 停止背景音樂，播放追擊音效
        if (!chaseAudioSource.isPlaying)
        {
            bgmAudioSource.Pause();
            chaseAudioSource.Play();
        }
    }

    void Attack()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Z_Attack"))
        {
            agent.SetDestination(transform.position);
            animator.SetTrigger("Attack");
            animator.SetBool("isRunning", false);
        }
    }

    public void EnableAttack()
    {
        boxCollider.enabled = true;
        Debug.Log("Attack enabled");
    }

    public void DisableAttack()
    {
        boxCollider.enabled = false;
        Debug.Log("Attack disabled");
    }

    private void OnTriggerEnter(Collider other)
    {
        var playerController = other.GetComponent<FirstPersonController>();
        if (playerController != null)
        {
            Debug.Log("Hit!");
            StopAllSounds(); // 停止所有音樂
            ShowYouDiedScreen();
            StartCoroutine(HandlePlayerCaught());
        }

        if (other.CompareTag("Key"))
        {
            Debug.Log("Player picked up the key!");
            playerHasKey = true;
            Destroy(other.gameObject);
        }
    }

    IEnumerator HandlePlayerCaught()
    {
        // 停止追擊音效
        if (chaseAudioSource.isPlaying)
        {
            chaseAudioSource.Stop();
        }

        // 显示怪物脸图像
        monsterFaceImage.SetActive(true);

        // 播放被抓住的音效
        AudioSource.PlayClipAtPoint(caughtAudioClip, transform.position);

        // 进行屏幕震动
        yield return StartCoroutine(screenShake.Shake(shakeDuration, shakeMagnitude));

        // 隐藏怪物脸图像
        monsterFaceImage.SetActive(false);
    }

    void ShowYouDiedScreen()
    {
        youDiedUI.SetActive(true);
        Time.timeScale = 0f;

        // 停止所有音效
        StopAllSounds();
    }

    public void StartChasing()
    {
        playerHasKey = true;
        Chase();
        agent.speed = chaseSpeed; // 設置追擊速度
        chaseTimer = 0f; // 重設追擊計時器
    }

    // 停止追擊方法
    public void StopChasing()
    {
        playerHasKey = false; // 停止追擊
        agent.ResetPath(); // 停止移動
        animator.SetBool("isWalking", true); // 回到巡邏狀態
        animator.SetBool("isRunning", false); // 回到巡邏狀態

        // 停止追擊音效，恢復背景音樂
        if (chaseAudioSource.isPlaying)
        {
            chaseAudioSource.Stop();
            if (!bgmAudioSource.isPlaying)
            {
                bgmAudioSource.Play();
            }
        }
    }

    private void StopAllSounds()
    {
        // 停止所有音樂
        Debug.Log("Stopping all sounds.");
        if (bgmAudioSource.isPlaying)
        {
            Debug.Log("Stopping background music.");
            bgmAudioSource.Stop();
        }

        if (chaseAudioSource.isPlaying)
        {
            Debug.Log("Stopping chase music.");
            chaseAudioSource.Stop();
        }
    }
}