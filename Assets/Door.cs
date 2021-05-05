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

        ChangeCheckpoint();
    }

    public void CloseDoor()
    {
        bc.enabled = true;
        spriteRenderer.sprite = closeDoor;
    }

    public void ChangeCheckpoint()
    {
        if (gameObject.name == "Door_1")
        {
            GameState.currentCheckpoint = Checkpoint.checkpoint1;
        }
        else if (gameObject.name == "Door_2")
        {
            GameState.currentCheckpoint = Checkpoint.checkpoint2;
        }
        else if (gameObject.name == "Door_3")
        {
            GameState.currentCheckpoint = Checkpoint.checkpoint3;
        }
        else if (gameObject.name == "Door_4")
        {
            GameState.currentCheckpoint = Checkpoint.checkpoint4;
            Debug.Log(GameState.currentCheckpoint);
        }

    }
}
