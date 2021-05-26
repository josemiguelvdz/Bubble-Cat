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

    [Tooltip("Cañon 1"), SerializeField]
    Animator cannon1Anim = null;
    [Tooltip("Cañon 2"), SerializeField]
    Animator cannon2Anim = null;
    [Tooltip("Aluminio protector"), SerializeField]
    Animator alumAnim = null;

    int proyectilesLeft;
    Transform player;
    Bastet bastet;

    public AudioClip shootSound;
    AudioSource audioSource;

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
        audioSource = GetComponent<AudioSource>();
    }

    void OnDisable()
    {
        CancelInvoke();
    }

    void Burst()
    {
        GameObject bag = Instantiate(proyectile, spawnPoint.position, Quaternion.identity);
        bag.GetComponent<Rigidbody2D>().AddForce(impulse * Random.Range(0.5f, 2f), ForceMode2D.Impulse);

        cannon1Anim.SetBool("isShooting", true);
        cannon2Anim.SetBool("isShooting", true);
        alumAnim.SetBool("isShooting", true);



        proyectilesLeft--;
        audioSource.PlayOneShot(shootSound);

        if (proyectilesLeft == 0)
        {
            cannon1Anim.SetBool("isShooting", false);
            cannon2Anim.SetBool("isShooting", false);
            alumAnim.SetBool("isShooting", false);

            if (Random.Range(0, 101) < prob)
                bastet.DesiredState(Bastet.States.fists);
            else
                bastet.DesiredState(Bastet.States.bomb);
        }
    }
}
