using UnityEngine;

public class movimentacao : MonoBehaviour
{
    [Header("Configurações de Velocidade")]
    public float velocidade = 5f;

    [Header("Referências")]
    public Animator animator;

    void Update()
    {
        // 1. Captura as entradas
        float movimentoHorizontal = Input.GetAxisRaw("Horizontal");
        float movimentoVertical = Input.GetAxisRaw("Vertical");

        Vector2 direcao = new Vector2(movimentoHorizontal, movimentoVertical);

        // 2. Move o personagem
        transform.Translate(direcao.normalized * velocidade * Time.deltaTime);

        // 3. Gerencia as animações
        AtualizarAnimacoes(direcao);
    }

    void AtualizarAnimacoes(Vector2 dir)
    {
        // Se a direção for diferente de zero, o player está se movendo
        if (dir != Vector2.zero)
        {
            animator.SetBool("Walking", true);

            // Atualiza os eixos para a Blend Tree de "Walking"
            animator.SetFloat("Horizontal", dir.x);
            animator.SetFloat("Vertical", dir.y);

            // Guarda a última direção para a Blend Tree de "Idle" saber para onde olhar
            animator.SetFloat("LastHorizontal", dir.x);
            animator.SetFloat("LastVertical", dir.y);
        }
        else
        {
            // O player está parado
            animator.SetBool("Walking", false);
        }
    }
}
