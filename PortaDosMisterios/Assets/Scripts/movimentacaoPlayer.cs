
using UnityEngine;

public class movimentacao : MonoBehaviour
{
    [Header("Configurações de Velocidade")]
    public float velocidade = 5f;

    [Header("Referências")]
    public Animator animator;
    private Rigidbody2D rb;

    [Header("Configurações de Interação")]
    public float raioInteracao = 1.5f;
    public LayerMask camadaInteragivel;

    private IInteragivel objetoDestaqueAtual;
    
    private Vector2 direcao;
    public Vector2 lastDirection {get; set;}

    void Start()
    {
        // Pega o componente Rigidbody2D anexado ao personagem automaticamente
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Se estiver lendo, impede a movimentação e verifica se quer fechar a UI
        if (GerenciadorUI.Instancia.estaLendo)
        {
            direcao = Vector2.zero;
            rb.linearVelocity = Vector2.zero; // Garante parada imediata na física

            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Escape))
            {
                GerenciadorUI.Instancia.FecharTexto();
            }

            AtualizarAnimacoes(direcao);
            return;
        }

        // Verifica a intenção de interação
        if (Input.GetKeyDown(KeyCode.E))
        {
            VerificarInteracao();
        }

        // 1. Captura as entradas de movimento
        float movimentoHorizontal = Input.GetAxisRaw("Horizontal");
        float movimentoVertical = Input.GetAxisRaw("Vertical");

        // Normaliza o vetor para o personagem não andar mais rápido na diagonal
        direcao = new Vector2(movimentoHorizontal, movimentoVertical).normalized;
        
        if (direcao != Vector2.zero)
        {
            lastDirection = direcao;
        }

        // 2. Atualiza os parâmetros das animações
        AtualizarAnimacoes(direcao);
    }

    void FixedUpdate()
    {
        // 3. Move o personagem aplicando velocidade física (Padrão Unity 6)
        // Isso resolve o problema de atravessar paredes
        rb.linearVelocity = direcao * velocidade;
    }

    void AtualizarAnimacoes(Vector2 dir)
    {
        // Se a direção for diferente de zero, o player está se movendo
        if (dir != Vector2.zero)
        {
            animator.SetBool("Walking", true);

            // Atualiza os eixos para a Blend Tree de caminhada
            animator.SetFloat("Horizontal", dir.x);
            animator.SetFloat("Vertical", dir.y);

            // Guarda a direção do último movimento para a Blend Tree de Idle (parado)
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
        // Cria uma área circular ao redor do player para detectar interagíveis
        Collider2D hit = Physics2D.OverlapCircle(transform.position, raioInteracao, camadaInteragivel);

        if (hit != null)
        {
            // Verifica se o objeto atingido possui a interface de interação
            IInteragivel objeto = hit.GetComponent<IInteragivel>();

            if (objeto != null)
            {
                objeto.Interagir();
            }
        }
    }

    // Desenha o círculo de interação amarelo no editor do Unity para ajuste visual
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, raioInteracao);
    }
}
