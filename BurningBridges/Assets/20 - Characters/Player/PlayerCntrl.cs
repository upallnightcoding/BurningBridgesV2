using UnityEngine;
using UnityEngine.InputSystem;
using Unity.AI.Navigation;
using UnityEngine.AI;

public class PlayerCntrl : MonoBehaviour
{
    [SerializeField] private GameData gameData;
    [SerializeField] private GameObject weaponPrefab;
    [SerializeField] private Transform FirePoint;

    private float maximumSpeed = 10.0f;
    private float rotationSpeed = 400.0f;

    private Vector2 playerMove;

    private Vector3 moveDirection;

    private NavMeshAgent navMeshAgent = null;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        moveDirection.x = playerMove.x; // * shutOffXAxis; // Horizontal
        moveDirection.y = 0.0f;
        moveDirection.z = playerMove.y; // * shutOffZAxis; // Vertical

        float inputMagnitude = Mathf.Clamp01(moveDirection.magnitude);

        if (moveDirection != Vector3.zero)
        {
            moveDirection.Normalize();

            Vector3 velocity = inputMagnitude * maximumSpeed * moveDirection;

            navMeshAgent.Move(velocity * Time.deltaTime);

            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        playerMove = context.ReadValue<Vector2>();
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameObject go = Instantiate(weaponPrefab);
            go.transform.localRotation = Quaternion.Euler(-45.0f, 0.0f, 0.0f);
            go.transform.position = FirePoint.position;
        }
    }

    public Vector3 GetPosition()
    {
        return (transform.position);
    }

    public bool WithinEnemy(Vector3 position)
    {
        return (Vector3.Distance(position, transform.position) < gameData.enemyTargetDistance);
    }
}
