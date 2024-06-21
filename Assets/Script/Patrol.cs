using UnityEngine;
using UnityEngine.AI;

public class Patrol : MonoBehaviour
{
    public Transform[] patrolPoints;
    private int currentPatrolIndex;
    private NavMeshAgent agent;
    private Animator animator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        currentPatrolIndex = 0;
        SetDestination();
    }

    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            SetDestination();
        }
    }

    void SetDestination()
    {
        if (patrolPoints.Length > 0)
        {
            agent.isStopped = false;
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
            animator.SetBool("isWalking", true);
        }
    }

    public void StopPatrolling()
    {
        animator.SetBool("isWalking", false);
        agent.isStopped = true;
    }

    public void ResumePatrolling()
    {
        agent.isStopped = false;
        SetDestination();
    }
}