using UnityEngine;

public class Livro : MonoBehaviour, IInteragivel
{
    [Header("Conteúdo do Livro")]
    [TextArea(3, 10)]
    public string[] paginas;

    public void Interagir()
    {
        // Envia todas as páginas para o GerenciadorUI
        GerenciadorUI.Instancia.AbrirTexto(paginas);
    }
}
