using UnityEngine;

public class BubbleSpawner : MonoBehaviour
{
    //Objeto publico donde va el prefab de la pompa
    public GameObject bubble;
    public AudioClip bubbleCreation;
    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void BubbleSpawn()
    {
        //Instanciamos la pompa en el SpawnPoint
        Instantiate(bubble, transform.position, bubble.transform.rotation);
        audioSource.PlayOneShot(bubbleCreation);
    }
}
