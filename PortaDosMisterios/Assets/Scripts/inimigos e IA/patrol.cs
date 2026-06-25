using System;
using System.Collections; // Necessário para Coroutines
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Necessário para reiniciar a cena
using UnityEngine.UI; // Necessário para manipular a tela preta

public class patrol : MonoBehaviour
{
    public enum DirecaoOlhar { Cima, Baixo, Esquerda, Direita }

    [Header("Configurações de Movimento")]
    public float velocidade = 2f;
    public GameObject[] objetosDaRota;
    public float tempoDeEspera = 1f;

    [Header("Configuração de Sentinela (Estacionário)")]
    [Tooltip("Direção que o inimigo ficará olhando caso não tenha pontos de patrulha")]
    public DirecaoOlhar direcaoInicialOlhar = DirecaoOlhar.Baixo;

    [Header("Configurações de Game Over")]
    public Image telaPreta; // Arraste a imagem preta da UI aqui
    public float tempoTelaPreta = 1f;
    public float tempoEscurecendo = 1f;

    [Header("Referências")]
    public Animator animator;

    [Header("Feedback Visual")]
    public SpriteRenderer particulaCabeca;
    public Sprite imagemAtencao;
    public Sprite imagemStun;

    private VisionCone coneDeVisao;

    private List<Vector3> rota = new List<Vector3>();
    private int indRota = 0;
    private bool indo = true;

    private float cronometro = 0f;
    private bool estaEsperando = false;

    public bool detectouPlayer = false;
    private bool jaIniciouGameOver = false;

    private enum estado
    {
        normal,
        buscando
    }
    //private estado situacao = estado.normal;
    private bool wasBlind = false;

    //public static event Action<int, int> OnMudouDeDirecao;

    void Start()
    {
        coneDeVisao = GetComponent<VisionCone>();

        if (particulaCabeca != null)
        {
            particulaCabeca.gameObject.SetActive(false);
        }

        Vector2 dirInicial = GetVectorDaDirecao(direcaoInicialOlhar);
        SettarDirecaoOlhar(dirInicial);

        if (objetosDaRota == null || objetosDaRota.Length == 0)
        {
            return;
        }

        rota.Add(transform.position);

        foreach(GameObject obj in objetosDaRota)
        {
            var posicao = new Vector3(obj.transform.position.x, obj.transform.position.y, transform.position.z);
            rota.Add(posicao);
        }
    }

    void Update()
    {        if (jaIniciouGameOver) return;

        if (!coneDeVisao.isSeeing)
        {
            wasBlind = true;
            OlharParaBaixoParado();
            AtivarParticula(imagemStun);
            return;
        }

        if (wasBlind)
        {
            wasBlind = false;
            DesativarParticula();
            EntraEmEstadoDeAlerta();
        }

        if (detectouPlayer)
        {
            OlharParaBaixoParado();
            AtivarParticula(imagemAtencao);

            if (!jaIniciouGameOver)
            {
                jaIniciouGameOver = true;
                StartCoroutine(RotinaAvistouPlayer());
            }
            return;
        }

        DesativarParticula();

        if (objetosDaRota == null || objetosDaRota.Length == 0)
        {
            Vector2 dirInicial = GetVectorDaDirecao(direcaoInicialOlhar);
            SettarDirecaoOlhar(dirInicial);
            return;
        }

        if (estaEsperando)
        {
            cronometro -= Time.deltaTime;

            if (cronometro <= 0f)
            {
                estaEsperando = false;
            }
            else
            {
                AtualizarAnimacoes(Vector2.zero);
                return;
            }
        }

        if (Vector2.Distance(transform.position, rota[indRota]) <= 0.05f)
        {
            estaEsperando = true;
            cronometro = tempoDeEspera;

            if (indRota == rota.Count - 1)
            {
                indo = false;
                indRota--;
            }
            else if (indRota == 0)
            {
                indo = true;
                indRota++;
            }
            else
            {
                if (indo) indRota++;
                else indRota--;
            }

            AtualizarAnimacoes(Vector2.zero);
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, rota[indRota], velocidade * Time.deltaTime);

        Vector3 direcaoMovimento = (rota[indRota] - transform.position).normalized;
        AtualizarAnimacoes(direcaoMovimento);

        if (coneDeVisao != null && direcaoMovimento != Vector3.zero)
        {
            coneDeVisao.SetDirection(direcaoMovimento);
        }
    }

    private IEnumerator RotinaAvistouPlayer()
    {
        yield return new WaitForSeconds(tempoTelaPreta);

        if (telaPreta != null)
        {
            Color corDaTela = telaPreta.color;
            float tempoPassado = 0f;

            while (tempoPassado < tempoEscurecendo)
            {
                tempoPassado += Time.deltaTime;
                corDaTela.a = Mathf.Clamp01(tempoPassado / tempoEscurecendo);
                telaPreta.color = corDaTela;
                yield return null;
            }
        }
        else
        {
            yield return new WaitForSeconds(tempoEscurecendo);
        }

        Scene cenaAtual = SceneManager.GetActiveScene();
        SceneManager.LoadScene(cenaAtual.name);
    }

    private void OlharParaBaixoParado()
    {
        animator.SetBool("Walking", false);
        animator.SetFloat("Horizontal", 0f);
        animator.SetFloat("Vertical", -1f);
        animator.SetFloat("LastHorizontal", 0f);
        animator.SetFloat("LastVertical", -1f);
    }

    private void SettarDirecaoOlhar(Vector2 dir)
    {
        animator.SetBool("Walking", false);
        animator.SetFloat("Horizontal", dir.x);
        animator.SetFloat("Vertical", dir.y);
        animator.SetFloat("LastHorizontal", dir.x);
        animator.SetFloat("LastVertical", dir.y);

        if (coneDeVisao != null)
        {
            coneDeVisao.SetDirection(dir);
        }
    }

    private Vector2 GetVectorDaDirecao(DirecaoOlhar dir)
    {
        switch (dir)
        {
            case DirecaoOlhar.Cima:     return Vector2.up;
            case DirecaoOlhar.Baixo:    return Vector2.down;
            case DirecaoOlhar.Esquerda: return Vector2.left;
            case DirecaoOlhar.Direita:  return Vector2.right;
            default:                    return Vector2.down;
        }
    }

    private void AtivarParticula(Sprite novaImagem)
    {
        if (particulaCabeca != null && novaImagem != null)
        {
            particulaCabeca.sprite = novaImagem;
            particulaCabeca.gameObject.SetActive(true);
        }
    }

    private void DesativarParticula()
    {
        if (particulaCabeca != null && particulaCabeca.gameObject.activeSelf)
        {
            particulaCabeca.gameObject.SetActive(false);
        }
    }

    void EntraEmEstadoDeAlerta()
    {
        cronometro = 0f;
        tempoDeEspera = tempoDeEspera / 2;
        velocidade *= 2;
    }

    void AtualizarAnimacoes(Vector2 dir)
    {
        if (dir.sqrMagnitude > 0.01f)
        {
            animator.SetBool("Walking", true);
            animator.SetFloat("Horizontal", dir.x);
            animator.SetFloat("Vertical", dir.y);
            animator.SetFloat("LastHorizontal", dir.x);
            animator.SetFloat("LastVertical", dir.y);
        }
        else
        {
            animator.SetBool("Walking", false);
        }
    }
}
