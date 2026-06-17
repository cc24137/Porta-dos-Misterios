using UnityEngine;
using TMPro;

public class GerenciadorUI : MonoBehaviour
{
    public static GerenciadorUI Instancia;

    public GameObject painelLeitura;
    public TextMeshProUGUI textoLeitura;
    public bool estaLendo = false;

    void Awake()
    {
        Instancia = this;
    }

    public void MostrarTexto(string conteudo)
    {
        textoLeitura.text = conteudo;
        painelLeitura.SetActive(true);
        estaLendo = true;
    }

    public void FecharTexto()
    {
        painelLeitura.SetActive(false);
        estaLendo = false;
    }
}
