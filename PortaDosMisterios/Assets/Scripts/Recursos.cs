using System;
using UnityEngine;

public class Recursos : MonoBehaviour
{
    [Header("Energia Espiritual")]
    public float energiaEspiritualMaxima = 100f;
    public float energiaEspiritualAtual;

    [Header("Regeneração")]
    public float regeneracaoPorSegundo = 2f;

    public event Action<float, float> OnEnergiaAlterada;

    void Start()
    {
        // teste so dps muda pra descomentar
        //energiaEspiritualAtual = energiaEspiritualMaxima;
        AtualizarUI();
    }

    void Update()
    {
        if (energiaEspiritualAtual < energiaEspiritualMaxima)
        {
            energiaEspiritualAtual += regeneracaoPorSegundo * Time.deltaTime;
            //Debug.Log("Regenerando energia espiritual: " + energiaEspiritualAtual);

            if (energiaEspiritualAtual > energiaEspiritualMaxima)
            {
                energiaEspiritualAtual = energiaEspiritualMaxima;
            }

            AtualizarUI();
        }
    }

    public bool ConsumirEnergia(float custo)
    {
        if (energiaEspiritualAtual >= custo)
        {
            energiaEspiritualAtual -= custo;
            AtualizarUI();
            return true;
        }

        Debug.Log("Espiritualidade insuficiente!");
        return false;
    }

    public void RecuperarEnergia(float quantidade)
    {
        energiaEspiritualAtual += quantidade;

        if (energiaEspiritualAtual > energiaEspiritualMaxima)
        {
            energiaEspiritualAtual = energiaEspiritualMaxima;
        }

        AtualizarUI();
    }

    private void AtualizarUI()
    {
        OnEnergiaAlterada?.Invoke(energiaEspiritualAtual, energiaEspiritualMaxima);
    }
}
