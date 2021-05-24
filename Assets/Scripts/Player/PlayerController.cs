using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject bala;
    public GameObject melee;
    public Transform bulletSpawn;
    public Transform allowAttack;

    public float meleeCooldown = 1.5f;
    private float currentMeleeCd;

    public float bulletRate = 0.5f;
    public float timeBulletRate = 0;
    public float jumpVelocity = 8f;
    public float jumpHeigth = 2f;
    public float fallMultiplier = 2.5f, lowJumpMultiplier = 1.2f;
    public float raycastLength;

    float jumpStart = 10000f; //Punto de inicio del salto
    Rigidbody2D rb;
    BubbleSpawner spawner;
    BubbleHelmet bubbleHelmet;
    BubbleSpawner bubbleSpawner;

    [Tooltip("Velocidad del jugador."), SerializeField]
    float speed = 0;
    float horizontal; // Input del eje horizontal
    bool isJumping;

    [Tooltip("Layer del escenario."), SerializeField]
    LayerMask stage;

    private bool key = false;
    private Animator animator;

    public AudioClip jumpSound, shootSound;
    AudioSource audioSource;

    private void Start()
    {
        currentMeleeCd = meleeCooldown;
        audioSource = GetComponent<AudioSource>();
    }
    void OnEnable()
    {
        GameManager.GetInstance().SetPlayerController(this);

        rb = GetComponent<Rigidbody2D>(); // Componente RigidBody2D del jugador
        spawner = GetComponentInChildren<BubbleSpawner>();  
        bubbleHelmet = GetComponent<BubbleHelmet>();
        bubbleSpawner = GetComponentInChildren<BubbleSpawner>();
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        
        currentMeleeCd -=  Time.deltaTime;
        //Debug.Log(meleeCooldown);

        horizontal = Input.GetAxisRaw("Horizontal"); //Recogida de input
        if (horizontal < 0 && transform.localScale.x >= 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
            
        else if (horizontal > 0 && transform.localScale.x<=0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }

        if (Mathf.Round(rb.velocity.magnitude) == 0)
        {
            animator.SetBool("isRunning", false);
        }
        else
            animator.SetBool("isRunning", true);


        Vector2 dir = allowAttack.position - transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, dir.magnitude, stage);
        if (!hit || !hit.collider.gameObject.GetComponent<CompositeCollider2D>())
        {
            if (Input.GetButtonDown("Gun"))
            {
                Shoot();
            }

            if (Input.GetButtonDown("Bubble") && !isJumping)
            {
                animator.SetBool("isRunning", false);
                rb.velocity = new Vector2(0, 0);
                spawner.BubbleSpawn();
                animator.SetBool("createBubble", true);
                Invoke("StopCreatingBubbleAnim", 0.3f);

                GameManager.GetInstance().DeactivatePlayerController();
            }

            if (Input.GetButtonDown("Melee") && currentMeleeCd < 0f)
            {
                Hit();
                animator.SetBool("isAttacking", true);
                currentMeleeCd = meleeCooldown;
            }
                

        }

        if (Input.GetButtonDown("Reload")) 
            GameManager.GetInstance().Reload();

        //Salto
        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            jumpStart = transform.position.y;
            rb.velocity = Vector2.up * jumpVelocity;

            audioSource.PlayOneShot(jumpSound);

            animator.SetBool("isJumping", true);

        }


        if (Input.GetButtonDown("Helmet") && !isJumping)
        {
            bubbleHelmet.InvokeReplace();
            
        } 
           


        if(isJumping)
        {
            //Añade una fuerza complementaria hacia abajo al llegar al tope de altura y al seguir cayendo
            if (rb.velocity.y > 0 && transform.position.y > jumpStart + jumpHeigth || rb.velocity.y < 0)
                rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;

            //Si cancelamos el salto aplica otra fuerza hacia abajo
            else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
                rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }



    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y); // Movimiento físico del jugador
        RaycastHit2D rcDown = Physics2D.Raycast(transform.position, Vector2.down, raycastLength, stage);

        if (rcDown)
        {
            //Debug.Log("He llegado al suelo");
            isJumping = false;
            bubbleSpawner.enabled = true;
            if(Mathf.Round(rb.velocity.y) == 0)
            {
                animator.SetBool("isJumping", false);
            }
        }
        else
        {
            //Debug.Log("Estoy en el aire");
            isJumping = true;
            bubbleSpawner.enabled = false;
        }

        Debug.DrawRay(transform.position, Vector2.down * raycastLength, Color.cyan);
    }

    void Shoot()
    {
        if (GameManager.GetInstance().CanShoot() && Time.time > timeBulletRate)
        {
            audioSource.PlayOneShot(shootSound);
            animator.SetBool("isShooting", true);
            Invoke("StopShootingAnim", 0.2f);

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

    private void StopShootingAnim()
    {
        animator.SetBool("isShooting", false);
    }
    private void StopCreatingBubbleAnim()
    {
        animator.SetBool("createBubble", false);
    }
    void Hit()
    {
        melee.SetActive(true);

        Invoke("desactiveMelee", 0.2f);
    }
    void desactiveMelee()
    {
        if (melee.activeSelf)
        {
            melee.SetActive(false);
            animator.SetBool("isAttacking", false);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.GetComponent<Door>() && key)
        {
            key = false;
            UIManager.GetInstance().UseKey();
            col.gameObject.GetComponent<Door>().OpenDoor();
        }
        if (col.gameObject.GetComponent<Key>())
        {
            key = true;
            UIManager.GetInstance().GetKey();
            Destroy(col.gameObject);
        }
    }
}
