using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    [Header("Ground Check")]
    //Objeto vazio colocado nos pes do jogador
    [SerializeField] private Transform groundCheckPoint;
    //Tamanho do circulo que verifica se esta a tocar no chao
    [SerializeField] private float groundCheckRadius = 0.2f;
    //Camada (Layer) que conta como chao
    [SerializeField] private LayerMask groundLayer;

    //Permite que outros scripts leiam se esta no chao
    public bool IsGrounded { get; private set; }

    private void FixedUpdate()
    {
        if (groundCheckPoint != null)
        {
            IsGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundLayer);
        }
    }

    //Desenha um circulo vermelho na Scene View para ajudar a posicionar os pes
    private void OnDrawGizmos()
    {
        if (groundCheckPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheckPoint.position, groundCheckRadius);
    }
}
