using UnityEngine;

public class MiniMapFollowCntrl : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float height;

    private Vector3 offset;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        offset = new Vector3(0.0f, height, 0.0f);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = offset + player.position;
    }
}
