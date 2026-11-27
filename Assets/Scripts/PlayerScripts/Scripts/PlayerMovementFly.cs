using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementFly : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField] private float maxSpeed; //Velocidade maxima horizontal permitida para o jogador

    [Header("Input")]
    //Referencia a acao Move do Input Actions Asset
    [SerializeField] private InputActionReference moveAction;

    //Taxas de aceleracao e desaceleracao calculadas no Awake
    private float acceleration;
    private float deceleration;

    //Referencia ao componente de fisica e entrada vertical atual
    private Rigidbody2D body;
    private float moveInput;

    private void Awake()
    {
        //Valores de suavizacao de movimento, os valores que estao sempre vao dar o mesmo tempo para atingir a velocidade maxima,
        //desta forma independente da maxSpeed sempre vai ter uma boa suavizacao
        acceleration = maxSpeed * 5.625f;    //aproximadamente 0.18s para atingir velocidade máxima
        deceleration = maxSpeed * 3.125f;    //aproximadamente 0.32s para parar totalmente

        body = GetComponent<Rigidbody2D>();
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
        //Le o eixo vertical (y) da acao 
        if (moveAction != null && moveAction.action != null)
        {
            moveInput = moveAction.action.ReadValue<Vector2>().y;
        }
        else
        {
            moveInput = 0f;
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
        float accelerationLocal;

        if (hasInput)
        {
            accelerationLocal = acceleration;
        }
        else
        {
            accelerationLocal = deceleration;
        }

        //Le as velocidades atuais para atualizar apenas o eixo horizontal
        float currentHorizontalSpeed = body.linearVelocity.x;
        //Le as velocidades atuais para atualizar apenas o eixo vertical
        float currentVerticalSpeed = body.linearVelocity.y;
        //Variacao velocidade maxima permitida neste passo da fisica
        float maxSpeedChange = acceleration * Time.fixedDeltaTime;
        //Calcula a nova velocidade horizontal aproximando do alvo sem ultrapassar
        float newY = Mathf.MoveTowards(currentVerticalSpeed, targetSpeed, maxSpeedChange);
        //Aplica a nova velocidade horizontal mantendo a velocidade vertical atual
        body.linearVelocity = new Vector2(currentHorizontalSpeed, newY);
    }
}
