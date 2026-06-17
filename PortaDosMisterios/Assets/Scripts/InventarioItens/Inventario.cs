using UnityEngine;
using System;

public class Inventario : MonoBehaviour
{
    [Header("Slots do Inventário")]
    public ItemData[] slots = new ItemData[3];

    public int slotSelecionado = -1;

    public event Action<int, ItemData> OnItemAlterado;
    public event Action<int> OnSlotSelecionado;

    void Start()
    {
        OnSlotSelecionado?.Invoke(slotSelecionado);
    }

    public void AlternarSelecao(int index)
    {
        if (slotSelecionado == index)
        {
            slotSelecionado = -1;
        }
        else
        {
            slotSelecionado = index;
        }

        OnSlotSelecionado?.Invoke(slotSelecionado);
    }

    public bool AdicionarItem(ItemData novoItem)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
            {
                slots[i] = novoItem;
                OnItemAlterado?.Invoke(i, novoItem);
                return true;
            }
        }
        Debug.Log("Inventário cheio!");
        return false;
    }

    public void UsarItemEquipado()
    {
        if (slotSelecionado != -1 && slots[slotSelecionado] != null)
        {
            slots[slotSelecionado].Usar(gameObject);
        }
        else
        {
            Debug.Log("Nenhum item equipado ou slot vazio.");
        }
    }

    public void JogarItemFora(int index)
    {
        if (slots[index] == null) return;

        ItemData itemParaDropar = slots[index];

        if (itemParaDropar.prefabNoChao != null)
        {
            Instantiate(itemParaDropar.prefabNoChao, transform.position, Quaternion.identity);
        }

        slots[index] = null;
        OnItemAlterado?.Invoke(index, null);

        AlternarSelecao(index);
    }
}
