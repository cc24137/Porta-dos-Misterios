using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    [Header("Configurações")]
    public string nomeDaCenaDoJogo = "Scene1";
    public string nomeDaCenaInicio = "MenuInicial";

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
        Debug.Log("Voltando para o menu inicial...");
        SceneManager.LoadScene(nomeDaCenaInicio);
    }

    public void AbrirGithub()
    {
        Application.OpenURL("https://github.com/cc24137/Porta-dos-Misterios");
    }
}
