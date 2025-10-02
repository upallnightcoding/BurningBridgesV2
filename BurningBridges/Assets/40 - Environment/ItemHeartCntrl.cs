using UnityEngine;

public class ItemHeartCntrl : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            EventManager.Instance.InvokeOnPlayerHit(-5);
            Destroy(gameObject);
        }
    }
}
