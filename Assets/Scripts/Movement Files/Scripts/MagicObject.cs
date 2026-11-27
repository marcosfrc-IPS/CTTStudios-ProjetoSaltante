using UnityEngine;

public class MagicObject : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;

    private int direction = 1;

    private float addForce = 10.0f;
    private float lifeTime = 5.0f;

    private Rigidbody2D body;

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        if (body != null )
        {
            if (playerMovement.spriteRenderer.flipX == true)
            {
                direction = -1;
            }

            var shotForceDirection = direction*(transform.right * addForce);
            body.AddForce(shotForceDirection, ForceMode2D.Impulse);
        }

        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
