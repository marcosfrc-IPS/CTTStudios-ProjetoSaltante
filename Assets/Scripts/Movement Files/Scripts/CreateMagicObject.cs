using UnityEngine;
using UnityEngine.InputSystem;

public class CreateMagicObject : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionReference inputCreateMagic;

    [Header("Player")]
    [SerializeField] private GameObject player;

    [SerializeField] public GameObject prefabMagic;

    private void FixedUpdate()
    {
        
    }

    public void DispararMagia()
    {
        if (prefabMagic != null && player != null)
        {
            // Pega a posição atual do player a cada disparo
            Vector3 posicaoSpawn = player.transform.position;

            // Adiciona 1 à frente no eixo X (se quiser que nasça à direita do player)
            posicaoSpawn.x += 1f;

            // Garante que o Z seja 0 para o plano 2D
            posicaoSpawn.z = 0f;

            // Usa a rotação do player ou do próprio script
            Quaternion rotacao = player.transform.rotation;

            // Instancia a magia
            Instantiate(prefabMagic, posicaoSpawn, rotacao);
        }
    }
}
