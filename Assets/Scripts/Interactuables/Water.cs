using UnityEngine;

public class Water : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<Damageable>()) // Si se cae un enemigo al agua
        {
            Destroy(col.gameObject);
        }
        else if (col.gameObject.GetComponent<BubbleController>()) // Si la pompa entra al agua, explota
            col.gameObject.GetComponent<BubbleController>().Pop();
    }
 
}
