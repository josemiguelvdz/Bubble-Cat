using UnityEngine;

public class Bomb : MonoBehaviour
{
    [Tooltip("Prefab de la bomba de butano a instanciar en el Spawn Point"), SerializeField]
    GameObject bomb;

    [Tooltip("Aquí se instancia la bomba de butano"), SerializeField]
    Transform spawnPoint;

    [Tooltip("Dirección que toma la bomba de butano"), SerializeField]
    Vector2 direction;

    [Tooltip("Tiempo que tarda Bastet en cargar la bomba de butano y disparar"), SerializeField]
    float chargeTime;

    GameObject bombInstance = null;

    private void OnEnable()
    {
        Invoke("Fire", chargeTime);
    }

    private void Update()
    {
        if (bombInstance != null) ;
    }

    void Fire()
    {
        bombInstance = Instantiate(bomb, spawnPoint);
        bombInstance.GetComponent<Rigidbody2D>().AddForce(direction, ForceMode2D.Impulse);
    }
}
