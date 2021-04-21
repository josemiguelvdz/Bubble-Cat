using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject bala;
    public GameObject melee;
    public Transform spawn_bala;

    public float rateBala = 0.5f;
    public float timeRateBala = 0;
    public float jumpVelocity = 8f;
    public float fallMultiplier = 2.5f, lowJumpMultiplier = 1.2f;

    Rigidbody2D rb;
    BubbleSpawner spawner;
    BubbleHelmet bubbleHelmet;
    BubbleSpawner bubbleSpawner;

    [Tooltip("Velocidad del jugador"), SerializeField]
    float speed = 0;
    float horizontal; // Input del eje horizontal
    bool isJumping;





    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Componente RigidBody2D del jugador
        spawner = GetComponentInChildren<BubbleSpawner>();  
        bubbleHelmet = GetComponent<BubbleHelmet>();
        bubbleSpawner = GetComponentInChildren<BubbleSpawner>();

        GameManager.GetInstance().SetPlayerController(this);
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
        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            rb.velocity = Vector2.up * jumpVelocity;
            bubbleSpawner.enabled = false;
            isJumping = true;
        }

        if (Input.GetButtonDown("Helmet")) bubbleHelmet.InvokeReplace();



        if (rb.velocity.y < 0) rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
       
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump")) rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
    }



    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y); // Movimiento físico del jugador
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (Physics2D.Raycast(transform.position,Vector2.down,0.05f))
        {
            isJumping = false;
            bubbleSpawner.enabled = true;
        }
    }



    void Shoot()
    {
        
        if (GameManager.GetInstance().CanShoot() && Time.time > timeRateBala)
        {

            if (transform.localScale.x < 0)
            {
                Quaternion inverse = Quaternion.Inverse(spawn_bala.rotation);
                //CREA UNA COPIA DE LA BALA
                Instantiate(bala, spawn_bala.position, inverse);
            }
            else if (transform.localScale.x > 0)
            {
                //CREA UNA COPIA DE LA BALA
                Instantiate(bala, spawn_bala.position, spawn_bala.rotation);
            }

            //ACTUALIZA LA VARIABLE QUE LIMITA LAS BALAS
            timeRateBala = Time.time + rateBala;

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
