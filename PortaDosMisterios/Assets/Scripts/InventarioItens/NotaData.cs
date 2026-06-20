using UnityEngine;

[CreateAssetMenu(fileName = "NovaNota", menuName = "Inventario/Documentos/Nota Coletavel")]
public class NotaData : ItemData
{
    [Header("Conteúdo da Nota")]
    [TextArea(5, 15)] 
    public string[] paginasDaNota;

    public override void Usar(GameObject player)
    {
        if (GerenciadorUI.Instancia != null)
        {
            // Chama a nova função do GerenciadorUI passando o Array de páginas
            GerenciadorUI.Instancia.AbrirTexto(paginasDaNota);
        }
        else
        {
            Debug.LogWarning("Não foi possível ler a nota porque o GerenciadorUI.Instancia não foi encontrado na cena!");
        }
    }
}
