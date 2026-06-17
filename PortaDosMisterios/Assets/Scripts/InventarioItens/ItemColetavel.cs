using UnityEngine;

public class ItemColetavel : MonoBehaviour, IInteragivel
{
    [Header("Qual é este item?")]
    public ItemData dadosDoItem;

    public void Interagir()
    {
        GameObject player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            Inventario inventario = player.GetComponent<Inventario>();

            if (inventario != null)
            {
                bool pegou = inventario.AdicionarItem(dadosDoItem);

                if (pegou)
                {
                    Debug.Log($"Coletado com sucesso: {dadosDoItem.nomeItem}");
                    Destroy(gameObject);
                }
            }
        }
        else
        {
            Debug.LogWarning("O item tentou ser pego, mas não achou nenhum objeto com a Tag 'Player' na cena!");
        }
    }
}
