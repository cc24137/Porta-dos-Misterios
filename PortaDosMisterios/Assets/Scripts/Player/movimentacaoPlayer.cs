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

    private Vector2 direcao;
    public Vector2 lastDirection { get; set; }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (GerenciadorUI.Instancia != null && GerenciadorUI.Instancia.estaLendo)
        {
            direcao = Vector2.zero;
            rb.linearVelocity = Vector2.zero;
            AtualizarAnimacoes(direcao);
            return;
        }

        float movimentoHorizontal = Input.GetAxisRaw("Horizontal");
        float movimentoVertical = Input.GetAxisRaw("Vertical");

        direcao = new Vector2(movimentoHorizontal, movimentoVertical).normalized;

        if (direcao != Vector2.zero)
        {
            lastDirection = direcao;
        }

        AtualizarAnimacoes(direcao);
        VerificarProximidadeDeInteragivel();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = direcao * velocidade;
    }

    void AtualizarAnimacoes(Vector2 dir)
    {
        if (dir != Vector2.zero)
        {
            animator.SetBool("Walking", true);
            animator.SetFloat("Horizontal", dir.x);
            animator.SetFloat("Vertical", dir.y);
            animator.SetFloat("LastHorizontal", dir.x);
            animator.SetFloat("LastVertical", dir.y);
        }
        else
        {
            animator.SetBool("Walking", false);
        }
    }

    public void VerificarProximidadeDeInteragivel()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, raioInteracao, camadaInteragivel);

        if (hit != null)
        {
            IInteragivel objeto = hit.GetComponent<IInteragivel>();

            GerenciadorUI.Instancia.MostrarTeclaDeInteragir();
        }
        else
        {
            GerenciadorUI.Instancia.EsconderTeclaDeInteragir();
        }
    }

    public void VerificarInteracao()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, raioInteracao, camadaInteragivel);

        if (hit != null)
        {
            IInteragivel objeto = hit.GetComponent<IInteragivel>();

            if (objeto != null)
            {
                objeto.Interagir();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, raioInteracao);
    }
}
