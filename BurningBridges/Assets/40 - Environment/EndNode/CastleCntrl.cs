using UnityEngine;

public class CastleCntrl : MonoBehaviour
{
    [SerializeField] private GameObject fireworksPrefab;

    void Start()
    {
        
    }

    private void SetOffFireworks()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject fireworks = Instantiate(fireworksPrefab, transform.position, Quaternion.identity);
            fireworks.SetActive(true);
            Destroy(fireworks, 15.0f);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            SetOffFireworks();

            EventManager.Instance.InvokeOnPlayerWin();
        } 
    }
}
