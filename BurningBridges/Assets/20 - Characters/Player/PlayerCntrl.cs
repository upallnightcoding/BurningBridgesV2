using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCntrl : MonoBehaviour
{
    private float maximumSpeed = 20.0f;
    private float rotationSpeed = 400.0f;

    private Vector2 playerMove;

    private Vector3 moveDirection;

    private CharacterController charCntrl;

    void Start()
    {
        charCntrl = GetComponent<CharacterController>();
    }

    void Update()
    {
        moveDirection.x = playerMove.x; // Horizontal
        moveDirection.y = 0.0f;
        moveDirection.z = playerMove.y; // Vertical

        float inputMagnitude = Mathf.Clamp01(moveDirection.magnitude);

        //animator.SetFloat("Speed", inputMagnitude, 0.05f, dt);

        if (moveDirection != Vector3.zero)
        {
            Debug.Log($"PlayerMove: {moveDirection}");

            moveDirection.Normalize();

            Vector3 velocity = inputMagnitude * maximumSpeed * moveDirection;

            charCntrl.Move(velocity * Time.deltaTime);

            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);

            Debug.Log($"Move: {transform.position}");
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        playerMove = context.ReadValue<Vector2>();

        
    }
}
