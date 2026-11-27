using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField] private float maxSpeed; //Velocidade maxima horizontal permitida para o jogador

    [Header("Input")]
    //Referencia a acao Move do Input Actions Asset
    [SerializeField] private InputActionReference moveAction;

    [Header("References")]
    //Referencia ao script que verifica se o jogador esta no chao
    [SerializeField] private PlayerGroundCheck groundCheck;
    [SerializeField] private Animator animator;
    [SerializeField] public SpriteRenderer spriteRenderer;

    //Taxas de aceleracao e desaceleracao calculadas no Awake
    private float groundAcceleration;
    private float groundDeceleration;
    private float airAcceleration;
    private float airDeceleration;

    //Indica se o jogador esta no chao, a verificacao e feita em outro script
    private bool IsGrounded { get; set; }

    //Referencia ao componente de fisica e entrada horizontal atual
    private Rigidbody2D body;
    private float moveInput;

    private void Awake()
    {
        //Valores de suavizacao de movimento, os valores que estao sempre vao dar o mesmo tempo para atingir a velocidade maxima,
        //desta forma independente da maxSpeed sempre vai ter uma boa suavizacao
        groundAcceleration = maxSpeed * 8.75f;  //aproximadamente 0.11s para atingir velocidade máxima
        groundDeceleration = maxSpeed * 11.25f; //aproximadamente 0.09s para parar totalmente
        airAcceleration = maxSpeed * 5.625f;    //aproximadamente 0.18s para atingir velocidade máxima
        airDeceleration = maxSpeed * 3.125f;    //aproximadamente 0.32s para parar totalmente

        body = GetComponent<Rigidbody2D>();
        //Este script deve de travar a rotacao do corpo por defeito so por garantia, se ja nao tiver travado, obs: evitar erros de certas pessoas
        if (body.freezeRotation != true)
        {
            body.freezeRotation = true;
        }
        if(spriteRenderer == null)
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }
    }

    private void OnEnable()
    {
        //Ativa a leitura do input quando o objeto fica ativo
        if (moveAction != null && moveAction.action != null)
        {
            moveAction.action.Enable();
        }
    }

    private void OnDisable()
    {
        //Desativa a leitura do input quando o objeto fica inativo
        if (moveAction != null && moveAction.action != null)
        {
            moveAction.action.Disable();
        }
    }

    private void Update()
    {
        //Le o eixo horizontal (X) da acao 
        if (moveAction != null && moveAction.action != null)
        {
            moveInput = moveAction.action.ReadValue<Vector2>().x;
        }
        else
        {
            moveInput = 0f;
        }
        //Flip do personagem quando ele carrega no A, ou seja anda para tras.
        if(spriteRenderer != null)
        {
            if (moveInput > 0.01f)
            {
                spriteRenderer.flipX = false;
            }
            else if (moveInput < -0.01f)
            {
                spriteRenderer.flipX=true;
            }
        }
        // Controlar a animação de corrida
        if (animator != null)
        {
            animator.SetBool("isRunning", Mathf.Abs(moveInput) > 0.01f);

            // Controlar a animação de salto
            animator.SetBool("Jumping", !IsGrounded);
        }

        //Pega os valores da class PlayerGroundCheck para dar valor ao IsGrounded
        if (groundCheck != null)
        {
            IsGrounded = groundCheck.IsGrounded;
        }
    }

    private void FixedUpdate()
    {
        //Margem minima para ignorar ruidos ou toques residuais no input
        float validInputValue = 0.01f;
        //Velocidade horizontal pretendida com base no input e na velocidade maxima
        float targetSpeed = moveInput * maxSpeed;
        //Indica se ha input horizontal significativo
        bool hasInput = Mathf.Abs(moveInput) > validInputValue;
        //Taxa de variacao de velocidade a aplicar neste ciclo de fisica
        float acceleration;

        if (hasInput) //verifica se existe input se existir a variavel acceleration recebe os valores de aceleracao
        {
            //if para verificar se vai ser utilizados os valores de aceleracao no ar ou no chao
            acceleration = IsGrounded ? groundAcceleration : airAcceleration;
        }
        else //caso a primeira nao se verifique a variavel acceleration recebe os valores de desaceleracao
        {
            //if para verificar se vai ser utilizados os valores de desaceleracao no ar ou no chao
            acceleration = IsGrounded ? groundDeceleration : airDeceleration;
        }

        //Le as velocidades atuais para atualizar apenas o eixo horizontal
        float currentHorizontalSpeed = body.linearVelocity.x;
        //Le as velocidades atuais para atualizar apenas o eixo vertical
        float currentVerticalSpeed = body.linearVelocity.y;
        //Variacao velocidade maxima permitida neste passo da fisica
        float maxSpeedChange = acceleration * Time.fixedDeltaTime;
        //Calcula a nova velocidade horizontal aproximando do alvo sem ultrapassar
        float newX = Mathf.MoveTowards(currentHorizontalSpeed, targetSpeed, maxSpeedChange);
        //Aplica a nova velocidade horizontal mantendo a velocidade vertical atual
        body.linearVelocity = new Vector2(newX, currentVerticalSpeed);
    }
}