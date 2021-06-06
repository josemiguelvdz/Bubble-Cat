using UnityEngine;

public class Door : MonoBehaviour
{
    [Tooltip("Sprite de la puerta"), SerializeField]
    private Sprite openDoor;

    private Sprite closeDoor;
    private BoxCollider2D bc;
    private SpriteRenderer spriteRenderer;
    

    private void Start()
    {
        bc = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        closeDoor = spriteRenderer.sprite;
    }

    // Abre la puerta, quita el collider y cambia el sprite, además de invocar a la función que se encarga de cambiar el checkpoint
    public void OpenDoor() 
    {
        bc.enabled = false;
        spriteRenderer.sprite = openDoor;

        ChangeCheckpoint();
    }

    //Cierra la puerta y activa el collider y el sprite
    public void CloseDoor()
    {
        bc.enabled = true;
        spriteRenderer.sprite = closeDoor;
    }

    public void ChangeCheckpoint()
    {
        if (gameObject.name == "Door_1") // Dependiendo de la puerta, cambias a un checkpoint u otro.
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

    }
}
