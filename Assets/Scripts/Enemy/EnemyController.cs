using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public enum EnemyState { Walk, Roar, Chase, Attack }

    [Header("References")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform player;
    [SerializeField] private Animator animator;

    [Header("Ranges")]
    [SerializeField] private float viewRange = 10f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float roamRadius = 20f;

    [Header("Speeds")]
    [SerializeField] private float walkSpeed = 1.5f;
    [SerializeField] private float chaseSpeed = 3.5f;

    private EnemyState currentState;
    private float roarTimer;

    void Start()
    {
        ChangeState(EnemyState.Walk);
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        switch (currentState)
        {
            case EnemyState.Walk:
                WalkBehaviour(distance);
                break;

            case EnemyState.Roar:
                RoarBehaviour();
                break;

            case EnemyState.Chase:
                ChaseBehaviour(distance);
                break;

            case EnemyState.Attack:
                break;
        }
    }

    void WalkBehaviour(float distance)
    {
        if (!agent.hasPath || agent.remainingDistance < 0.5f)
            SetRandomDestination();

        animator.SetFloat("Speed", agent.velocity.magnitude);

        if (distance <= viewRange)
            ChangeState(EnemyState.Roar);
    }

    void RoarBehaviour()
    {
        roarTimer += Time.deltaTime;

        if (roarTimer >= 1.8f)
            ChangeState(EnemyState.Chase);
    }

    void ChaseBehaviour(float distance)
    {
        agent.SetDestination(player.position);
        animator.SetFloat("Speed", agent.velocity.magnitude);

        if (distance <= attackRange)
            ChangeState(EnemyState.Attack);

        if (distance > viewRange * 1.2f)
            ChangeState(EnemyState.Walk);
    }

    void ChangeState(EnemyState newState)
    {
        currentState = newState;

        switch (newState)
        {
            case EnemyState.Walk:
                agent.speed = walkSpeed;
                roarTimer = 0f;
                SetRandomDestination();
                break;

            case EnemyState.Roar:
                agent.ResetPath();
                roarTimer = 0f;
                animator.SetTrigger("Roar");
                break;

            case EnemyState.Chase:
                agent.speed = chaseSpeed;
                break;

            case EnemyState.Attack:
                agent.ResetPath();
                animator.SetTrigger("Attack");
                GameManager.Instance.LoseGame();
                break;
        }
    }

    void SetRandomDestination()
    {
        Vector3 randomDir = Random.insideUnitSphere * roamRadius;
        randomDir += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDir, out hit, roamRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}