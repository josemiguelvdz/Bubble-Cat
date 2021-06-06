using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    //Clips de musica de para sewer y para la batalla contra bastet
    public AudioClip sewer, bastet;
    AudioSource audioSource;

    static MusicManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        else
            Destroy(this.gameObject);

        //En el awake cogemos el componente de audio para que no de error entre escenas
        audioSource = GetComponent<AudioSource>();
    }

    public void MusicScene()
    {
        Scene escena = SceneManager.GetActiveScene();

        //Dependiendo de la escena
        switch (escena.name)
        {
            //Si estamos en sewer
            case "Sewer":
                audioSource.PlayOneShot(sewer);
                break;
        }
    }

    public void StartBastetMusic()
    {
        //La iniciamos desde el script del propio bastet
        audioSource.PlayOneShot(bastet);
    }

    public static MusicManager GetInstance()
    {
        return instance;
    }
}
