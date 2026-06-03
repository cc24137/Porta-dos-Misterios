using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Internal;
using TreeEditor;
using UnityEditor.EditorTools;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Rendering;

public class patrol : MonoBehaviour
{
    public float velocidade = 2f;
    public GameObject[] objetosDaRota;
    private List<Vector3> rota = new List<Vector3>();
    private int indRota = 0;
    private bool indo = true;

    // Eventos
    public static event Action<int, int> OnMudouDeDirecao;

    void Start()
    {
        if (objetosDaRota.Length == 0) throw new System.Exception("Rota vazia");
        rota.Add(transform.position);
        foreach(GameObject obj in objetosDaRota)
        {
            var posicao = new Vector3(obj.transform.position.x, obj.transform.position.y, 0);
            rota.Add(posicao);
        }
        Debug.Log("Tamanho da rota: " + rota.Count);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Tamanho do vetor rota: " + rota.Count);
        //Debug.Log("indRota: " + indRota);
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
        int movimentoX, movimentoY;
        if (transform.position.x > rota[indRota].x)
            movimentoX = -1;
        else
            movimentoX = 1;

        if (transform.position.y > rota[indRota].y)
            movimentoY = -1;
        else
            movimentoY = 1;
        

        //Debug.Log("Coordenadas da próxima meta da rota: (" + rota[indRota].x +", " + rota[indRota].y + ")");
        Vector3 direcaoMovimento = new Vector3(
              movimentoX, movimentoY, 0   
            );
        direcaoMovimento = direcaoMovimento.normalized;

        transform.Translate(direcaoMovimento * velocidade * Time.deltaTime);
    }
}
