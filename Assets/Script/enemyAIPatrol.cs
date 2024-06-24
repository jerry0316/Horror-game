using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using StarterAssets;

public class EnemyAIPatrol : MonoBehaviour
{
    GameObject player;
    NavMeshAgent agent;
    [SerializeField] LayerMask groundLayer, playerLayer;
    [SerializeField] float patrolSpeed = 1.0f; // 巡逻速度
    [SerializeField] float chaseSpeed = 3.5f; // 追击速度
    [SerializeField] AudioClip chaseAudioClip; // 追击音效
    [SerializeField] AudioClip caughtAudioClip; // 被抓住的音效
    Animator animator;
    BoxCollider boxCollider;

    // 巡逻点
    public Transform[] patrolPoints;
    int currentPatrolIndex = 0;

    // 状态变更
    [SerializeField] float sightRange, attackRange;
    bool playerInSight, playerInAttackRange;

    // 是否玩家拿到钥匙
    bool playerHasKey = false;

    // 追击计时器
    float chaseTimer;
    public float chaseDuration = 10f; // 追击时间（秒）

    // 音效
    private AudioSource bgmAudioSource;
    private AudioSource chaseAudioSource;

    // "You Died" UI
    public GameObject youDiedUI;

    // 怪物脸图像
    public GameObject monsterFaceImage;

    // 主角被攻击次数
    private int playerHitCount = 5;
    private const int MaxPlayerHits = 5; // 最大攻击次数

    // 血量显示
    public HealthDisplay healthDisplay;

    // 第一次攻击提示
    public TextMeshProUGUI firstHitText; // 使用 TextMeshProUGUI 组件
    private bool hasShownFirstHitText = false;
    public float firstHitTextDuration = 2f; // 提示显示时间

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");
        animator = GetComponent<Animator>();
        boxCollider = GetComponentInChildren<BoxCollider>();
        boxCollider.isTrigger = true;
        youDiedUI.SetActive(false);
        firstHitText.enabled = false; // 默认隐藏提示文字

        if (patrolPoints.Length == 0)
        {
            Debug.LogError("No patrol points set!");
            enabled = false;
            return;
        }

        agent.SetDestination(patrolPoints[currentPatrolIndex].position);

        // 初始化音效来源
        bgmAudioSource = GameObject.Find("BackgroundMusic").GetComponent<AudioSource>();
        chaseAudioSource = gameObject.AddComponent<AudioSource>();
        // 设置 chaseAudioSource 的 clip
        chaseAudioSource.clip = chaseAudioClip;
        chaseAudioSource.volume = 0.03f; // 设置音量
        chaseAudioSource.loop = true; // 重复播放追击音效

        // 检查所有关键变量是否正确设置
        if (player == null)
        {
            Debug.LogError("Player not found!");
        }
        if (bgmAudioSource == null)
        {
            Debug.LogError("BackgroundMusic AudioSource not found!");
        }
        if (chaseAudioSource == null)
        {
            Debug.LogError("Chase AudioSource not found!");
        }
        if (firstHitText == null)
        {
            Debug.LogError("FirstHitText not found!");
        }
        if (healthDisplay == null)
        {
            Debug.LogError("HealthDisplay not found!");
        }

        // 初始化血量显示
        healthDisplay.UpdateHealth(playerHitCount, MaxPlayerHits);
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

        // 如果正在追击，则递增计时器
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

        // 停止背景音乐，播放追击音效
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

    // 重置攻击
    public void ResetAfterAttack()
    {
        if (playerInSight)
        {
            Chase();
        }
        else
        {
            Patrol();
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
        // 确保 FirstPersonController 类类型已正确引用
        var playerController = other.GetComponent<FirstPersonController>();
        if (playerController != null)
        {
            playerHitCount--;
            Debug.Log("Hit! Player hit count: " + playerHitCount);
            healthDisplay.UpdateHealth(playerHitCount, MaxPlayerHits);

            if (playerHitCount == 1 && !hasShownFirstHitText)
            {
                StartCoroutine(ShowFirstHitText());
            }

            if (playerHitCount <= 0)
            {
                Debug.Log("Player has been hit maximum times!");
                StopAllSounds(); // 停止所有音乐
                ShowYouDiedScreen();
                StartCoroutine(HandlePlayerCaught());
            }
        }

        if (other.CompareTag("Key"))
        {
            Debug.Log("Player picked up the key!");
            playerHasKey = true;
            Destroy(other.gameObject);
        }
    }

    IEnumerator ShowFirstHitText()
    {
        hasShownFirstHitText = true;
        firstHitText.enabled = true;
        yield return new WaitForSeconds(firstHitTextDuration);
        firstHitText.enabled = false;
    }

    IEnumerator HandlePlayerCaught()
    {
        // 停止追击音效
        if (chaseAudioSource.isPlaying)
        {
            chaseAudioSource.Stop();
        }

        // 显示怪物脸图像
        monsterFaceImage.SetActive(true);

        // 播放被抓住的音效
        AudioSource.PlayClipAtPoint(caughtAudioClip, transform.position);

        // 等待 1 秒钟
        yield return new WaitForSeconds(1f);
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
        agent.speed = chaseSpeed; // 设置追击速度
        chaseTimer = 0f; // 重设追击计时器
    }

    // 停止追击方法
    public void StopChasing()
    {
        playerHasKey = false; // 停止追击
        agent.ResetPath(); // 停止移动
        animator.SetBool("isWalking", true); // 回到巡逻状态
        animator.SetBool("isRunning", false); // 回到巡逻状态

        // 停止追击音效，恢复背景音乐
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
        // 停止所有音乐
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
