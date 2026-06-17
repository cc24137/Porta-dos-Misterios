using UnityEngine;

public class UIInventario : MonoBehaviour
{
    [Header("Referências")]
    public Inventario inventarioDoPlayer;
    public UISlotInventario[] slotsUI; // Array com os 3 scripts dos slots

    void Start()
    {
        if (inventarioDoPlayer != null)
        {
            // Inscreve nos eventos do jogador
            inventarioDoPlayer.OnItemAlterado += AtualizarIconeDoSlot;
            inventarioDoPlayer.OnSlotSelecionado += AtualizarSelecaoVisual;
        }
    }

    void OnDestroy()
    {
        if (inventarioDoPlayer != null)
        {
            inventarioDoPlayer.OnItemAlterado -= AtualizarIconeDoSlot;
            inventarioDoPlayer.OnSlotSelecionado -= AtualizarSelecaoVisual;
        }
    }

    private void AtualizarIconeDoSlot(int index, ItemData item)
    {
        // Se pegou um item, manda o ícone; se perdeu, manda nulo
        Sprite icone = item != null ? item.icone : null;
        slotsUI[index].AtualizarIcone(icone);
    }

    private void AtualizarSelecaoVisual(int indexSelecionado)
    {
        // Varre todos os slots e avisa se eles são o escolhido da vez ou não
        for (int i = 0; i < slotsUI.Length; i++)
        {
            bool ehOSelecionado = (i == indexSelecionado);
            slotsUI[i].MudarStatusSelecao(ehOSelecionado);
        }
    }
}
