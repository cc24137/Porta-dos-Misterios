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

    public void Inicio()
    {
        SceneManager.LoadScene("MenuInicial");
    }

    public void AbrirGithub()
    {
        Application.OpenURL("https://github.com/cc24137/Porta-dos-Misterios");
    }
}
