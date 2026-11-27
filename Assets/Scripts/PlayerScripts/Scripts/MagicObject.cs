using UnityEngine;

public class MagicObject : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;

    private float direction = 1.0f;

    private float addForce = 10.0f;
    private float lifeTime = 5.0f;

    private Rigidbody2D body;
    private bool initialized;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        if (body != null)
        {
            body.bodyType = RigidbodyType2D.Dynamic;
            body.gravityScale = 0f;
        }
    }

    public void Inicializar(float direcao, Collider2D playerCollider = null)
    {
        direction = direcao >= 0f ? 1f : -1f;
        initialized = true;

        // Evita que a magia se destrua ao tocar no proprio jogador
        Collider2D magicCollider = GetComponent<Collider2D>();
        if (magicCollider != null && playerCollider != null)
        {
            Physics2D.IgnoreCollision(magicCollider, playerCollider);
        }

        // Vira o sprite da magia se for disparada para a esquerda
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        if (sr != null)
        {
            sr.flipX = direction < 0f;
        }

        AplicarImpulso();
    }

    private void Start()
    {
        if (!initialized)
        {
            AplicarImpulso();
        }

        Destroy(gameObject, lifeTime);
    }

    private void AplicarImpulso()
    {
        if (body != null)
        {
            body.linearVelocity = Vector2.zero;
            Vector2 shotForceDirection = transform.right * (direction * addForce);
            body.AddForce(shotForceDirection, ForceMode2D.Impulse);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
