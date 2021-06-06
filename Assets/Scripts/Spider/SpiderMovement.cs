using UnityEngine;

public class SpiderMovement : MonoBehaviour
{
    [Tooltip("Velocidad de la araña"), SerializeField]
    float spiderSpeed;
    [Tooltip("Velocidad de la araña al escapar"), SerializeField]
    float escapeSpeed;
    [Tooltip("Radio de visión"), SerializeField]
    float visionRadius;
    [Tooltip("Radio de ataque"), SerializeField]
    float attackRadius;
    [Tooltip("Tiempo para hacer el ataque"), SerializeField]
    float timeToAttack;
    [Tooltip("Fuerza para el salto en horizontal"), SerializeField]
    float forceX;
    [Tooltip("Fuerza para el salto en vertical"), SerializeField]
    float forceY;
    [Tooltip("Tiempo para volver a atacar de seguido"), SerializeField]
    float refreshAttack;
    [Tooltip("Distancia del RaycastHit2D hacia el suelo"), SerializeField]
    float raycastLength;
    [Tooltip("Distancia del RayCastHit2D hacia el frente"), SerializeField]
    float wallDistance;

    float distance,izq, der;
    bool attacking,ground, runnningAway;
    bool enemy;

    GameObject player;
    Rigidbody2D rb;
    Animator animator;

    Quaternion spiderRotation = new Quaternion(0, 0, 0, 0);
    RaycastHit2D wall;
    public LayerMask stageLayer;

    private void Start()
    {
        animator = GetComponent<Animator>();
        izq = -transform.localScale.x;
        der = transform.localScale.x;
        rb = GetComponent<Rigidbody2D>();
        player = GameManager.GetInstance().GetPlayer();
    }

    private void Update()
    {
        //Si no esta huyendo, la araña mirá donde esté el player
        if (!runnningAway)
        {
            if (player.transform.position.x - transform.position.x < 0 ) transform.localScale = new Vector3(izq, transform.localScale.y, transform.localScale.z);
            else if (player.transform.position.x - transform.position.x > 0 ) transform.localScale = new Vector3(der, transform.localScale.y, transform.localScale.z);
        }
        //Si está en el suelo y huyendo, comprueba que si se choca con una pared se gire
        else if(ground)
        {
            wall = Physics2D.Raycast(transform.position,new Vector2(transform.localScale.x, 0).normalized, wallDistance);
            Debug.DrawRay(transform.position,new Vector2(transform.localScale.x, 0), Color.red);

            if ((wall.collider != null && !wall.collider.gameObject.GetComponent<PlayerController>()) || enemy)
            {
                if (transform.localScale.x <= 0 ) transform.localScale = new Vector3(1, 1, 1);
                else if (transform.localScale.x > 0) transform.localScale = new Vector3(-1, 1, 1);
                enemy = false;
            }
            else rb.velocity = new Vector2(escapeSpeed * transform.localScale.x, 0);
        }

    }




    private void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position, visionRadius, 1 << LayerMask.NameToLayer("Player"));
        Vector3 forward = transform.TransformDirection(player.transform.position - transform.position).normalized;
        Debug.DrawRay(transform.position, forward*visionRadius, Color.red);

        //Si detecta colisión, es que está en el suelo y empieza a moverse
        if (Physics2D.Raycast(transform.position,new Vector2(0,-transform.up.y), raycastLength, stageLayer))
        {
            animator.SetBool("Jump", false);
            ground = true;
            transform.rotation = spiderRotation;
        }
        //Si está en el aire se activa la animación
        else
        {
            animator.SetBool("Move", false);
            ground = false;
        }
        //Calcula la distancia al player
        if (hit.collider!=null && hit.collider.GetComponent<PlayerController>())
        {
            distance = Vector3.Distance(player.transform.position, transform.position);
        }
        //Si la distancia es menor que el radio de ataque y la araña está en el suelo, ataca
        if (distance < attackRadius && !attacking && ground)
        {
            animator.SetBool("Move", false);
            animator.SetBool("Jump", true);
            rb.velocity = Vector3.zero;
            attacking = true;
            Invoke("Attack", timeToAttack);
        }
        //Si la distancia es menor que el radio de visión pero no que el de ataque, y está en el suelo, se mueve hacia el jugador
        else if (distance < visionRadius && distance > attackRadius && ground && !attacking) 
        {
            animator.SetBool("Move", true);
            rb.velocity = new Vector2(player.transform.position.x - transform.position.x, rb.velocity.y).normalized * spiderSpeed;
            if (!attacking) CancelInvoke();
            else attacking = false;
            runnningAway = false;
        }
        //Si la distancia es mayor que el radio de visión, huye
        else if(distance > visionRadius && ground && !attacking)
        {
            animator.SetBool("Move", true);
            if (!runnningAway)
            {
                if (player.transform.position.x < transform.position.x) transform.localScale = new Vector3(der, transform.localScale.y, transform.localScale.z);
                else transform.localScale = new Vector3(izq, transform.localScale.y, transform.localScale.z);
            }
            runnningAway = true;
        }
    }



    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionRadius);
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Coloca a la araña con buena rotación
        transform.rotation = spiderRotation;
        if (collision.gameObject.GetComponent<EnemyHealth>()) 
            enemy = true;
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            rb.AddForce(new Vector2(Random.Range(-3, 3), Random.Range(2, 5)), ForceMode2D.Impulse);
        }
    }

    //Ataca
    void Attack()
    {
        if (transform.localScale.x < 0) rb.AddForce(new Vector2(-forceX, forceY), ForceMode2D.Impulse);
        else if (transform.localScale.x > 0) rb.AddForce(new Vector2(forceX, forceY), ForceMode2D.Impulse);
        Invoke("CanAttack", refreshAttack);
    }


    void CanAttack()
    {
        attacking = false;
    }


}
