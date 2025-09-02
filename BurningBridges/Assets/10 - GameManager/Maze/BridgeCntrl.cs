using System.Collections;
using UnityEngine;

public class BridgeCntrl : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab;

    private bool trigger = false;

    void Start()
    {
        
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
        trigger = true;
    }

    /**
     * DestroyBridge() - Instantiate the explorsion prefab and destroy the
     * bridge game object.  This is called back from the MazeCntrl class
     * when a player has tripped the bridge box collider.
     */
    public void DestroyBridge()
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    /**
     * OnTriggerEnter() - If the game object that has triggered the collision
     * box has a player controller, then send out an event to explode the 
     * bridge.  Enemies will not trigger the bridge to explode, they do not
     * contain a Player Controller component.
     */
    private void OnTriggerEnter(Collider other)
    {
        if (trigger)
        {
            if(other.gameObject.TryGetComponent<PlayerCntrl>(out PlayerCntrl playerCntrl))
            {
                EventManager.Instance.InvokeOnResetPlayer(this);
            }
        }
    }
}
