using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YunoArrow : MonoBehaviour
{
    [Tooltip("Indicador de la posición de Yuno cuando este sale de la pantalla"), SerializeField]
    RectTransform yunoArrow;

    [Tooltip("Capa de colisión del jugador."), SerializeField]
    int playerLayer = 8;

    [Tooltip("Posición de Yuno Detector de la cámara, para lanzar un raycast desde ahí"), SerializeField]
    Transform center;

    [Tooltip("Distancia de la flechita de Yuno hasta el borde de la pantalla"), SerializeField]
    float distance = 2;

    Transform player;

    Vector3 closestPoint;
    Vector3 arrowPosition;

    private void Start()
    {
        player = GameManager.GetInstance().GetPlayer().transform;
    }

    private void Update()
    {
        if (yunoArrow.gameObject.activeSelf)
        {
            Vector2 dir = player.position - center.position;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, Mathf.Infinity);
            Debug.DrawRay(center.position, dir.normalized * hit.distance, Color.green);

            Vector3 perpendicularDir = new Vector3(dir.x, dir.y, 0).normalized;
            if(Mathf.Abs(perpendicularDir.x) > Mathf.Cos(29.40f))
            {
                if (perpendicularDir.x < 0) perpendicularDir = new Vector3(-1, 0, 0);
                else perpendicularDir = new Vector3(1, 0, 0);
            }
            else if(Mathf.Abs(perpendicularDir.x) < Mathf.Cos(29.40f))
            {
                if (perpendicularDir.y < 0) perpendicularDir = new Vector3(0, -1, 0);
                else perpendicularDir = new Vector3(0, 1, 0);

            }


            arrowPosition = closestPoint - perpendicularDir * distance;
            yunoArrow.position = arrowPosition;
        }
    }

    public void ClosestPoint(Vector3 point)
    {
        closestPoint = point;
    }
}
