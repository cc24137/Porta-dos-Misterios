using UnityEngine;

public class InputManager : MonoBehaviour
{
    private Inventario inventario;
    private movimentacao movimentacaoPlayer;
    private Abilities habilidades;

    void Start()
    {
        inventario = GetComponent<Inventario>();
        movimentacaoPlayer = GetComponent<movimentacao>();
        habilidades = GetComponent<Abilities>();
    }

    void Update()
    {
        if (GerenciadorUI.Instancia != null && GerenciadorUI.Instancia.estaLendo)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GerenciadorUI.Instancia.FecharTexto();
            }
            else if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.E))
            {
                GerenciadorUI.Instancia.AvancarTexto();
            }
            return;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) inventario.AlternarSelecao(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) inventario.AlternarSelecao(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) inventario.AlternarSelecao(2);

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            if (inventario.slotSelecionado != -1)
            {
                inventario.JogarItemFora(inventario.slotSelecionado);
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (inventario.slotSelecionado != -1)
            {
                inventario.UsarItemEquipado();
            }
            else
            {
                movimentacaoPlayer.VerificarInteracao();
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (habilidades != null)
            {
                habilidades.TentarUsarFlash();
            }
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            if (habilidades != null)
            {
                habilidades.TentarUsarLoudSound();
            }
        }
    }
}
