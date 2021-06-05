using UnityEngine;

public class MenuMovement : MonoBehaviour
{
    float mousePosX, mousePosY;
    public float movement=20f;
    RectTransform rT;

    void Start()
    {
        rT = GetComponent<RectTransform>();
    }

    void Update()
    {
        mousePosX = Input.mousePosition.x;
        mousePosY = Input.mousePosition.y;

        rT.position = new Vector2((mousePosX / Screen.width) * movement + (Screen.width / 2),
            (mousePosY / Screen.height) * movement + (Screen.height / 2));
    }
}
