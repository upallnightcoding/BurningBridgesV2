using UnityEngine;

public class CastleCntrl : MonoBehaviour
{
    [SerializeField] private GameObject fireworksPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetOffFireworks()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject fireworks = Instantiate(fireworksPrefab, transform.position, Quaternion.identity);
            fireworks.SetActive(true);
            Destroy(fireworks, 5.0f);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            Debug.Log("Fireworks Called ...");
            SetOffFireworks();
        } else
        {
            Debug.Log("Fireworks NOT Called ...");
        }
    }
}
