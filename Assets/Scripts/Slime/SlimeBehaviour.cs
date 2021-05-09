using UnityEngine;

public class SlimeBehaviour : MonoBehaviour
{
    GameObject player;
    Rigidbody2D rb;
    RaycastHit2D hitPlayer, hitWall, hitGround;
    Vector3 direction = Vector3.left;
    float distance;
    public float slimeSpeed, wallDistance,force;
    public float visionRadius;

    void Start()
    {
        player = GameManager.GetInstance().GetPlayer();
        rb = GetComponent<Rigidbody2D>();
        distance = 100f;
    }

    private void FixedUpdate()
    {
        Vector3 raySpawn = (player.transform.position - transform.position);

        hitPlayer = Physics2D.Raycast(transform.position, raySpawn, visionRadius, 1 << LayerMask.NameToLayer("Player"));
        Debug.DrawRay(transform.position, raySpawn.normalized * visionRadius, Color.cyan);

        if (hitPlayer.collider != null && hitPlayer.collider.GetComponent<PlayerController>())
        {
            distance = Vector3.Distance(player.transform.position, transform.position);
        }

        hitWall = Physics2D.Raycast(transform.position, direction, wallDistance, 1 << LayerMask.NameToLayer("Stage"));
        Debug.DrawRay(transform.position, direction.normalized*wallDistance, Color.red);

        hitGround = Physics2D.Raycast(transform.position, Vector2.down, wallDistance, 1 << LayerMask.NameToLayer("Stage"));
        Debug.DrawRay(transform.position, Vector2.down.normalized*wallDistance, Color.yellow);

        if (hitGround.collider == null)
        {
            direction = -direction;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }

        if ((distance > visionRadius) && hitWall.collider != null && hitWall.collider.gameObject.layer == 9)
        {
            Debug.Log("Muro encontrado");
            direction = -direction;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }

        if ((distance > visionRadius) || (distance < visionRadius && (hitWall.collider == null && hitGround.collider != null)))
        {
            transform.Translate(slimeSpeed * direction * Time.deltaTime);
        }

        if (distance < visionRadius)
        {
            Debug.Log("jugador encontrado");

            direction = new Vector3(player.transform.position.x - transform.position.x, 0, 0).normalized;

            if (player.transform.position.x < transform.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }

            else
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            rb.AddForce(-direction*force, ForceMode2D.Impulse);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionRadius);
    }
}
