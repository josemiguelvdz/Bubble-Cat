using UnityEngine;

public class BatBehaviour : MonoBehaviour
{
    [Tooltip("Velocidad de movimiento del murciélago."), SerializeField]
    float flySpeed = 1f;

    [Tooltip("Aceleración del aleteo del murciélago."), SerializeField]
    float acceleration = 1f;

    [Tooltip("Impulso que recibe el murcielago al golpear el jugador"), SerializeField]
    float bounceForce = 10f;

    [Tooltip("Tiempo que tarda el murciélago en perder al jugador."), SerializeField]
    float waitingTime = 5f;

    [Tooltip("Capa de colisión del jugador."), SerializeField]
    int playerLayer = 8;

    float radius = 10f; //A que distancia lo veo
    bool waiting = false; //Si el murcielago esta esperando a volver a ver al jugador
    bool invokeOnce = false; //Solo hacemos el invoke 1 vez

    Transform player; //Posicion del jugador, perseguir
    Transform bubble; //Posicion de la pompa, huir

    Rigidbody2D rb;

    Animator animator;
    SpriteRenderer sprite;
    bool death;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        death = false;
    }

    void FixedUpdate()
    {
        //Si hay una pompa cerca
        if (animator.GetBool("Death"))
        {
            rb.AddForce(Vector2.down * acceleration);
        }
        else
        {
            
            if (bubble)
            {
                //Huir de ella
                if (rb.velocity.magnitude < flySpeed) //Evitamos acelerar de más
                    rb.AddForce(-(bubble.position - transform.position).normalized * acceleration);
                if ((bubble.position - transform.position).normalized.x < 0)
                    sprite.flipX = true;
                else
                    sprite.flipX = false;
            }
            else if (player != null)
            {
                
                if ((player.position - transform.position).normalized.x < 0)
                    sprite.flipX = false;
                else
                    sprite.flipX = true;
                //Trazamos un rayo hacia el jugador
                Vector2 dir = player.position - transform.position;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, radius);
                Debug.DrawRay(transform.position, dir.normalized * hit.distance, Color.red);

                //Si colisiona con el jugador
                if (hit && hit.collider.gameObject.layer == playerLayer)
                {
                    //Perseguimos al jugador
                    if (rb.velocity.magnitude < flySpeed)
                        rb.AddForce(dir.normalized * acceleration);

                    Debug.Log("Estoy persiguiendo a Yuno");

                    CancelInvoke();
                    invokeOnce = false;
                    waiting = true;
                }
                else if (waiting)
                {
                    if (!invokeOnce)
                    {
                        Invoke("EndWait", waitingTime);

                        invokeOnce = true;
                    }

                    Debug.Log("No veo a Yuno");
                }
                else
                {
                    //Vuelve arriba
                    if (rb.velocity.magnitude < flySpeed)
                        rb.AddForce(Vector2.up * acceleration);

                    Debug.Log("Me he cansao de esperar");
                }
            }

            //Vuelve a rotacion 0
            transform.rotation = Quaternion.Euler(Vector3.zero);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        int stageLayer = LayerMask.NameToLayer("Stage");

        if (collision.gameObject.GetComponent<PlayerController>())
            rb.AddForce((transform.position - player.position).normalized * bounceForce, ForceMode2D.Impulse);
        if (collision.gameObject.layer == stageLayer)
        {
            if (death == true)
                gameObject.SetActive(false);
        }
    }

    public void CollissionData(Transform t, float r, bool isBubble) //true si es pompa, false si es player
    {
        //Me da los datos que necesito saber de warningZone, como la referencia al jugador y su distancia para trazar el rayo
        if (isBubble)
            bubble = t;
        else
            player = t;

        radius = r;

        waiting = true;

        animator.SetBool("Mov", true);
    }

    void EndWait()
    {
        waiting = false;
        invokeOnce = false;
    }

    public void die()
    {
        death = true;
    }
}
