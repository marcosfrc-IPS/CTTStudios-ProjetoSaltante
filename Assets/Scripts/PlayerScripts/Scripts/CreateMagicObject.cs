using UnityEngine;
using UnityEngine.InputSystem;

public class CreateMagicObject : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionReference inputCreateMagic;

    [Header("Player")]
    [SerializeField] private GameObject player;

    [Header("Magic")]
    [SerializeField] public GameObject prefabMagic;
    [SerializeField] private float spawnOffsetX = 1f;

    private void OnEnable()
    {
        if (inputCreateMagic != null && inputCreateMagic.action != null)
        {
            inputCreateMagic.action.Enable();
            inputCreateMagic.action.performed += OnMagicPerformed;
        }
    }

    private void OnDisable()
    {
        if (inputCreateMagic != null && inputCreateMagic.action != null)
        {
            inputCreateMagic.action.performed -= OnMagicPerformed;
        }
    }

    private void OnMagicPerformed(InputAction.CallbackContext context)
    {
        DispararMagia();
    }

    public void DispararMagia()
    {
        if (prefabMagic != null && player != null)
        {
            if (prefabMagic == null || player == null) return;

            float direcao = ObterDirecaoJogador();

            Vector3 posicaoSpawn = player.transform.position;
            posicaoSpawn.x += spawnOffsetX * direcao;
            posicaoSpawn.z = 0f;

            GameObject instancia = Instantiate(prefabMagic, posicaoSpawn, Quaternion.identity);

            MagicObject magic = instancia.GetComponent<MagicObject>();
            if (magic != null)
            {
                Collider2D playerCollider = player.GetComponent<Collider2D>();
                magic.Inicializar(direcao, playerCollider);
            }
        }
    }

    private float ObterDirecaoJogador()
    {
        PlayerMovement movement = player.GetComponent<PlayerMovement>();
        if (movement != null)
        {
            if (Mathf.Abs(movement.moveInput) > 0.01f)
            {
                return Mathf.Sign(movement.moveInput);
            }

            if (movement.spriteRenderer != null && movement.spriteRenderer.flipX)
            {
                return -1f;
            }
        }

        return 1f;
    }
}
