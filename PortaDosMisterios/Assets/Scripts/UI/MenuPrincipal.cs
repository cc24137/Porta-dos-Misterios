using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    [Header("Configurações")]
    public string nomeDaCenaDoJogo = "Scene1";

    public void Jogar()
    {
        SceneManager.LoadScene(nomeDaCenaDoJogo);
    }

    public void SairDoJogo()
    {
        Application.Quit();
    }
}
