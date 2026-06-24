using UnityEngine;
using TMPro;

public class GerenciadorUI : MonoBehaviour
{
    public static GerenciadorUI Instancia { get; private set; }

    public bool estaLendo = false;

    [Header("Configurações de Texto")]
    public GameObject painelTexto;
    public TextMeshProUGUI componenteTexto;
    public GameObject paiDoTextoDeInteracao;

    public Inventario inventario;

    private string[] paginasAtuais;
    private int indexPagina;

    void Awake()
    {
        if (Instancia == null) Instancia = this;
        else Destroy(gameObject);
    }

    public void AbrirTexto(string[] novasPaginas)
    {
        if (novasPaginas == null || novasPaginas.Length == 0) return;

        paginasAtuais = novasPaginas;
        indexPagina = 0; // Começa sempre na primeira página
        estaLendo = true;

        painelTexto.SetActive(true);
        MostrarPaginaAtual();
    }

    public void AvancarTexto()
    {
        indexPagina++; // Vai para a próxima página

        // Verifica se ainda tem páginas para mostrar
        if (indexPagina < paginasAtuais.Length)
        {
            MostrarPaginaAtual();
        }
        else
        {
            // Se as páginas acabaram, fecha o livro
            FecharTexto();
        }
    }

    private void MostrarPaginaAtual()
    {
        // Atualiza o texto na tela com base no índice atual
        componenteTexto.text = paginasAtuais[indexPagina];
    }

    public void FecharTexto()
    {
        estaLendo = false;
        painelTexto.SetActive(false);
        inventario.AlternarSelecao(-1);
    }

    public void MostrarTeclaDeInteragir()
    {
        paiDoTextoDeInteracao.SetActive(true);
    }

    public void EsconderTeclaDeInteragir()
    {
        paiDoTextoDeInteracao.SetActive(false);
    }
    
}
