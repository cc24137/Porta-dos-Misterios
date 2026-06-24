using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

[System.Serializable]
public class FalaFinal
{
    [TextArea(2, 5)]
    public string texto;

    [Tooltip("Tempo (em segundos) que a tela ficará preta ANTES de mostrar este texto")]
    public float tempoAntesDeMostrar = 1f;

    [Tooltip("Coloque aqui o método do som que deve tocar quando esta fala aparecer")]
    public UnityEvent eventoSom;
}

public class FimCena1 : MonoBehaviour
{
    [Header("Configurações do Diálogo")]
    public FalaFinal[] sequenciaDeFalas;

    [Header("Efeito Visual")]
    public Image telaPreta;
    public float tempoParaEscurecer = 2f;

    [Header("Transição de Cena")]
    public string nomeCenaCreditosOMenu = "MenuInicial";

    private bool jaAtivou = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !jaAtivou)
        {
            jaAtivou = true;
            StartCoroutine(RotinaFinal());
        }
    }

    private IEnumerator RotinaFinal()
    {
        if (GerenciadorUI.Instancia != null)
        {
            GerenciadorUI.Instancia.AbrirTexto(new string[] { "" });
            GerenciadorUI.Instancia.painelTexto.SetActive(false);
            GerenciadorUI.Instancia.estaLendo = true;
        }

        if (telaPreta != null)
        {
            telaPreta.gameObject.SetActive(true);
            Color cor = telaPreta.color;
            float tempo = 0;

            while (tempo < tempoParaEscurecer)
            {
                tempo += Time.deltaTime;
                cor.a = Mathf.Clamp01(tempo / tempoParaEscurecer);
                telaPreta.color = cor;

                if (GerenciadorUI.Instancia != null) GerenciadorUI.Instancia.estaLendo = true;

                yield return null;
            }
        }

        if (GerenciadorUI.Instancia != null)
        {
            for (int i = 0; i < sequenciaDeFalas.Length; i++)
            {
                float tempoEspera = sequenciaDeFalas[i].tempoAntesDeMostrar;

                while (tempoEspera > 0)
                {
                    tempoEspera -= Time.deltaTime;
                    GerenciadorUI.Instancia.estaLendo = true;
                    GerenciadorUI.Instancia.painelTexto.SetActive(false);
                    yield return null;
                }

                string[] falaUnica = new string[] { sequenciaDeFalas[i].texto };
                GerenciadorUI.Instancia.AbrirTexto(falaUnica);

                sequenciaDeFalas[i].eventoSom?.Invoke();

                while (GerenciadorUI.Instancia.estaLendo)
                {
                    yield return null;
                }
            }
        }

        Debug.Log("Fim de Jogo");
        SceneManager.LoadScene(nomeCenaCreditosOMenu);
    }
}
