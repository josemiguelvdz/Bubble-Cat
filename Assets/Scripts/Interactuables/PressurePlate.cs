using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public GameObject door;
    Door _door;

    private void Start()
    {
        _door = door.GetComponent<Door>();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        _door.OpenDoor();
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        _door.CloseDoor();
    }
}
