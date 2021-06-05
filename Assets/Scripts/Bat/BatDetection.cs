using UnityEngine;

public class BatDetection : MonoBehaviour
{
    BatBehaviour parent;
    CircleCollider2D col;

    private void Start()
    {
        parent = GetComponentInParent<BatBehaviour>();
        col = GetComponent<CircleCollider2D>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //Manda al murciélago la referencia del jugador o pompa que estén cerca
        if (collision.gameObject.GetComponent<PlayerController>())
            parent.CollissionData(collision.gameObject.transform, col.radius, false);
        else if(collision.gameObject.GetComponent<BubbleController>())
            parent.CollissionData(collision.gameObject.transform, col.radius, true);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<BubbleController>())
            parent.CollissionData(null, col.radius, true);
    }
}
