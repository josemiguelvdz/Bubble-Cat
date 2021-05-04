using UnityEngine;

public class Water : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<Damageable>())
        {
            Destroy(col.gameObject);
        }
    }
}
