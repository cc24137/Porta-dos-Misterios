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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) AlternarSelecao(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) AlternarSelecao(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) AlternarSelecao(2);

        if (Input.GetKeyDown(KeyCode.E)) // USO DE ITEM
        {
            if (slotSelecionado != -1)
            {
                UsarSlot(slotSelecionado);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            if (slotSelecionado != -1)
            {
                JogarItemFora(slotSelecionado);
            }
        }
    }

    private void AlternarSelecao(int index)
    {
        if (slotSelecionado == index)
        {
            slotSelecionado = -1; // Desseleciona (mãos vazias)
        }
        else
        {
            slotSelecionado = index; // Seleciona o novo slot
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

    private void UsarSlot(int index)
    {
        if (slots[index] != null)
        {
            slots[index].Usar(gameObject);
        }
        else
        {
            Debug.Log($"Você tentou usar o slot {index + 1}, mas ele está vazio.");
        }
    }

    private void JogarItemFora(int index)
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
