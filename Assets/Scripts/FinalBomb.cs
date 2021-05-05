using UnityEngine;

public class FinalBomb : MonoBehaviour
{
    public Vector2 impulse;
    public float rotationSpeed = 1f;

    bool returned = false;
    Rigidbody2D rb;
    float angle;

    void OnEnable()
    {
        if (returned)
            rb.AddForce(impulse, ForceMode2D.Impulse);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
                
            Destroy(this.gameObject);
        }
    }
}
