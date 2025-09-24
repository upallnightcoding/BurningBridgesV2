using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BridgeCntrl : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private GameObject bridgePrefab;

    private NavMeshObstacle navMeshObstacle;

    private bool bridgeTrigger = false;

    private bool alreadyBlown = false;

    void Start()
    {
        navMeshObstacle = GetComponent<NavMeshObstacle>();
    }

    void Update()
    {
        
    }

    /**
     * SetTrigger() - Set the trigger so that the bridge can be destroyed. If
     * the trigger is not set, the OnTriggerEnter method is skipped.
     */
    public void SetTrigger()
    {
        bridgeTrigger = true;

        if (navMeshObstacle)
        {
            navMeshObstacle.carving = true;
        }
    }

    /**
     * DestroyBridge() - Instantiate the explorsion prefab and destroy the
     * bridge game object.  This is called back from the MazeCntrl class
     * when a player has tripped the bridge box collider.
     */
    public void DestroyBridge()
    {
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(explosion, 4.0f);
        //Destroy(gameObject);
        bridgePrefab.SetActive(false);
        alreadyBlown = true;
    }

    /**
     * OnTriggerEnter() - If the game object that has triggered the collision
     * box has a player controller, then send out an event to explode the 
     * bridge.  Enemies will not trigger the bridge to explode, they do not
     * contain a Player Controller component.
     */
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerCntrl>(out PlayerCntrl playerCntrl))
        {
            if (bridgeTrigger)
            {
                if (!alreadyBlown)
                {
                    EventManager.Instance.InvokeOnResetPlayer(this);
                }
            } else
            {
                navMeshObstacle.enabled = false;

                int layerMask = LayerMask.GetMask("Skull");
                Collider[] hitColliders = Physics.OverlapSphere(other.transform.position, 10.0f, layerMask);

                foreach (Collider collider in hitColliders)
                {
                    collider.gameObject.GetComponent<DungeonSkullCntrl>().FireSkull(playerCntrl.GetPosition());
                }
            }
        }
    }
}
