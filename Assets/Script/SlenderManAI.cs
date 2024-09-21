using UnityEngine;
using UnityEngine.AI;

public class SlenderManAI : MonoBehaviour
{
    public Transform player; // Reference to the player's GameObject
    public float teleportDistance = 10f; // Maximum teleportation distance
    public float teleportCooldown = 5f; // Time between teleportation attempts
    public float returnCooldown = 10f; // Time before returning to base spot
    [Range(0f, 1f)] public float chaseProbability = 0.65f; // Probability of chasing the player
    public float closeRange = 8f; // Range within which the teleport sound plays
    public AudioClip teleportSound; // Reference to the teleport sound effect
    private AudioSource audioSource;

    public GameObject staticObject; // Reference to the "static" GameObject
    public float staticActivationRange = 5f; // Range at which "static" should be activated

    private Vector3 baseTeleportSpot;
    private float teleportTimer;
    private bool returningToBase;

    private NavMeshAgent agent; // Reference to NavMeshAgent (used for boundaries but not movement)

    private void Start()
    {
        baseTeleportSpot = transform.position;
        teleportTimer = teleportCooldown;

        // Get or add an AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Set the teleport sound
        audioSource.clip = teleportSound;

        // Ensure the "static" object is initially turned off
        if (staticObject != null)
        {
            staticObject.SetActive(false);
        }

        // Initialize the NavMeshAgent
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (player == null)
        {
            return;
        }

        teleportTimer -= Time.deltaTime;

        if (teleportTimer <= 0f)
        {
            if (returningToBase)
            {
                TeleportToBaseSpot(); // Return to base using teleport
                teleportTimer = returnCooldown;
                returningToBase = false;
            }
            else
            {
                DecideTeleportAction();
                teleportTimer = teleportCooldown;
            }
        }

        FacePlayer(); // Instantly face the player

        // Check player distance and toggle the "static" object accordingly
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= staticActivationRange)
        {
            if (staticObject != null && !staticObject.activeSelf)
            {
                staticObject.SetActive(true);
            }
        }
        else
        {
            if (staticObject != null && staticObject.activeSelf)
            {
                staticObject.SetActive(false);
            }
        }

        // Play teleport sound if within the close range
        if (distanceToPlayer <= closeRange)
        {
            PlayTeleportSound();
        }
    }

    private void DecideTeleportAction()
    {
        float randomValue = Random.value;

        if (randomValue <= chaseProbability)
        {
            TeleportNearPlayer(); // Teleport to player's location
        }
        else
        {
            TeleportToBaseSpot(); // Teleport back to the base spot
        }
    }

    private void TeleportNearPlayer()
    {
        // Calculate a random position near the player
        Vector3 randomPosition = player.position + Random.insideUnitSphere * teleportDistance;
        randomPosition.y = transform.position.y; // Keep the same Y position

        // Ensure the random position is within the NavMesh bounds
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPosition, out hit, teleportDistance, NavMesh.AllAreas))
        {
            transform.position = hit.position; // Instant teleport to the valid NavMesh position
        }
        else
        {
            transform.position = player.position; // Fallback: teleport directly to player
        }

        // Check if sound should play depending on proximity
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= closeRange)
        {
            PlayTeleportSound();
        }
    }

    private void TeleportToBaseSpot()
    {
        // Teleport to the initial base spot
        NavMeshHit hit;
        if (NavMesh.SamplePosition(baseTeleportSpot, out hit, 1.0f, NavMesh.AllAreas))
        {
            transform.position = hit.position;
        }
        else
        {
            transform.position = baseTeleportSpot; // Fallback: directly set position
        }

        returningToBase = true;

        // Check if sound should play depending on proximity
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= closeRange)
        {
            PlayTeleportSound();
        }
    }

    private void FacePlayer()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0f; // Ignore the vertical component

        if (directionToPlayer != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = targetRotation; // Instantly face the player
        }
    }

    private void PlayTeleportSound()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play(); // Play sound only if it is not already playing
        }
    }
}
