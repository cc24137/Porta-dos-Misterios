using UnityEngine;

public class Livro : MonoBehaviour, IInteragivel {

    [TextArea(3, 10)]
    public string textoDoLivro;

    public void Interagir()
    {
        // implementar painel de diálogo
        Debug.Log("Texto do Livro: " + textoDoLivro);
    }
	
}
