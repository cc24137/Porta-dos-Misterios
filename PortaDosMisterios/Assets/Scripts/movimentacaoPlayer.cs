using UnityEngine;

public class movimentacao : MonoBehaviour
{
    [Header("Configurações de Velocidade")]
    public float velocidade = 5f;

    [Header("Referências")]
    public Animator animator;

    [Header("Configurações de Interação")]
    public float raioInteracao = 1.5f;
    public LayerMask camadaInteragivel; // Use isso para o player não tentar interagir com o chão

    private IInteragivel objetoDestaqueAtual;

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

        if (Input.GetKeyDown(KeyCode.E))
        {
            VerificarInteracao();
        }
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

    void VerificarInteracao()
    {
        // Cria um círculo invisível ao redor do player para detectar colisores
        Collider2D hit = Physics2D.OverlapCircle(transform.position, raioInteracao, camadaInteragivel);

        if (hit != null)
        {
            // Verifica se o objeto que atingimos tem o script que "assina" a interface
            IInteragivel objeto = hit.GetComponent<IInteragivel>();

            if (objeto != null)
            {
                objeto.Interagir();
            }
        }
    }

    // Desenha o círculo no editor para você ajustar o tamanho visualmente
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, raioInteracao);
    }
}
