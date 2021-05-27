using UnityEngine;

public class SpiderMovement : MonoBehaviour
{
    public float spiderSpeed,escapeSpeed,visionRadius,attackRadius;
    public float timeToAttack, forceX, forceY, refreshAttack, raycastLength,wallDistance;
    public LayerMask stageLayer;


    float distance,izq, der;
    bool attacking,ground, runnningAway;
    bool enemy;

    GameObject player;
    Rigidbody2D rb;
    Quaternion spiderRotation=new Quaternion(0,0,0,0);
    RaycastHit2D wall;
    Animator animator;

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
        if (!runnningAway)
        {
            if (player.transform.position.x - transform.position.x < 0 ) transform.localScale = new Vector3(izq, transform.localScale.y, transform.localScale.z);
            else if (player.transform.position.x - transform.position.x > 0 ) transform.localScale = new Vector3(der, transform.localScale.y, transform.localScale.z);
        }
        else if(ground)
        {
            wall = Physics2D.Raycast(transform.position,new Vector2(transform.localScale.x, 0).normalized, wallDistance);
            Debug.DrawRay(transform.position,new Vector2(transform.localScale.x, 0), Color.red);

            if(wall.collider!=null)Debug.Log(wall.collider.name);

            if ((wall.collider != null && !wall.collider.gameObject.GetComponent<PlayerController>()) || enemy)
            {
                if (transform.localScale.x <= 0 )
                {
                    transform.localScale = new Vector3(1, 1, 1);
                    Debug.Log("Derecha");
                }
                else if (transform.localScale.x > 0)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                    Debug.Log("Izquierda");
                }
                enemy = false;
            }
            else
            {
                rb.velocity = new Vector2(escapeSpeed * transform.localScale.x, 0);
            }
        }

    }




    private void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position, visionRadius, 1 << LayerMask.NameToLayer("Player"));
        Vector3 forward = transform.TransformDirection(player.transform.position - transform.position).normalized;
        Debug.DrawRay(transform.position, forward*visionRadius, Color.red);

        if (Physics2D.Raycast(transform.position,new Vector2(0,-transform.up.y), raycastLength, stageLayer))
        {
            animator.SetBool("Jump", false);
            Debug.Log("He llegado al suelo");
            ground = true;
            transform.rotation = spiderRotation;
            Debug.DrawRay(transform.position, Vector3.down * raycastLength, Color.red);
        }
        else
        {
            animator.SetBool("Move", false);
            Debug.Log("Estoy en el aire");
            ground = false;
        }


        if (hit.collider!=null && hit.collider.GetComponent<PlayerController>())
        {
            distance = Vector3.Distance(player.transform.position, transform.position);
        }

        if (distance < attackRadius && !attacking && ground)
        {
            animator.SetBool("Move", false);
            animator.SetBool("Jump", true);
            rb.velocity = Vector3.zero;
            attacking = true;
            Invoke("Attack", timeToAttack);
        }
        else if (distance < visionRadius && distance > attackRadius && ground && !attacking) 
        {
            animator.SetBool("Move", true);
            rb.velocity = new Vector2(player.transform.position.x - transform.position.x, rb.velocity.y).normalized * spiderSpeed;
            if (!attacking) CancelInvoke();
            else attacking = false;
            runnningAway = false;
        }

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
         transform.rotation = spiderRotation;
        if (collision.gameObject.GetComponent<EnemyHealth>()) 
            enemy = true;
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            rb.AddForce(new Vector2(Random.Range(-3, 3), Random.Range(2, 5)), ForceMode2D.Impulse);
        }
        Debug.Log("He colisonado con la araña");
    }

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
