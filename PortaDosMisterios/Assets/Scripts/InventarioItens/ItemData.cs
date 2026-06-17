using UnityEngine;

[CreateAssetMenu(fileName = "NovoItem", menuName = "Inventario/Item Base")]
public class ItemData : ScriptableObject
{
    public string nomeItem;
    public Sprite icone;
    
    [TextArea]
    public string descricao;

    [Header("Configuração de Descarte")]
    // recebe o prefab do item (que tem o script ItemColetavel)
    public GameObject prefabNoChao; 

    public virtual void Usar(GameObject player)
    {
        Debug.Log($"Item base {nomeItem} usado.");
    }
}