using UnityEngine;
using UnityEngine.InputSystem;
using Unity.AI.Navigation;
using UnityEngine.AI;

public class PlayerCntrl : MonoBehaviour
{
    [SerializeField] private GameData gameData;
    [SerializeField] private GameObject lobWeaponPrefab;
    [SerializeField] private GameObject straightWeaponPrefab;
    [SerializeField] private Transform FirePoint;

    private Animator animator;

    private float playerSpeed = 8.0f;
    private float playerRotation = 400.0f;

    private Vector2 playerMove;

    private Vector3 moveDirection;

    private NavMeshAgent navMeshAgent = null;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        animator.SetBool("run", true);
    }

    void Update()
    {
        moveDirection.x = playerMove.x; // * shutOffXAxis; // Horizontal
        moveDirection.y = 0.0f;
        moveDirection.z = playerMove.y; // * shutOffZAxis; // Vertical

        float inputMagnitude = Mathf.Clamp01(moveDirection.magnitude);

        if (moveDirection != Vector3.zero)
        {
            animator.SetBool("run", true);

            moveDirection.Normalize();

            Vector3 velocity = inputMagnitude * playerSpeed * moveDirection;

            navMeshAgent.Move(velocity * Time.deltaTime);

            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, playerRotation * Time.deltaTime);
        } else
        {
            animator.SetBool("run", false);
        }
    }

    public void StopPlayerFromMoving()
    {
        navMeshAgent.isStopped = true;
        navMeshAgent.ResetPath();
    }

    public void StartPlayerFromMoving()
    {
        navMeshAgent.isStopped = false;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        playerMove = context.ReadValue<Vector2>();
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameObject go = Instantiate(lobWeaponPrefab);
            Vector3 angles = transform.rotation.eulerAngles;
            go.transform.localRotation = Quaternion.Euler(-45.0f, angles.y, 0.0f);
            go.transform.position = FirePoint.position;
            Destroy(go, 4.0f);
        }
    }

    public void OnFire1(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameObject go = Instantiate(straightWeaponPrefab);
            Vector3 angles = transform.rotation.eulerAngles;
            go.transform.localRotation = Quaternion.Euler(0.0f, angles.y, 0.0f);
            go.transform.position = FirePoint.position;
            Destroy(go, 4.0f);
        }
    }

    public Vector3 GetPosition()
    {
        return (transform.position);
    }

    public bool WithinEnemy(Vector3 enemyPosition)
    {
        return (Vector3.Distance(enemyPosition, transform.position) < gameData.enemyTargetDistance);
    }
}
