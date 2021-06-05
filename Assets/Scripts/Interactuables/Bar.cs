using UnityEngine;

public class Bar : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Si me choco con el jugador
        if(collision.gameObject.GetComponent<PlayerController>())
        {
            //Me destruyo
            Destroy(gameObject);

            //Notifico al GM que se ha cogido una pastilla
            GameManager.GetInstance().CollectBar();
        }
    }
}
