using System;
using System.Collections.Generic;
using UnityEngine;

public class patrol : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    public float velocidade = 2f;
    public GameObject[] objetosDaRota;

    [Header("Referências")]
    public Animator animator;


    private VisionCone coneDeVisao;

    private List<Vector3> rota = new List<Vector3>();
    private int indRota = 0;
    private bool indo = true;

    // Eventos
    public static event Action<int, int> OnMudouDeDirecao;

    void Start()
    {
        if (objetosDaRota.Length == 0) throw new System.Exception("Rota vazia");

        coneDeVisao = GetComponent<VisionCone>();

        rota.Add(transform.position);

        foreach(GameObject obj in objetosDaRota)
        {
            var posicao = new Vector3(obj.transform.position.x, obj.transform.position.y, 0);
            rota.Add(posicao);
        }

        Debug.Log("Tamanho da rota: " + rota.Count);
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, rota[indRota]) < 0.1f)
        {
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
        }

        Vector3 direcaoMovimento = (rota[indRota] - transform.position).normalized;

        transform.Translate(direcaoMovimento * velocidade * Time.deltaTime);

        AtualizarAnimacoes(direcaoMovimento);

        // passa a direção de movimento para o cone de visão
        if (coneDeVisao != null && direcaoMovimento != Vector3.zero)
        {
            coneDeVisao.SetDirection(direcaoMovimento);
        }
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
