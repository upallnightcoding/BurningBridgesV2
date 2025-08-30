using UnityEngine;

public class CameraCntrl : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float damping;

    private Vector3 movePosition;
    private Vector3 delta;
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        delta = Vector3.zero - transform.position;
    }

    void LateUpdate()
    {
        movePosition = player.position - delta;

        transform.position = Vector3.SmoothDamp(transform.position, movePosition, ref velocity, damping);
    }

    private void SetCamera()
    {
        transform.position = player.position - delta;
    }

    private void OnEnable()
    {
        EventManager.Instance.OnPlayerPosition += SetCamera;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnPlayerPosition -= SetCamera;
    }
}
