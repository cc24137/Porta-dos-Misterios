using System;
using System.Collections.Generic;
using UnityEngine;

public class patrol : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    public float velocidade = 2f;
    public GameObject[] objetosDaRota;
    public float tempoDeEspera = 1f;

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

    private enum estado
    {
        normal, // patrulha normal
        buscando // depois de ouvir barulho, vai ate a fonte do barulho. Nao implementado ainda
    }
    private estado situacao = estado.normal;
    private bool wasBlind = false;

    // Eventos
    public static event Action<int, int> OnMudouDeDirecao;

    void Start()
    {
        if (objetosDaRota.Length == 0) throw new System.Exception("Rota vazia");

        coneDeVisao = GetComponent<VisionCone>();

        if (particulaCabeca != null)
        {
            particulaCabeca.gameObject.SetActive(false);
        }

        rota.Add(transform.position);

        foreach(GameObject obj in objetosDaRota)
        {
            var posicao = new Vector3(obj.transform.position.x, obj.transform.position.y, 0);
            rota.Add(posicao);
        }
    }

    void Update()
    {
        // stun
        if (!coneDeVisao.isSeeing)
        {
            wasBlind = true;
            OlharParaBaixoParado();
            AtivarParticula(imagemStun);
            return;
        }

        // stun acabou
        if (wasBlind)
        {
            wasBlind = false;
            DesativarParticula(); // Apaga as estrelinhas
            EntraEmEstadoDeAlerta(); // Acorda mais rápido
        }

        if (detectouPlayer)
        {
            OlharParaBaixoParado();
            AtivarParticula(imagemAtencao); // Liga a exclamação
            return; // Fica parado olhando
        }

        DesativarParticula();

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

        if (Vector3.Distance(transform.position, rota[indRota]) < 0.1f)
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

        Vector3 direcaoMovimento = (rota[indRota] - transform.position).normalized;
        transform.Translate(direcaoMovimento * velocidade * Time.deltaTime);
        AtualizarAnimacoes(direcaoMovimento);

        if (coneDeVisao != null && direcaoMovimento != Vector3.zero)
        {
            coneDeVisao.SetDirection(direcaoMovimento);
        }
    }

    private void OlharParaBaixoParado()
    {
        animator.SetBool("Walking", false);
        animator.SetFloat("Horizontal", 0f);
        animator.SetFloat("Vertical", -1f);
        animator.SetFloat("LastHorizontal", 0f);
        animator.SetFloat("LastVertical", -1f);
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
