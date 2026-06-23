using UnityEngine;

public class PortaIlusoria : MonoBehaviour, IInteragivel
{
    [Header("Configurações")]
    public Transform destino; // A outra porta para onde o player vai
    public float tempoDeVida = 10f; // Tempo até a porta sumir sozinha

    void Start()
    {
        Destroy(gameObject, tempoDeVida);
    }

    public void Interagir()
    {
        if (destino != null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                player.transform.position = destino.position;
                Debug.Log("Player atravessou a parede!");
            }
        }
        else
        {
            Debug.LogWarning("Esta porta não tem um destino conectado!");
        }
    }
}
