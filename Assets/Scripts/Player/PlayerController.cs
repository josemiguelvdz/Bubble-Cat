using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject bala;
    public GameObject melee;
    public Transform bulletSpawn;

    public float bulletRate = 0.5f;
    public float timeBulletRate = 0;
    public float jumpVelocity = 8f;
    public float fallMultiplier = 2.5f, lowJumpMultiplier = 1.2f;
    public float raycastLength;

    Rigidbody2D rb;
    BubbleSpawner spawner;
    BubbleHelmet bubbleHelmet;
    BubbleSpawner bubbleSpawner;

    [Tooltip("Velocidad del jugador."), SerializeField]
    float speed = 0;
    float horizontal; // Input del eje horizontal
    bool isJumping;

    [Tooltip("Índice de la layer del escenario."), SerializeField]
    int stageLayer = 9;


    void Start()
    {
        GameManager.GetInstance().SetPlayerController(this);

        rb = GetComponent<Rigidbody2D>(); // Componente RigidBody2D del jugador
        spawner = GetComponentInChildren<BubbleSpawner>();  
        bubbleHelmet = GetComponent<BubbleHelmet>();
        bubbleSpawner = GetComponentInChildren<BubbleSpawner>();

        //Bit shift para obtener la bit mask
        stageLayer = 1 << stageLayer;
    }




    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal"); //Recogida de input
        if (horizontal < 0 && transform.localScale.x >= 0) transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        else if (horizontal > 0 && transform.localScale.x<=0) transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

        if (Input.GetButtonDown("Gun")) Shoot();
        if (Input.GetButtonDown("Bubble") && !isJumping)
        {
            rb.velocity = new Vector2(0, 0);
            spawner.BubbleSpawn();
            GameManager.GetInstance().DeactivatePlayerController();
        }
        if (Input.GetButtonDown("Melee")) Hit();

        if (Input.GetButtonDown("Reload")) GameManager.GetInstance().Reload();

        //Salto
        if (Input.GetButtonDown("Jump") && !isJumping) rb.velocity = Vector2.up * jumpVelocity;

        if (Input.GetButtonDown("Helmet")) bubbleHelmet.InvokeReplace();



        if (rb.velocity.y < 0) rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
       
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump")) rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
    }



    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y); // Movimiento físico del jugador

        if (Physics2D.Raycast(transform.position, Vector2.down, raycastLength, stageLayer))
        {
            Debug.Log("He llegado al suelo");
            isJumping = false;
            bubbleSpawner.enabled = true;
        }
        else
        {
            Debug.Log("Estoy en el aire");
            isJumping = true;
            bubbleSpawner.enabled = false;
        }

        Debug.DrawRay(transform.position, Vector2.down * raycastLength, Color.cyan);
    }

    void Shoot()
    {
        if (GameManager.GetInstance().CanShoot() && Time.time > timeBulletRate)
        {

            if (transform.localScale.x < 0)
            {
                Quaternion inverse = Quaternion.Inverse(bulletSpawn.rotation);
                //CREA UNA COPIA DE LA BALA
                Instantiate(bala, bulletSpawn.position, inverse);
            }
            else if (transform.localScale.x > 0)
            {
                //CREA UNA COPIA DE LA BALA
                Instantiate(bala, bulletSpawn.position, bulletSpawn.rotation);
            }

            //ACTUALIZA LA VARIABLE QUE LIMITA LAS BALAS
            timeBulletRate = Time.time + bulletRate;

            GameManager.GetInstance().Shoot();
        }
    }


    void Hit()
    {
        melee.SetActive(true);

        Invoke("desactiveMelee", 0.2f);
    }
    void desactiveMelee()
    {
        if (melee.activeSelf) melee.SetActive(false);
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Debug.Log("Soy player");
        }
        else
            Debug.Log("No soy player");
    }
}
