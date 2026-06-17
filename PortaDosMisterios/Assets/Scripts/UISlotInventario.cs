using UnityEngine;
using UnityEngine.UI;

public class UISlotInventario : MonoBehaviour
{
    [Header("Componentes")]
    public Image imagemDeFundo; // O próprio Slot (a caixa)
    public Image imagemDoIcone; // O objeto filho (o item)

    [Header("Visuais da Caixa")]
    public Sprite fundoNormal;
    public Sprite fundoSelecionado;

    public void AtualizarIcone(Sprite iconeDoItem)
    {
        if (iconeDoItem != null)
        {
            imagemDoIcone.sprite = iconeDoItem; 
            imagemDoIcone.enabled = true; // Mostra o ícone
        }
        else
        {
            imagemDoIcone.sprite = null;
            imagemDoIcone.enabled = false; // Esconde o ícone (slot vazio)
        }
    }

    public void MudarStatusSelecao(bool estaSelecionado)
    {
        // Troca o sprite da caixa dependendo se está selecionada ou não
        imagemDeFundo.sprite = estaSelecionado ? fundoSelecionado : fundoNormal;
    }
}
