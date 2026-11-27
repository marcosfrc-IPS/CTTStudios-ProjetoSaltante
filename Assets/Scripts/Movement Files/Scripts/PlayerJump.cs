using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerJump : MonoBehaviour
{
    [Header("Jump")]
    //Velocidade vertical inicial ao saltar
    [SerializeField] private float jumpSpeed = 12f;

    [Header("Input")]
    [SerializeField] private InputActionReference jumpAction;

    [Header("References")]
    //Referencia ao script que verifica se o jogador esta no chao
    [SerializeField] private PlayerGroundCheck groundCheck;



    private Rigidbody2D body;
    private bool jumpRequested;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        if (jumpAction != null && jumpAction.action != null)
        {
            jumpAction.action.Enable();
            jumpAction.action.performed += OnJumpPerformed;
        }
    }

    private void OnDisable()
    {
        if (jumpAction != null && jumpAction.action != null)
        {
            jumpAction.action.performed -= OnJumpPerformed;
        }
    }

    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        if (groundCheck != null && groundCheck.IsGrounded)
        {
            jumpRequested = true;
        }
    }

    private void FixedUpdate()
    {
        if (jumpRequested)
        {
            jumpRequested = false;
            //Define a velocidade vertical inicial para cima mantendo a velocidade horizontal
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpSpeed);
        }
    }
}
