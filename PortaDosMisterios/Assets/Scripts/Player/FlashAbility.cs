using UnityEngine;

public class FlashAbility : MonoBehaviour
{
    public float tempoDeFlashAtiva = 0.5f;

    public void AcabouAnimacao()
    {
        Destroy(gameObject, tempoDeFlashAtiva);
        int newLayer = LayerMask.NameToLayer("FlashObject");
        gameObject.layer = newLayer;
    }
}
