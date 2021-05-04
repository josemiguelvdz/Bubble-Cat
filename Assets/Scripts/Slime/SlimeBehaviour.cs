using UnityEngine;

public class SlimeBehaviour : MonoBehaviour
{
    GameObject player;
    RaycastHit2D hitPlayer, hitWall;
    Vector3 direction = Vector3.left;
    float distance;
    public float slimeSpeed, coolDown, wallDistance;
    public float attackRadius, visionRadius;

    void Start()
    {
        player = GameManager.GetInstance().GetPlayer();
        distance = 100f;
    }

    private void FixedUpdate()
    {
        hitPlayer = Physics2D.Raycast(transform.position, player.transform.position - transform.position, visionRadius, 1 << LayerMask.NameToLayer("Player"));
        Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.cyan);

        if (hitPlayer.collider != null && hitPlayer.collider.GetComponent<PlayerController>())
        {
            distance = Vector3.Distance(player.transform.position, transform.position);
        }

        hitWall = Physics2D.Raycast(transform.position + direction, direction, wallDistance, 1 << LayerMask.NameToLayer("Stage"));
        Debug.DrawRay(transform.position + direction, direction, Color.red);

        if ((distance > visionRadius) && hitWall.collider != null && (hitWall.collider.CompareTag("Ground") || hitWall.collider.GetComponent<Bubbleable>()))
        {
            Debug.Log("Muro encontrado");
            direction = -direction;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }

        if ((distance > visionRadius) || (distance > attackRadius && distance < visionRadius && hitWall.collider == null))
        {
            transform.Translate(slimeSpeed * direction * Time.deltaTime);
            Debug.Log("Me muevo por la cara");
        }

        if (distance < visionRadius && distance > attackRadius)
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionRadius);
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
