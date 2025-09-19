using UnityEngine;
using UnityEngine.AI;

public class EnemyCntrl : MonoBehaviour
{
    [SerializeField] private GameObject deathExplosionPrefab;

    private readonly float TARGET_INTERVAL = 2.0f;

    private NavMeshAgent navMeshAgent = null;

    private PlayerCntrl playerCntrl = null;

    private EnemyState currentState = EnemyState.IDLE;

    private Animator animator = null;

    private float targetInterval;

    private int health = 3;

    private void Awake()
    {
        targetInterval = TARGET_INTERVAL;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        EventManager.Instance.InvokeOnUpdateEnemyCount(1);
    }

    private void Update()
    {
        switch(currentState)
        {
            case EnemyState.IDLE:
                currentState = State_Idle();
                break;
            case EnemyState.TARGETING:
                currentState = State_Targeting();
                break;
        }
    }

   

    /**
     * State_Idle() - 
     */
    private EnemyState State_Idle()
    {
        EnemyState state = EnemyState.IDLE;

        if (playerCntrl.WithinEnemy(transform.position))
        {
            state = EnemyState.TARGETING;
            animator.SetBool("target", true);
            navMeshAgent.SetDestination(playerCntrl.GetPosition());
            navMeshAgent.isStopped = false;
        }

        return (state);
    }

    /**
     * State_Targeting() - 
     */
    private EnemyState State_Targeting()
    {
        EnemyState state = EnemyState.TARGETING;

        if (targetInterval < 0.0f)
        {
            FollowHero();
            targetInterval = TARGET_INTERVAL;
        } else
        {
            targetInterval -= Time.deltaTime;
            TurnToNextSteeringPoint();
        }

        if (!playerCntrl.WithinEnemy(transform.position))
        {
            state = EnemyState.IDLE;
            animator.SetBool("target", false);
            navMeshAgent.isStopped = true;
        }

        return (state);
    }

    public void FollowHero()
    {
        navMeshAgent.SetDestination(playerCntrl.GetPosition());

        if (AgentHasPath())
        {
            TurnToPoint(navMeshAgent.steeringTarget);
        }

    }

    public void TurnToNextSteeringPoint()
    {
        TurnToPoint(navMeshAgent.steeringTarget);
    }

    public void TurnToPoint(Vector3 target)
    {
        Vector3 direction = target - transform.position;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5.0f);
        }
    }

    public bool AgentHasPath()
    {
        return ((navMeshAgent) && (navMeshAgent.hasPath));
    }

    /**
     * Set() - Determines the destination position of the enemy.
     */
    public void Set(PlayerCntrl playerCntrl)
    {
        this.playerCntrl = playerCntrl;
    }

    private void OnParticleCollision(GameObject other)
    {
        if (--health == 0)
        {
            GameObject explosion = Instantiate(deathExplosionPrefab, transform.position, Quaternion.identity);
            EventManager.Instance.InvokeOnUpdateEnemyCount(-1);
            Destroy(gameObject);
            Destroy(explosion, 4.0f);
        }
    }
}

public enum EnemyState
{
    IDLE,
    TARGETING
}
