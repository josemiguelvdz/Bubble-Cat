using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFists : MonoBehaviour
{
    public AudioClip fistSound;
    public AudioSource audioSource;
    
    public void PlaySound()
    {
        audioSource.PlayOneShot(fistSound);
    }
}
