using UnityEngine;

public class Gravity : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField] private float fallMaxSpeedP;

    [Header("References")]
    //Referencia ao script que verifica se o jogador esta no chao
    [SerializeField] private PlayerGroundCheck groundCheck;

    private float fallMaxSpeed;
    private float acceleration;

    private bool IsGrounded { get; set; }

    private Rigidbody2D body;

    private void Awake()
    {
        // Converte automaticamente para negativo para a física funcionar bem
        fallMaxSpeed = -Mathf.Abs(fallMaxSpeedP);
        // Calcula a aceleração
        acceleration = Mathf.Abs(fallMaxSpeedP) * 8.75f;

        body = GetComponent<Rigidbody2D>();
        //Este script deve de travar a rotacao do corpo por defeito so por garantia, se ja nao tiver travado, obs: evitar erros de certas pessoas
        if (body.freezeRotation != true)
        {
            body.freezeRotation = true;
        }
        //Desliga a gravidade do objeto caso ainda nao estaja para utilizar a gravidade do proprio script
        if (body.gravityScale != 0)
        {
            body.gravityScale = 0;
        }
    }

    private void Update()
    {
        //Pega os valores da class PlayerGroundCheck para dar valor ao IsGrounded
        if (groundCheck != null)
        {
            IsGrounded = groundCheck.IsGrounded;
        }
    }

    private void FixedUpdate()
    {
        if (!IsGrounded) {
            //Le as velocidades atuais para atualizar apenas o eixo horizontal
            float currentHorizontalSpeed = body.linearVelocity.x;
            //Le as velocidades atuais para atualizar apenas o eixo vertical
            float currentVerticalSpeed = body.linearVelocity.y;
            //Variacao velocidade maxima permitida neste passo da fisica
            float maxSpeedChange = acceleration * Time.fixedDeltaTime;
            //Calcula a nova velocidade vertical aproximando do alvo sem ultrapassar
            float newY = Mathf.MoveTowards(currentVerticalSpeed, fallMaxSpeed, maxSpeedChange);
            //Aplica a nova velocidade vertical mantendo a velocidade horizontal atual
            body.linearVelocity = new Vector2(currentHorizontalSpeed, newY);
        }
    }
}
