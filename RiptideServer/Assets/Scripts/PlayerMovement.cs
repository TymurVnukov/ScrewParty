using RiptideNetworking;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public Player player;
    [SerializeField] private CharacterController controller;
    [SerializeField] private Transform camProxy;
    [SerializeField] private float gravity;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpHeight;
    [SerializeField] public Vector3 tp;
    [SerializeField] public bool isFreeze;
    [SerializeField] public string animationName = "idle";
    [SerializeField] public int playerRotation;
    public GameObject TagCollider;
    public GameObject TagColliderTrigger;

    private float gravityAcceleration;
    private float moveSpeed;
    private float jumpSpeed;

    private bool[] inputs;
    private float yVelocity;

    private void OnValidate()
    {
        if(controller == null)
            controller = GetComponent<CharacterController>();
        if(player == null)
            player = GetComponent<Player>();
        isFreeze = false;
        tp = Vector3.zero;
        Initialize();
    }

    private void Start()
    {
        Initialize();
        inputs = new bool[6];
    }
    private void FixedUpdate()
    {
        TagColliderTrigger.GetComponent<TagColliderTrigger>().isEPressed = inputs[5];
        TagCollider.transform.rotation = Quaternion.Euler(0, playerRotation, 0);
        Vector2 _inputDirection = Vector2.zero;
        if (!isFreeze)
        {
            if (inputs[0] || inputs[1] || inputs[2] || inputs[3])
            {
                animationName = "run";
            }
            else
            {
                animationName = "idle";
            }
            if (inputs[0])
            {
                _inputDirection.y += 1;
                playerRotation = 0;
            }
            if (inputs[1])
            {
                _inputDirection.y -= 1;
                playerRotation = 180;
            }
            if (inputs[2])
            {
                _inputDirection.x -= 1;
                playerRotation = -90;
            }
            if (inputs[3])
            {
                _inputDirection.x += 1;
                playerRotation = 90;
            }
            if(inputs[0] && inputs[2])
            {
                playerRotation = -45;
            }
            if (inputs[0] && inputs[3])
            {
                playerRotation = 45;
            }
            if (inputs[1] && inputs[2])
            {
                playerRotation = 235;
            }
            if (inputs[1] && inputs[3])
            {
                playerRotation = 125;
            }
        }
        Move(_inputDirection, inputs[4]);
    }
    private void Initialize()
    {
        gravityAcceleration = gravity * Time.fixedDeltaTime * Time.fixedDeltaTime;
        moveSpeed = movementSpeed * Time.fixedDeltaTime;
        jumpSpeed = Mathf.Sqrt(jumpHeight * -2f * gravityAcceleration);
    }
    private void Move(Vector2 inputDirection, bool jump)
    {
        Vector3 moveDirection = Vector3.Normalize(camProxy.right * inputDirection.x + Vector3.Normalize(FlattenVector3(camProxy.forward)) * inputDirection.y);
        moveDirection *= moveSpeed;
        if (controller.isGrounded)
        {
            yVelocity = 0f;
            if (jump)
                yVelocity = jumpSpeed;
        }
        yVelocity += gravityAcceleration;
        moveDirection.y = yVelocity;
        controller.Move(moveDirection);
        if(tp != Vector3.zero)
        {
            transform.position = tp;
            tp = Vector3.zero;
        }
        SendMovement();
    }
    private Vector3 FlattenVector3(Vector3 vector)
    {
        vector.y = 0f;
        return vector;
    }
    public void SetInput(bool[] inputs, Vector3 forward)
    {
        this.inputs = inputs;
        camProxy.forward = forward;
    }
    private void SendMovement()
    {
        Message message = Message.Create(MessageSendMode.unreliable, ServerToClientId.playerMovement);
        message.AddUShort(player.Id);
        message.AddVector3(transform.position);
        message.AddVector3(camProxy.forward);
        message.AddString(animationName);
        message.AddInt(playerRotation);
        NetworkManager.Singleton.Server.SendToAll(message);
    }
}
