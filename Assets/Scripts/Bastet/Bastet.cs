using UnityEngine;

public class Bastet : MonoBehaviour
{
    [SerializeField, Tooltip("Vida que tiene cada fase de Bastet. " +
        "Cuando llegue a 0, su próximo ataque será el de lanzar " +
        "la bomba de butano que deberemos devolver para dejarla KO")]
    int health;

    [SerializeField, Tooltip("Tiempo en segundos que tarda Bastet en salir de su estado KO")]
    float koTime = 10; 



    int pieces = 3; //Número de piezas que tenemos que quitar a Bastet para desmontar su robot
    bool ko = false;

    Transform player;
    CapsuleCollider2D col;

    //Ataques que hay que meter (se acumulan):

    //Fase 1
    //Ataque con puños
    //Lanzar proyectiles de basura o de mugre whatever

    //Lanzar botella de butano

    //Fase 2
    //Lanzar rayos láser

    //Lanzar botella de butano

    //Fase 3
    //Spawnear cajas desde el "techo" para dificultar el movimiento
    //Rayos láser que salen del suelo

    //Lanzar botella de butano

    //Final epico

    private void Start()
    {
        col = GetComponent<CapsuleCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Damageable>() && !collision.gameObject.GetComponent<EnemyHealth>())
        {
            health--;

            if (health >= 0)
                KOEnter();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Melee>())
        {
            health--;

            if (health >= 0)
                KOEnter();
        }
    }

    public void Appear()
    {
        GetComponent<SpriteRenderer>().enabled = true;
    }

    void KOEnter()
    {
        col.enabled = false;
        ko = true;
        Invoke("KOExit", koTime);
    }

    void KOExit()
    {

    }
}
