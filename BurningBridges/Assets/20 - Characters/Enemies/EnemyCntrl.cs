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

    private void OnParticleCollision(GameObject other)
    {
        if (--health == 0)
        {
            GameObject explosion = Instantiate(deathExplosionPrefab, transform.position, Quaternion.identity);
            EventManager.Instance.InvokeOnUpdateEnemyCount(-1);
            Destroy(gameObject);
            Destroy(explosion);
        }

        Debug.Log($"Health: {health}");
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
            navMeshAgent.SetDestination(playerCntrl.GetPosition());
            targetInterval = TARGET_INTERVAL;
        } else
        {
            targetInterval -= Time.deltaTime;
        }

        if (!playerCntrl.WithinEnemy(transform.position))
        {
            state = EnemyState.IDLE;
            animator.SetBool("target", false);
            navMeshAgent.isStopped = true;
        }

        return (state);
    }

    /**
     * Set() - Determines the destination position of the enemy.
     */
    public void Set(PlayerCntrl playerCntrl)
    {
        this.playerCntrl = playerCntrl;
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }
}

public enum EnemyState
{
    IDLE,
    TARGETING
}
