using UnityEngine;

public class Shoot : MonoBehaviour
{
    [Tooltip("Prefab del proyectil a instanciar en el Spawn Point"), SerializeField]
    GameObject proyectile;

    [Tooltip("Aquí se instancian los proyectiles"), SerializeField]
    Transform spawnPoint;

    [Tooltip("Munición que tiene el ataque"), SerializeField]
    int ammo;

    [Tooltip("Fuerza del impulso que toman los proyectiles al ser instanciados"), SerializeField]
    Vector2 impulse;

    [Tooltip("Tiempo que tarda Bastet en cargar los proyectiles y empezar a disparar"), SerializeField]
    float chargeTime;

    [Tooltip("Tiempo entre proyectiles"), SerializeField]
    float shootRate;

    [Tooltip("Probabilidad de que el siguiente ataque sean los puños"), Range(0, 100f), SerializeField]
    float prob = 40f;

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
        GameObject bag = Instantiate(proyectile, spawnPoint.position, Quaternion.identity);
        bag.GetComponent<Rigidbody2D>().AddForce(impulse * Random.Range(0.5f, 2f), ForceMode2D.Impulse);
        proyectilesLeft--;

        if (proyectilesLeft == 0)
        {
            if(Random.Range(0, 101) < prob)
                bastet.DesiredState(Bastet.States.fists);
            else
                bastet.DesiredState(Bastet.States.bomb);
        }
    }
}
