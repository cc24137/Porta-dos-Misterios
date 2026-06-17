using UnityEngine;

[CreateAssetMenu(fileName = "NovaNota", menuName = "Inventario/Documentos/Nota Coletavel")]
public class NotaData : ItemData
{
    [Header("Conteúdo da Nota")]
    [TextArea(5, 15)]
    public string conteudoDaNota;

    public override void Usar(GameObject player)
    {
        if (GerenciadorUI.Instancia != null)
        {
            GerenciadorUI.Instancia.MostrarTexto(conteudoDaNota);
        }
        else
        {
            Debug.LogWarning("Não foi possível ler a nota porque o GerenciadorUI.Instancia não foi encontrado na cena!");
        }
    }
}
