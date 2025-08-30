using UnityEngine;
using UnityEngine.AI;

public class EnemyCntrl : MonoBehaviour
{
    private NavMeshAgent navMeshAgent = null;

    private Vector3 position;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.SetDestination(position);
    }

    void Update()
    {

    }

    /**
     * Set() - Determines the destination position of the enemy.
     */
    public void Set(Vector3 position)
    {
        this.position = position;
    }
}
