using UnityEngine;

public class FinalBomb : MonoBehaviour
{
    public Vector2 impulse;
    public float rotationSpeed = 1f;
    public GameObject explosion;

    bool returned = false;
    Rigidbody2D rb;
    float angle;

    public AudioClip explosionSound;
    AudioSource audioSource;

    void OnEnable()
    {
        if (returned)
            rb.AddForce(impulse, ForceMode2D.Impulse);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        rb.MoveRotation(angle);

        angle += rotationSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        returned = collision.gameObject.GetComponent<BubbleController>();

        if(!returned)
        {
            GameManager.GetInstance().GetBastet().FirstAttack();

            if (collision.gameObject.GetComponent<Bastet>())
                collision.gameObject.GetComponent<Bastet>().BombDamage();
            else
                //Explotar
                GameManager.GetInstance().GetPlayer().GetComponent<BubbleHelmet>().MakeDamage();
                
            audioSource.PlayOneShot(explosionSound);
            Destroy(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        GameObject ex = Instantiate(explosion, transform.position, Quaternion.identity);
        ex.transform.localScale *= 2.5f;
        Destroy(ex, 1);
    }
}
