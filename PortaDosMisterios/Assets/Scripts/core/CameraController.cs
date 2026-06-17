using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 posicaoAlvo;

    void Start()
    {
        posicaoAlvo = transform.position;
    }

    void Update()
    {
        transform.position = posicaoAlvo;
    }

    // metodo que a porta chama
    public void MudarParaSala(Transform novaPosicaoDaSala)
    {
        posicaoAlvo = new Vector3(novaPosicaoDaSala.position.x, novaPosicaoDaSala.position.y, transform.position.z);
    }
}
