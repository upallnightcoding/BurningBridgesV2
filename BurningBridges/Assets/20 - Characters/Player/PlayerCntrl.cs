using UnityEngine;
using UnityEngine.InputSystem;
using Unity.AI.Navigation;
using UnityEngine.AI;

public class PlayerCntrl : MonoBehaviour
{
    [SerializeField] private GameObject weaponPrefab;
    [SerializeField] private Transform FirePoint;

    private float maximumSpeed = 10.0f;
    private float rotationSpeed = 400.0f;

    private Vector2 playerMove;
    private Vector2 prevPlayerMove;

    private Vector3 moveDirection;

    //private int shutOffXAxis = 1;
    //private int shutOffZAxis = 1;

    private NavMeshAgent navMeshAgent = null;

    //private CharacterController charCntrl;

    void Start()
    {
        //charCntrl = GetComponent<CharacterController>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        moveDirection.x = playerMove.x; // * shutOffXAxis; // Horizontal
        moveDirection.y = 0.0f;
        moveDirection.z = playerMove.y; // * shutOffZAxis; // Vertical

        float inputMagnitude = Mathf.Clamp01(moveDirection.magnitude);

        //animator.SetFloat("Speed", inputMagnitude, 0.05f, dt);

        if (moveDirection != Vector3.zero)
        {
            prevPlayerMove = playerMove;

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

    public void EnteringTurningBox()
    {
        Debug.Log("EnteringTurningBox ...");

        //shutOffXAxis = 1;
        //shutOffZAxis = 1;
    }

    public void ExitingTurningBox()
    {
        Debug.Log($"ExitingTurningBox Enter ...{prevPlayerMove}");

        //if (prevPlayerMove.x != 0) shutOffZAxis = 0;
        //if (prevPlayerMove.y != 0) shutOffXAxis = 0;

        /*if (prevPlayerMove.x > prevPlayerMove.y)
        {
            shutOffXAxis = 1;
            shutOffZAxis = 0;
        } else
        {
            shutOffXAxis = 0;
            shutOffZAxis = 1;
        }*/

        //Debug.Log($"ExitingTurningBox Exit ...{shutOffXAxis}/{shutOffZAxis}");
    }

    private void OnEnable()
    {
        EventManager.Instance.OnEnteringTurningBox += EnteringTurningBox;
        EventManager.Instance.OnExitingTurningBox += ExitingTurningBox;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnEnteringTurningBox -= EnteringTurningBox;
        EventManager.Instance.OnExitingTurningBox -= ExitingTurningBox;
    }
}
