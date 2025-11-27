using Unity.Netcode;
using UnityEngine;

/// <summary>
/// 负责本地玩家输入与移动，只有 owner 会读取输入，从而避免多个客户端同时控制。
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class NetPlayerMove : NetworkBehaviour
{
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float rotationSpeed = 720f;
    [SerializeField] private float gravity = -9.81f;

    private CharacterController characterController;
    private float verticalVelocity;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (!IsOwner)
        {
            return;
        }

        var input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (input.sqrMagnitude > 1f)
        {
            input.Normalize();
        }

        var move = new Vector3(input.x, 0f, input.y);
        if (move.sqrMagnitude > 0f)
        {
            var targetRot = Quaternion.LookRotation(move, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
        }

        if (characterController.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -1f; // slight downward force keeps controller grounded
        }
        verticalVelocity += gravity * Time.deltaTime;

        var velocity = move * moveSpeed + Vector3.up * verticalVelocity;
        characterController.Move(velocity * Time.deltaTime);
    }
}
