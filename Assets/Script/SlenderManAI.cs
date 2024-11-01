using UnityEngine;
using UnityEngine.AI;

public class SlenderManAI : MonoBehaviour
{
    public Transform player; // ���a���⪺�Ѧ�
    public float teleportDistance = 10f; // �̤j�����Z��
    public float teleportCooldown = 5f; // �������j�ɶ�
    public float returnCooldown = 10f; // ��^����I�����j�ɶ�
    [Range(0f, 1f)] public float chaseProbability = 0.65f; // �l�v���a�����v
    public float closeRange = 8f; // �a��d�򤺼��������n��
    public AudioClip teleportSound; // ��������
    private AudioSource audioSource;

    public GameObject staticObject; // �R�q�ĪG���C������
    public float staticActivationRange = 5f; // �R�q�ĪG�Ұʪ��d��

    private Vector3 baseTeleportSpot;
    private float teleportTimer;
    private bool returningToBase;
    private bool sanityDeducted = false; // �T�O�C���R�q�ĪG�u���@���z����

    private NavMeshAgent agent; // NavMeshAgent �Ω�ɭ�
    private SanityManager sanityManager; // ���o�z���޲z���Ѧ�

    private void Start()
    {
        baseTeleportSpot = transform.position;
        teleportTimer = teleportCooldown;

        // �T�O AudioSource �s�b
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.clip = teleportSound;

        // �T�O�R�q�ĪG�@�}�l�O������
        if (staticObject != null)
        {
            staticObject.SetActive(false);
        }

        agent = GetComponent<NavMeshAgent>();
        sanityManager = FindObjectOfType<SanityManager>(); // ��� SanityManager
    }

    private void Update()
    {
        if (player == null) return;

        teleportTimer -= Time.deltaTime;

        if (teleportTimer <= 0f)
        {
            if (returningToBase)
            {
                TeleportToBaseSpot(); // ��^����I
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

        // �ˬd���a�Z���ñҰ��R�q�ĪG
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= staticActivationRange)
        {
            if (staticObject != null && !staticObject.activeSelf)
            {
                staticObject.SetActive(true);
                // �����z����
                if (sanityManager != null && !sanityDeducted)
                {
                    sanityManager.DecreaseSanityForStaticEffect(); // �����z����
                    sanityDeducted = true; // ����h������
                }
            }
        }
        else
        {
            if (staticObject != null && staticObject.activeSelf)
            {
                staticObject.SetActive(false);
                sanityDeducted = false; // ���m�����P�_
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
