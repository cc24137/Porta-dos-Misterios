using UnityEngine;

public class InteragivelDialogo : MonoBehaviour, IInteragivel
{
    [Header("Conteúdo do diálogo")]
    [TextArea(3, 10)]
    public string[] paginas;
    public bool[] saoPaginas;

    public void Interagir()
    {
        // Envia todas as páginas para o GerenciadorUI
        GerenciadorUI.Instancia.AbrirTexto(paginas);
    }
}
