using UnityEngine;

public class Door : MonoBehaviour
{
    public Sprite openDoor;
    Sprite closeDoor;
    BoxCollider2D bc;
    SpriteRenderer spriteRenderer;
    

    private void Start()
    {
        bc = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        closeDoor = spriteRenderer.sprite;
    }
    public void OpenDoor()
    {
        bc.enabled = false;
        spriteRenderer.sprite = openDoor;
    }

    public void CloseDoor()
    {
        bc.enabled = true;
        spriteRenderer.sprite = closeDoor;
    }
}
