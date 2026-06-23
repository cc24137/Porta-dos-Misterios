using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Events;

public class Abilities : MonoBehaviour
{
    [Header("Triggers das habilidades")]
    public UnityEvent eFlash;
    public UnityEvent eLoudSound;
    public UnityEvent ePorta;

    [Header("Objetos das habilidades")]
    public GameObject objetoFlash;
    public GameObject objetoLoudSound;

    [Header("Custos das habilidades")]
    public int custoFlash = 20;
    public int custoLoudSound = 30;

    [Header("Habilitação das habilidades")]
    public bool flashLiberada = true;
    public bool loudSoundLiberado = true;

    [Header("Particulares de habilidades")]
    public float distanciaDeSpawn = 40f;

    [Header("Habilidade: Porta Ilusória")]
    public GameObject prefabPortaIlusoria;
    public int custoPorta = 40;
    public LayerMask mascaraParedes;
    public float distanciaMaxBuscaParede = 5f;
    public float espessuraMaxParede = 10f;

    private movimentacao player;
    private FlashAbility flashObject;
    private LoudSound loudSoundObject;
    private Recursos recursos;

    void Start()
    {
        if (objetoFlash == null || objetoLoudSound == null)
        {
            Debug.LogError("Precisa atribuir a flash e o loud sound");
        }

        if (!objetoFlash.TryGetComponent<FlashAbility>(out flashObject) || !objetoLoudSound.TryGetComponent<LoudSound>(out loudSoundObject))
        {
            Debug.LogError("Os objetos das habilidades precisam ter os scripts correspondentes");
        }

        player = GetComponent<movimentacao>();
        recursos = GetComponent<Recursos>();
    }

    public void TentarUsarFlash()
    {
        if (!flashLiberada)
        {
            Debug.Log("Apertou o botao de flash, mas nao esta liberada");
            return;
        }
        if (!recursos.ConsumirEnergia(custoFlash))
        {
            Debug.Log("Sem energia suficiente");
            // TODO logica para mostrar para o jogador por UI que esta sem energia
        }
        else
        {
            Debug.Log("Lanca flash");
            MakeFlash();
        }
    }

    public void TentarUsarLoudSound()
    {
        if (!loudSoundLiberado)
        {
            Debug.Log("Apertou o botao de loud sound, mas nao esta liberada");
            return;
        }
        if (!recursos.ConsumirEnergia(custoLoudSound))
        {
            Debug.Log("Sem energia suficiente");
            // TODO logica para mostrar para o jogador por UI que esta sem energia
        }
        else
        {
            Debug.Log("Lanca loud sound");
            MakeLoudSound();
        }
    }

    private void MakeFlash()
    {
        Debug.Log("player.direcao: " + player.lastDirection.x + ", " + player.lastDirection.y);
        Vector3 direction = (Vector3) (player.lastDirection * distanciaDeSpawn);
        Vector3 posicaoSpawn = transform.position + direction;
        Instantiate(objetoFlash, posicaoSpawn, Quaternion.identity);
        eFlash?.Invoke();
    }

    private void MakeLoudSound()
    {

    }

    public void TentarUsarPortaIlusoria()
    {
        if (!recursos.ConsumirEnergia(custoPorta))
        {
            return;
        }

        MakePorta();
    }

    private void MakePorta()
    {
        Vector2 origin = transform.position;
        Vector2 dir = player.lastDirection.normalized;

        // encontra a primeira porta
        RaycastHit2D hitEntrada = Physics2D.Raycast(origin, dir, distanciaMaxBuscaParede, mascaraParedes);

        if (hitEntrada.collider != null)
        {
            Vector2 pontoEntrada = hitEntrada.point;
            Vector2 pontoAtual = pontoEntrada + (dir * 0.2f); // Dá o primeiro passo para dentro da parede

            bool achouSaida = false;
            int passos = 0;

            int maxPassos = Mathf.CeilToInt(espessuraMaxParede / 0.2f);

            int estadoTravessia = 0;
            // 0 = Atravessando a borda da Sala 1
            // 1 = Passando pelo Vazio entre as salas
            // 2 = Atravessando a borda da Sala 2

            GameObject primeiraParede = hitEntrada.collider.gameObject;

            while (passos < maxPassos)
            {
                Collider2D col = Physics2D.OverlapPoint(pontoAtual, mascaraParedes);

                if (estadoTravessia == 0)
                {
                    if (col == null)
                    {
                        // Saiu da Parede 1 e não bateu em nada -> vazio entre salas
                        estadoTravessia = 1;
                    }
                    else if (col.gameObject != primeiraParede)
                    {
                        // duas bordas grudadas uma na outra
                        estadoTravessia = 2;
                    }
                }
                else if (estadoTravessia == 1)
                {
                    if (col != null)
                    {
                        // encontrou a parede 2
                        estadoTravessia = 2;
                    }
                }
                else if (estadoTravessia == 2)
                {
                    if (col == null)
                    {
                        // chegou na sala 2
                        achouSaida = true;
                        break;
                    }
                }

                pontoAtual += (dir * 0.2f);
                passos++;
            }

            if (achouSaida)
            {
                float anguloDirecao = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

                Quaternion rotacaoEntrada = Quaternion.Euler(0, 0, anguloDirecao - 90f);

                Quaternion rotacaoSaida = Quaternion.Euler(0, 0, anguloDirecao + 90f);


                float distanciaSegura = 5.0f;

                Vector2 posEntrada = pontoEntrada - (dir * distanciaSegura);
                GameObject portaEntrada = Instantiate(prefabPortaIlusoria, posEntrada, rotacaoEntrada);

                Vector2 posSaida = pontoAtual + (dir * distanciaSegura);
                GameObject portaSaida = Instantiate(prefabPortaIlusoria, posSaida, rotacaoSaida);

                PortaIlusoria scriptEntrada = portaEntrada.GetComponent<PortaIlusoria>();
                PortaIlusoria scriptSaida = portaSaida.GetComponent<PortaIlusoria>();

                scriptEntrada.destino = portaSaida.transform;
                scriptSaida.destino = portaEntrada.transform;

                ePorta?.Invoke(); // faz som
            }
            else
            {
                Debug.Log("A distância até a próxima sala é maior que a 'Espessura Max Parede' ou não existe outra sala nessa direção!");
                recursos.RecuperarEnergia(custoPorta);
            }
        }
        else
        {
            Debug.Log("Nenhuma parede encontrada nessa direção.");
            recursos.RecuperarEnergia(custoPorta);
        }
    }
}
