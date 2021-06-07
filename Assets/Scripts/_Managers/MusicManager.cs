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

    public static MusicManager GetInstance()
    {
        return instance;
    }
}
