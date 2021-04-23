using UnityEngine;

public class SlimeBehaviour : MonoBehaviour
{
    public int force;

    Rigidbody2D rb;
    public Vector2 direction = new Vector2();
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        direction = Vector2.left;
    }

    private void FixedUpdate()
    {
        rb.AddForce(direction*force);
    }
}
