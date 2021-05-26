using UnityEngine;

public class Butano : MonoBehaviour
{
    public Vector2 velocityLimit;
    public GameObject explosion;
    public AudioClip explosionSound;
    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.name != "Bullet(Clone)")
        {
            if (Mathf.Abs(col.relativeVelocity.x) > velocityLimit.x)
            {
                Explode();
                if (col.gameObject.GetComponent<BrokenWall>()) Destroy(col.gameObject);
            }
            if (Mathf.Abs(col.relativeVelocity.y) > velocityLimit.y)
            {
                Explode();
                if (col.gameObject.GetComponent<BrokenWall>()) Destroy(col.gameObject);
            }
        }
    }

    void Explode()
    {
        explosion.SetActive(true);
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        // PUM particulas y tal
        Destroy(this.gameObject, 1);
        audioSource.PlayOneShot(explosionSound);
    }
}
