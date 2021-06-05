using UnityEngine;

public class YunoVisible : MonoBehaviour
{
    GameObject yuno;
    bool isVisible = false;
    Collider2D col;

    void Start()
    {
        yuno = GameManager.GetInstance().GetPlayer();

        col = GetComponent<Collider2D>();
    }

    private void Update()
    {
        //Manda a YunoArrow el punto más cercano a Yuno del borde de la pantalla
        if (isVisible)
        {
            UIManager.GetInstance().ClosestPoint(col.bounds.ClosestPoint(yuno.transform.position));
        }
    }

    //Activa y desactiva el indicador
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            UIManager.GetInstance().DeactivateArrow();
            isVisible = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            UIManager.GetInstance().ActivateArrow();
            isVisible = true;
        }
    }
}
