using UnityEngine;

public class TrashAttack : MonoBehaviour
{
    [Tooltip("Pilares"), SerializeField]
    private GameObject Pillars;

    private Vector3 initialPosition;
    private Rigidbody2D rb;
    private Bastet bastet;

    private bool afterSpawn = false;

    [Tooltip("Sonido de los pilares"), SerializeField]
    private AudioClip pilarSound;
    private AudioSource audioSource;

    void Start()
    {
        rb = Pillars.transform.GetChild(0).GetComponent<Rigidbody2D>();

        initialPosition = Pillars.transform.position; // Para que al terminar, vuelvan a estar en su misma posición, por si hay que
        // repetir el ataque

        bastet = GetComponent<Bastet>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    { 
        Invoke("TrashSpawn", 1f);
    }

    private void Update()
    {
        if (rb.velocity == Vector2.zero && afterSpawn) // Cuando los pilares han parado
        {
            Invoke("TrashDespawn", 2f);
        }
    }
    public void TrashSpawn()
    {

        Pillars.transform.position = initialPosition;

        if (Random.Range(0, 2) == 1) // Los pilares no aparecen siempre en el mismo sitio, a veces aparecen en distinta localización
        {
            Pillars.transform.position = Pillars.transform.position + new Vector3(2, 0, 0);
        }

        Pillars.SetActive(true); // Aparecen los pilares
        audioSource.PlayOneShot(pilarSound);

        afterSpawn = true;
        Invoke("NextAttack", 3f);
    }
    public void TrashDespawn() // Desaparecen los pilares
    {
        Pillars.SetActive(false);
        afterSpawn = false;
    }

    void NextAttack() // El siguiente ataque de Bastet siempre será el de la bomba
    {
        bastet.DesiredState(Bastet.States.bomb);
    }

    private void OnDisable()
    {
        CancelInvoke();
        TrashDespawn();
    }
}
