using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIEnergia : MonoBehaviour
{
    [Header("Referências")]
    public Recursos recursosDoPlayer;
    public Image imagemDaBarra;
    public TextMeshProUGUI textoDaEnergia;

    [Header("Sprites da Barra")]
    [Tooltip("Coloque na ordem: do VAZIO (posição 0) até o CHEIO (última posição)")]
    public Sprite[] spritesEnergia;

    void Start()
    {
        if (recursosDoPlayer != null)
        {
            recursosDoPlayer.OnEnergiaAlterada += AtualizarBarra;
            AtualizarBarra(recursosDoPlayer.energiaEspiritualAtual, recursosDoPlayer.energiaEspiritualMaxima);
        }
        else
        {
            Debug.LogWarning("Faltou referenciar o Player no UIEnergia!");
        }
    }

    void OnDestroy()
    {
        if (recursosDoPlayer != null)
        {
            recursosDoPlayer.OnEnergiaAlterada -= AtualizarBarra;
        }
    }

    private void AtualizarBarra(float atual, float maxima)
    {
        if (spritesEnergia.Length == 0) return;

        float porcentagem = atual / maxima;
        int index = Mathf.RoundToInt(porcentagem * (spritesEnergia.Length - 1));
        index = Mathf.Clamp(index, 0, spritesEnergia.Length - 1);
        imagemDaBarra.sprite = spritesEnergia[index];

        if (textoDaEnergia != null)
        {
            textoDaEnergia.text = Mathf.FloorToInt(atual).ToString() + " / " + maxima.ToString();
        }
    }
}
