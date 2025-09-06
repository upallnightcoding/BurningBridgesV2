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

    private Collider[] enemies = null;
    private Vector3 aimDirection;

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
            //go.transform.localRotation = Quaternion.Euler(0.0f, angles.y, 0.0f);
            go.transform.localRotation = AssistedAiming(angles.y);
            go.transform.position = FirePoint.position;
            Destroy(go, 4.0f);
        }
    }

    private Quaternion AssistedAiming(float playerAngle)
    {
        aimDirection = EulerToDirection(new Vector3(0.0f, playerAngle, 0.0f));

        LayerMask layerMask = LayerMask.GetMask("Enemy");

        enemies = Physics.OverlapSphere(transform.position, 8.0f, layerMask);

        Transform bestTarget = null;
        float bestAngle = 45.0f;

        foreach(var enemy in enemies)
        {
            Vector3 dirToEnemy = (enemy.transform.position - transform.position).normalized;
            float angle = Vector3.Angle(dirToEnemy, aimDirection);

            Debug.Log($"Angle: {angle}");

            if (angle < bestAngle)
            {
                bestAngle = angle;
                bestTarget = enemy.transform;
            }
        }

        float aimAngle = (bestTarget != null) ? bestAngle + playerAngle : playerAngle;

        Debug.Log($"aimAngle: {aimAngle}/{playerAngle}/{enemies.Length}/{bestAngle}");

        return (Quaternion.Euler(0.0f, aimAngle, 0.0f));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if (enemies != null)
        {
            Gizmos.color = Color.red;
            foreach (var enemy in enemies) { 
                Gizmos.DrawLine(transform.position, enemy.transform.position);
            }

            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + aimDirection * 3.0f);
        }
    }

    private Vector3 EulerToDirection(Vector3 euler)
    {
        float pitch = euler.x * Mathf.Deg2Rad;
        float yaw   = euler.y * Mathf.Deg2Rad;

        float x = Mathf.Cos(pitch) * Mathf.Cos(yaw);
        float y = Mathf.Sin(pitch);
        float z = Mathf.Cos(pitch) * Mathf.Sin(yaw);

        return (new Vector3(x, y, z).normalized);
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
