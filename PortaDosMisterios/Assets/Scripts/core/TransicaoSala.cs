using UnityEngine;
using UnityEngine.Events;

public class TransicaoSala : MonoBehaviour
{
    [Header("Destinos")]
    [Tooltip("O ponto onde o player vai aparecer na próxima sala.")]
    public Transform pontoDeChegada;

    [Tooltip("O ponto central da próxima sala para onde a câmera deve ir.")]
    public Transform posicaoNovaCamera;

    private CameraController cameraController;

    public UnityEvent eventoSom;

    void Start()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            eventoSom?.Invoke();
            if (pontoDeChegada != null)
            {
                collision.transform.position = pontoDeChegada.position;
            }
            else
            {
                Debug.LogWarning("O Ponto de Chegada não foi configurado na porta!");
            }

            if (posicaoNovaCamera != null && cameraController != null)
            {
                cameraController.MudarParaSala(posicaoNovaCamera);
            }
            else
            {
                Debug.LogWarning("A Posição da Nova Câmera não foi configurada na porta!");
            }
        }
    }
}
