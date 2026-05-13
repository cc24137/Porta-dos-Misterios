using UnityEngine;

public class movimentacao : MonoBehaviour
{
    [Header("Configurações de Velocidade")]
    public float velocidade = 5f;

    void Update()
    {
        // Captura as entradas (WASD ou Setas)
        // O valor varia de -1 a 1
        float movimentoHorizontal = Input.GetAxisRaw("Horizontal");
        float movimentoVertical = Input.GetAxisRaw("Vertical");

        // Cria um vetor de direção baseado nos inputs
        Vector3 direcao = new Vector3(movimentoHorizontal, movimentoVertical, 0f);

        // Normaliza o vetor para que o personagem não ande mais rápido na diagonal
        direcao = direcao.normalized;

        // Move o personagem
        // transform.Translate move o objeto no espaço
        // Time.deltaTime garante que a velocidade seja a mesma em qualquer FPS
        transform.Translate(direcao * velocidade * Time.deltaTime);
    }
}
