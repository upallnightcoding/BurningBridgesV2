using UnityEngine;

public class CameraCntrl : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float offset;
    [SerializeField] private float damping;

    private Vector3 movePosition;
    private Vector3 delta;
    private Vector3 velocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        delta.Set(0.0f, offset, 0.0f);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        movePosition = player.position + delta;

        transform.position = Vector3.SmoothDamp(transform.position, movePosition, ref velocity, damping);
    }
}
