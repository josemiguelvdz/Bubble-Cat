﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
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
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>(); 
    }

    public void MusicScene()
    {
        Scene escena = SceneManager.GetActiveScene();

        Debug.Log("Estoy en " + escena.name);

        switch (escena.name)
        {
            case "Sewer":   
                audioSource.PlayOneShot(sewer);
                break;

            case "Beach":
                audioSource.PlayOneShot(bastet);
                break;
        }


    }

    public static MusicManager GetInstance()
    {
        return instance;
    }
}