using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float acceleration = 20f;
    [SerializeField] private float gravity = -9.81f;

    private CharacterController controller;
    private Vector3 moveVelocity;
    private float verticalSpeed;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 input = new Vector3(horizontal, 0f, vertical);
        if (input.sqrMagnitude > 1f)
            input.Normalize();

        Vector3 desired = input * moveSpeed;
        moveVelocity = Vector3.MoveTowards(moveVelocity, desired, acceleration * Time.deltaTime);

        if (controller.isGrounded && verticalSpeed < 0f)
            verticalSpeed = -1f;
        verticalSpeed += gravity * Time.deltaTime;

        Vector3 motion = new Vector3(moveVelocity.x, 0f, moveVelocity.z) + Vector3.up * verticalSpeed;
        controller.Move(motion * Time.deltaTime);
    }
}
