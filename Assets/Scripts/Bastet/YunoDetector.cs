using UnityEngine;

public class YunoDetector : MonoBehaviour
{
    Bastet parent;

    void Start()
    {
        parent = GetComponentInParent<Bastet>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Bastet aparece cuando detecta a Yuno
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            parent.Appear();
            Destroy(gameObject);
        }
    }
}
