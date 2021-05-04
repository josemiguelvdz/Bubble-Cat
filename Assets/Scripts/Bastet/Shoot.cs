using UnityEngine;

public class Shoot : MonoBehaviour
{
    [Tooltip("Prefab del proyectil a instanciar en el Spawn Point"), SerializeField]
    GameObject proyectile;

    [Tooltip("Aquí se instancian los proyectiles"), SerializeField]
    Transform spawnPoint;

    [Tooltip("Munición que tiene el ataque"), SerializeField]
    int ammo;

    [Tooltip("Desvío hacia arriba al apuntar al jugador para contraarrestar la gravedad"), SerializeField]
    float verticalOffset;

    [Tooltip("Tiempo que tarda Bastet en cargar los proyectiles y empezar a disparar"), SerializeField]
    float chargeTime;

    [Tooltip("Tiempo entre proyectiles"), SerializeField]
    float shootRate;

    int proyectilesLeft;
    Transform player;
    Bastet bastet;

    private void Awake()
    {
        bastet = GetComponent<Bastet>();
    }

    private void OnEnable()
    {
        proyectilesLeft = ammo;

        InvokeRepeating("Burst", chargeTime, shootRate);
    }

    private void Start()
    {
        player = GameManager.GetInstance().GetPlayer().transform;
    }

    void OnDisable()
    {
        CancelInvoke();
    }

    void Burst()
    {
        Instantiate(proyectile, spawnPoint);
        proyectilesLeft--;

        if (proyectilesLeft == 0)
            bastet.DesiredState(Bastet.States.fists);
    }
}
