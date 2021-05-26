using UnityEngine;

public class TrashAttack : MonoBehaviour
{
    public GameObject Pillars;

    Vector3 initialPosition;
    Rigidbody2D rb;
    Bastet bastet;

    private bool afterSpawn = false;

    public AudioClip pilarSound;
    AudioSource audioSource;

    void Start()
    {
        rb = Pillars.transform.GetChild(0).GetComponent<Rigidbody2D>();

        initialPosition = Pillars.transform.position;

        // ANIMACIÓN 
        bastet = GetComponent<Bastet>();

        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    { 
        Invoke("TrashSpawn", 1f);
    }

    private void Update()
    {
        if (rb.velocity == Vector2.zero && afterSpawn)
        {
            Invoke("TrashDespawn", 2f);
        }
    }
    public void TrashSpawn()
    {

        Pillars.transform.position = initialPosition;

        if (Random.Range(0, 2) == 1){
            Pillars.transform.position = Pillars.transform.position + new Vector3(2, 0, 0);
        }

        Pillars.SetActive(true);
        audioSource.PlayOneShot(pilarSound);

        afterSpawn = true;
        Invoke("NextAttack", 3f);
    }
    public void TrashDespawn()
    {
        Pillars.SetActive(false);
        afterSpawn = false;
    }

    void NextAttack()
    {
        bastet.DesiredState(Bastet.States.bomb);
    }

    private void OnDisable()
    {
        CancelInvoke();
        TrashDespawn();
    }
}
