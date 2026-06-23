using UnityEngine;

public class PlayOnEvent : MonoBehaviour
{

    // se é para o som recomeçar ou se sobrepor a outro já começado
    // oneShot sobrepõe
    public bool oneShot = false;

    public AudioSource audioSource;
    public AudioClip audioClip;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySound()
    {
        Debug.Log("Som chamado");
        Debug.Log("Nome do som: " + audioClip.name);
        if (oneShot)
        {
            audioSource.PlayOneShot(audioClip);
        }
        else
        {
            audioSource.Play();
        }
    }
}
