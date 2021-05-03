using UnityEngine;

public class Bomb : MonoBehaviour
{
    [Tooltip("Prefab de la bomba de butano a instanciar en el Spawn Point"), SerializeField]
    GameObject bomb;

    [Tooltip("Aquí se instancia la bomba de butano"), SerializeField]
    Transform spawnPoint;

    [Tooltip("Dirección que toma la bomba de butano"), SerializeField]
    Vector3 direction;

    [Tooltip("Tiempo que tarda Bastet en cargar la bomba de butano y disparar"), SerializeField]
    float chargeTime;

    private void OnEnable()
    {
        Invoke("Fire", chargeTime);
    }

    void Fire()
    {
        Instantiate(bomb, spawnPoint);
    }
}
