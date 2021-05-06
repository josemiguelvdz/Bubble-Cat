using UnityEngine;

public class SpiderMovement : MonoBehaviour
{
    public float spiderSpeed,escapeSpeed,visionRadius,attackRadius;
    public float timeToAttack, forceX, forceY, refreshAttack, raycastLength;
    public LayerMask stageLayer;


    float distance,izq, der;
    bool attacking,ground, runnningAway;

    GameObject player;
    Rigidbody2D rb;
    Quaternion playerRotation;

    private void Start()
    {
        izq = -transform.localScale.x;
        der = transform.localScale.x;
        playerRotation = transform.rotation;
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
    }




    private void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position, visionRadius, 1 << LayerMask.NameToLayer("Player"));
        Vector3 forward = transform.TransformDirection(player.transform.position - transform.position);
        Debug.DrawRay(transform.position, forward, Color.red);


        if (Physics2D.Raycast(transform.position, Vector2.down, raycastLength, stageLayer))
        {
            Debug.Log("He llegado al suelo");
            ground = true;
            transform.rotation = playerRotation;
        }
        else
        {
            Debug.Log("Estoy en el aire");
            ground = false;
        }


        if (hit.collider!=null && hit.collider.GetComponent<PlayerController>())
        {
            distance = Vector3.Distance(player.transform.position, transform.position);
        }

        
        if (distance < visionRadius && distance > attackRadius && ground)
        {
            rb.velocity = new Vector2(player.transform.position.x - transform.position.x, rb.velocity.y).normalized * spiderSpeed;
            if (!attacking) CancelInvoke();
            else attacking = false;
            runnningAway = false;
        }
        else if (distance < attackRadius && !attacking && ground)
        {
            rb.velocity = Vector3.zero;
            attacking = true;
            Invoke("Attack", timeToAttack);
        }
        else if(distance > visionRadius && ground)
        {
            runnningAway = true;
            rb.velocity = new Vector2(-player.transform.position.x + transform.position.x, rb.velocity.y).normalized * escapeSpeed;
            transform.localScale = new Vector3(-transform.localScale.x,transform.localScale.y,transform.localScale.z);
        }
    }



    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionRadius);
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }

    void Attack()
    {
        if(transform.localScale.x<0) rb.AddForce(new Vector2(-forceX, forceY), ForceMode2D.Impulse);
        else if(transform.localScale.x>0) rb.AddForce(new Vector2(forceX, forceY), ForceMode2D.Impulse);

        Invoke("CanAttack", refreshAttack);
    }

    void CanAttack()
    {
        attacking = false;
    }


}
