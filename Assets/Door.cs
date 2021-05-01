using UnityEngine;

public class Door : MonoBehaviour
{
    public void OpenDoor()
    {
        BoxCollider2D bc = GetComponent<BoxCollider2D>();
        bc.enabled = false;

        // PARTICULAS
        Destroy(gameObject);
    }
}
