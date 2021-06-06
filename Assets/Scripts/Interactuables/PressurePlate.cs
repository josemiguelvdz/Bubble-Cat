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

    //Si colocamos un objeto encima de la placa, abre la puerta correspondiente
    private void OnCollisionStay2D(Collision2D collision)
    {
        _door.OpenDoor();
    }

    //Si quitamos el objeto, se vuelve a cerrar la puerta
    private void OnCollisionExit2D(Collision2D collision)
    {
        _door.CloseDoor();
    }
}
