

using System.Security.Cryptography;
using UnityEngine;


// First two abilities to be implemented:
/*

Flash: This Spell is not usually used not to illuminate the environment but to affect others’ vision, 
with a Flash intensity several times more dazzling than old-fashioned flash cameras.

Loud Noise: This creates a massive sound out of nowhere, 
which can be used to fake explosions or disrupt enemies’ attention and hearing.

*/
public class Abilities : MonoBehaviour
{
    [Header("Objetos das habilidades")]
    public GameObject objetoFlash;
    public GameObject objetoLoudSound;

    [Header("Custos das habilidades")]
    public int custoFlash = 20;
    public int custoLoudSound = 30;

    [Header("Habilitação das habilidades")]
    public bool flashLiberada = true;
    public bool loudSoundLiberado = true;

    [Header("Particulares de habildiades")]
    public float distanciaDeSpawn = 40f;

    private movimentacao player;
    private FlashAbility flashObject;
    private LoudSound loudSoundObject;
    private Recursos recursos;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
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

        if (Input.GetKeyDown(KeyCode.L))
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
    }

    private void MakeFlash()
    {
        Debug.Log("player.direcao: " + player.lastDirection.x + ", " + player.lastDirection.y);
        Vector3 direction = (Vector3) (player.lastDirection * distanciaDeSpawn);
        Vector3 posicaoSpawn = transform.position + direction;
        Instantiate(objetoFlash, posicaoSpawn, Quaternion.identity);
    }

    private void MakeLoudSound()
    {
        
    }
}
