using UnityEngine;

public class ExplosionRadius : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<BubbleController>())
        {
            BubbleManager.GetInstance().DestroyBubble(collision.gameObject, collision.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>(),collision.gameObject.transform.GetChild(0).gameObject);
        }
    }
}
