using UnityEngine;

public class DetectPlayer : MonoBehaviour
{
    [Tooltip("Distancia del RayCastHit2D"), SerializeField]
    public float raycastDistance;

    Rigidbody2D rb;
    Collider2D cc;
    Animator animator;

    Vector3 spider;
    RaycastHit2D  hit;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<Collider2D>();
        spider = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Si se ha movido a la araña con la burbuja, cae
        if (transform.position != spider) rb.gravityScale = 1;

        //Si pasa Yuno por debajo, cae
        hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - cc.bounds.size.y / 2), Vector2.down, raycastDistance);
        if (hit.collider != null)
        {
            //Debug.Log(hit.collider.name);
            if (hit.transform.GetComponent<PlayerController>()) rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision) 
    {
        //Si colisiona con el suelo se activa el movimiento
        if (collision.gameObject.layer == LayerMask.NameToLayer("Stage") || collision.gameObject.GetComponent<PlayerController>())
        {
            animator.SetBool("Cuelga", false);
            GetComponent<SpiderMovement>().enabled = true;
            Destroy(this);
        }
    }
}
