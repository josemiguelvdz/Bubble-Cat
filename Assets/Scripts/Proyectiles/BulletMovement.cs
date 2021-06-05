using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    private Rigidbody2D bulletRB;
    public float speedBullet;
    public float timeBullet;

    void Awake()
    {
        bulletRB = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        if(GameManager.GetInstance().localScaleX() > 0)
            bulletRB.velocity = new Vector2(speedBullet, 0);
        else
        {
            gameObject.transform.localScale = new Vector3
                (-gameObject.transform.localScale.x, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
            bulletRB.velocity = new Vector2(-speedBullet, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, timeBullet);
    }

    void OnTriggerEnter2D(Collider2D colision)
    {
        if (colision.gameObject.CompareTag("Enemy")) Destroy(colision.gameObject);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag!="Player") Destroy(this.gameObject);
    }
}
