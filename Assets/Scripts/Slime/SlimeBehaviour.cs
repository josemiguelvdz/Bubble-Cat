using UnityEngine;

public class SlimeBehaviour : MonoBehaviour
{
    GameObject player;
    Rigidbody2D rb;
    //Variables de los distintos rayos a usar
    RaycastHit2D hitPlayer, hitWall, hitGround;
    Vector3 direction = Vector3.left;
    float distance;
    public float slimeSpeed, wallDistance, force;
    public float visionRadius;
    bool enemy = false;
    Animator animator;
    Quaternion slimeRotation = new Quaternion(0, 0, 0, 0);

    void Start()
    {
        //Componente Animator
        animator = GetComponent<Animator>();
        //Componente transform del jugador
        player = GameManager.GetInstance().GetPlayer();
        rb = GetComponent<Rigidbody2D>();
        distance = 100f;
    }

    private void FixedUpdate()
    {
        //Definimos el vector de direccion hacia el jugador
        Vector3 raySpawn = (player.transform.position - transform.position);

        //Raycast para detectar al player
        hitPlayer = Physics2D.Raycast(transform.position, raySpawn, visionRadius, 1 << LayerMask.NameToLayer("Player"));
        Debug.DrawRay(transform.position, raySpawn.normalized * visionRadius, Color.cyan);

        //Cuando detectemos al player
        if (hitPlayer.collider != null && hitPlayer.collider.GetComponent<PlayerController>())
        {
            //Calculamos la distancia que hay hasta él
            distance = Vector3.Distance(player.transform.position, transform.position);
        }

        //Raycast para detectar cualquier objeto, muro, etc con lo que pueda chocar el slime
        hitWall = Physics2D.Raycast(transform.position, direction, wallDistance, 1 << LayerMask.NameToLayer("Stage"));
        Debug.DrawRay(transform.position, direction.normalized * wallDistance, Color.red);

        //Raycast para detectar el suelo y así no caiga por precipicios
        hitGround = Physics2D.Raycast(transform.position + direction * wallDistance, Vector2.down, wallDistance, 1 << LayerMask.NameToLayer("Stage"));
        Debug.DrawRay(transform.position + direction * wallDistance, Vector2.down.normalized * wallDistance, Color.yellow);

        //Si no detectamos suelo, tampoco pared y no estamos en el aire (velocidad en y es menor que 0.1f)
        if (hitGround.collider == null && hitWall.collider == null && Mathf.Abs(rb.velocity.y) < 0.1f)
        {
            //Cambiamos la direccion y la escala del spite
            direction = -direction;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }

        //Si la distancia es mayor que el radio de vision y colisiona con algo del escenario o con otro enemigo
        if (((distance > visionRadius) && hitWall.collider != null && hitWall.collider.gameObject.layer == 9) || enemy)
        {
            //Cambiamos la direccion y la escala del sprite
            direction = -direction;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            enemy = false;
        }

        //Si no se detecta muro, estamos en el suelo y la velocidad en y es menor que 0.1f
        if (Mathf.Abs(rb.velocity.y) < 0.1f && (distance > visionRadius) || (distance < visionRadius && (hitWall.collider == null && hitGround.collider != null)))
        {
            //Se mueve el slime
            transform.Translate(slimeSpeed * direction * Time.deltaTime);
        }

        //Si la distancia es menor que el radio de vision
        if (distance < visionRadius)
        {
            //Asignamos una nueva direccion, pero solo en el eje x para que vaya en horizontal
            direction = new Vector3(player.transform.position.x - transform.position.x, 0, 0).normalized;

            //Si la posicion en x del jugador es distinta a la del slime (para que si estamos justo debajo no cambie a izq y der todo el rato)
            if (Mathf.Round(player.transform.position.x) != Mathf.Round(transform.position.x))
            {
                //Cambiamos la escala dependiendo de la posicion

                if (player.transform.position.x < transform.position.x)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }

                else
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }
            }
        }

        //Si está en el aire cayendo
        if (hitGround.collider == null)
        {
            //Que caiga con la rotacion inicial
            transform.rotation = slimeRotation;
            transform.Translate(slimeSpeed*Vector2.down*Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Si colisionamos con el player
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            animator.SetBool("Attack", true);
            Invoke("DesactivateAttack", 0.5f);
            //Aplicamos un pequeño impulso hacia atrás a la babosa
            rb.AddForce(-direction * force, ForceMode2D.Impulse);
        }
        
        //Si colisonamos con un enemigo activamos el boleano para cambiar en el fixedupdate la direccion y escala
        if (collision.gameObject.GetComponent<EnemyHealth>()) enemy = true;
        
        //Si colisionamos con una tuberia
        if (collision.gameObject.GetComponent<Pipeline>())
        {
            //Cambiamos la direccion y escala del sprite
            direction = -direction;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionRadius);
    }

    private void DesactivateAttack()
    {
        animator.SetBool("Attack", false);
    }
}
