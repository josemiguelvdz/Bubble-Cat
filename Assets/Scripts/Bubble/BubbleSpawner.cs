using UnityEngine;

public class BubbleSpawner : MonoBehaviour
{
    //Objeto publico donde va el prefab de la pompa
    public GameObject bubble;
    public void BubbleSpawn()
    {
        //Instanciamos la pompa en el SpawnPoint
        Instantiate(bubble, transform.position, bubble.transform.rotation);
    }
}
