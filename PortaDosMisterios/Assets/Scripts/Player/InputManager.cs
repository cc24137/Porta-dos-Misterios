using UnityEngine;
using UnityEngine.Events;

using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    public UnityEvent eventoSomPagina;
    private Inventario inventario;
    private movimentacao movimentacaoPlayer;
    private Abilities habilidades;

    // Trava para o D-Pad do controle não alternar de slot várias vezes ao ser pressionado
    private bool dpadSegurado = false;

    void Start()
    {
        inventario = GetComponent<Inventario>();
        movimentacaoPlayer = GetComponent<movimentacao>();
        habilidades = GetComponent<Abilities>();
    }

    void Update()
    {
        // reset do jogo
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        // ==========================================
        // MODO DE LEITURA (UI / DIÁLOGO)
        // ==========================================
        if (GerenciadorUI.Instancia != null && GerenciadorUI.Instancia.estaLendo)
        {
            // ESC (Teclado) | B ou Start (Controle)
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1) || Input.GetKeyDown(KeyCode.JoystickButton7))
            {
                GerenciadorUI.Instancia.FecharTexto();
            }
            // Espaço ou E (Teclado) | A / Cross (Controle)
            else if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton0))
            {
                eventoSomPagina?.Invoke();
                GerenciadorUI.Instancia.AvancarTexto();
            }
            return;
        }

        // ==========================================
        // SELEÇÃO DE INVENTÁRIO (SLOTS 0, 1, 2)
        // ==========================================
        // Teclas 1, 2, 3 no Teclado
        if (Input.GetKeyDown(KeyCode.Alpha1)) inventario.AlternarSelecao(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) inventario.AlternarSelecao(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) inventario.AlternarSelecao(2);

        // Suporte ao D-Pad (Setas do Controle)
        VerificarDPadInventario();

        // ==========================================
        // JOGAR ITEM FORA
        // ==========================================
        // Tecla Z (Teclado) | Botão Y / Triangle (Controle)
        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.JoystickButton3))
        {
            if (inventario.slotSelecionado != -1)
            {
                inventario.JogarItemFora(inventario.slotSelecionado);
            }
        }

        // ==========================================
        // INTERAGIR / USAR ITEM
        // ==========================================
        // Tecla E (Teclado) | Botão A / Cross (Controle)
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton0))
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

        // ==========================================
        // HABILIDADES
        // ==========================================
        // Flash: Tecla F | Botão X / Square (Controle)
        if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.JoystickButton2))
        {
            if (habilidades != null)
            {
                habilidades.TentarUsarFlash();
            }
        }

        // Som Alto (Loud Sound): Tecla L | Botão RB / R1 (Controle)
        if (Input.GetKeyDown(KeyCode.L) || Input.GetKeyDown(KeyCode.JoystickButton5))
        {
            if (habilidades != null)
            {
                habilidades.TentarUsarLoudSound();
            }
        }

        // Porta Ilusória: Tecla Q | Botão LB / L1 (Controle)
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.JoystickButton4))
        {
            if (habilidades != null)
            {
                habilidades.TentarUsarPortaIlusoria();
            }
        }
    }

    private void VerificarDPadInventario()
    {
        float dpadX = 0f;
        float dpadY = 0f;

        // Tenta ler os eixos específicos do D-Pad (Setas do controle)
        try
        {
            dpadX = Input.GetAxisRaw("D-Pad Horizontal");
            dpadY = Input.GetAxisRaw("D-Pad Vertical");
        }
        catch
        {
            // Se o D-Pad ainda não foi configurado na Unity, não faz nada
            // e NÃO usa o analógico de movimento como reserva!
            return;
        }

        // Se o jogador soltou as setas do D-Pad, destrava para o próximo clique
        if (Mathf.Abs(dpadX) < 0.5f && Mathf.Abs(dpadY) < 0.5f)
        {
            dpadSegurado = false;
            return;
        }

        // Troca o slot do inventário de acordo com a seta pressionada
        if (!dpadSegurado)
        {
            if (dpadX < -0.5f)
            {
                inventario.AlternarSelecao(0); // Seta Esquerda = Slot 0
                dpadSegurado = true;
            }
            else if (dpadX > 0.5f)
            {
                inventario.AlternarSelecao(2); // Seta Direita = Slot 2
                dpadSegurado = true;
            }
            else if (Mathf.Abs(dpadY) > 0.5f)
            {
                inventario.AlternarSelecao(1); // Seta Cima / Baixo = Slot 1
                dpadSegurado = true;
            }
        }
    }
}
