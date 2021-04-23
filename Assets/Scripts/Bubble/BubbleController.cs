using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleController : MonoBehaviour
{
    // F = m*a

    [Tooltip("Fuerza de la burbuja"), SerializeField]
    float force = 0;

    float horizontal; // Input del eje horizontal
    float vertical;
    float delta;
    public float rotationSpeed;

    Rigidbody2D rb;
    GameObject child;
    SpriteRenderer spriteRenderer;
    CircleCollider2D circleCollider2D;

    Vector2 movement;
    Vector3 rotation;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Componente RigidBody2D del jugador

        if (rb == null)
            Debug.LogError("No he encontrado el Rigidbody2D de la pompa.");

        child = transform.GetChild(0).gameObject;
        spriteRenderer = child.GetComponent<SpriteRenderer>();
        circleCollider2D = GetComponent<CircleCollider2D>();
    }
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal"); //Recogida de input
        vertical = Input.GetAxisRaw("Vertical"); //Recogida de input
        delta = Input.GetAxis("Rotate");

        movement = new Vector2(horizontal * force, vertical * force);

        if (Input.GetButtonDown("Bubble"))
        {
            BubbleManager.GetInstance().DestroyBubble(this.gameObject, spriteRenderer,child);
        }
    }

    private void FixedUpdate()
    {
        rb.AddForce(movement);
        rb.rotation += delta * rotationSpeed;
    }



    private void OnCollisionEnter2D(Collision2D col)
    {
        int stageLayer = LayerMask.NameToLayer("Stage");
        int playerLayer = LayerMask.NameToLayer("Player");
        //Si el objeto colisionado tiene la layer de un objeto interactuable activa el trigger
        if (col.gameObject.GetComponent<Bubbleable>() && spriteRenderer.sprite == null)
            BubbleManager.GetInstance().ActivateTrigger(circleCollider2D);
        else if (col.gameObject.GetComponent<Bubbleable>() && spriteRenderer.sprite)
            BubbleManager.GetInstance().DestroyBubble(this.gameObject, spriteRenderer, child);
        //Añadir layer de todo lo que destruya la burbuja
        else if (col.gameObject.layer == stageLayer || col.gameObject.layer == playerLayer)
            BubbleManager.GetInstance().DestroyBubble(this.gameObject, spriteRenderer, child);
        else if (col.gameObject.GetComponent<Lizard>())
            col.gameObject.GetComponent<Lizard>().StopShooting();
    }


    private void OnTriggerStay2D(Collider2D col)
    {
        //Si el otro se puede coger con la burbuja y no hay uno dentro, activa su sprite y desactiva el trigger y el otro objeto
        if (col.GetComponent<Bubbleable>() && spriteRenderer.sprite == null)
        {
            BubbleManager.GetInstance().TakeObjects(col.gameObject, spriteRenderer,child, circleCollider2D);
        }
    }
}
