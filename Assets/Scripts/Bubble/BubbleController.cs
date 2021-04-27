﻿using UnityEngine;

public class BubbleController : MonoBehaviour
{
    // F = m*a

    [Tooltip("Fuerza de la burbuja"), SerializeField]
    float force = 0;

    [Tooltip("Limite de velocidad para la colisión"), SerializeField]
    float velocityLimit = 0;

    [Tooltip("Capa de colisión de las piezas."), SerializeField]
    int pieceLayer = 17;

    [Tooltip("Capa de colisión del bastón."), SerializeField]
    int staffLayer = 18;

    [Tooltip("Cantidad de rotacion para quitar una pieza de Bastet"), SerializeField]
    float pieceRotation = 10f;

    [Tooltip("Aumento de masa al tirar de un objeto"), SerializeField]
    float pullWeigth = 100f;

    [Tooltip("Distancia para quitar el bastón"), SerializeField]
    float pullDistance = 1f;

    float horizontal; // Input del eje horizontal
    float vertical;
    float delta;
    public float rotationSpeed;

    bool piece = false; //Si es una pieza de Bastet el comportamiento es diferente
    bool grab = false; //Si estamos tirando de algo agarrado
    Vector3 ini;
    float currentRotation = 0f;

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

        if (Input.GetButtonDown("Bubble") ||
            (grab && Mathf.Abs(Vector3.Distance(ini, transform.position)) > pullDistance))
        {
            BubbleManager.GetInstance().DestroyBubble(this.gameObject, spriteRenderer,child);

            if (grab)
                GameManager.GetInstance().NextPhase();
        }
    }

    private void FixedUpdate()
    {
        if(!piece)
            rb.AddForce(movement);

        if(!grab)
        {
            if (piece && (currentRotation > 0 || delta > 0))
            {
                rb.rotation += delta * rotationSpeed;

                currentRotation += delta;

                if (currentRotation >= pieceRotation)
                {
                    BubbleManager.GetInstance().DestroyBubble(this.gameObject, spriteRenderer, child);
                    GameManager.GetInstance().NextPhase();
                }
            }
            else if (!piece)
                rb.rotation += delta * rotationSpeed;
        }
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

        if (col.gameObject.layer == pieceLayer)
        {
            piece = true;
            rb.velocity = Vector2.zero;
            GameManager.GetInstance().GetBastet().SetBubble(this);
        }
        else if (col.gameObject.layer == staffLayer)
        {
            grab = true;
            rb.velocity = Vector2.zero;
            rb.mass *= pullWeigth;
            ini = transform.position;
            GameManager.GetInstance().GetBastet().SetBubble(this);
        }
    }


    private void OnTriggerStay2D(Collider2D col)
    {
        //Si el otro se puede coger con la burbuja y no hay uno dentro, activa su sprite y desactiva el trigger y el otro objeto
        if (col.GetComponent<Bubbleable>() && spriteRenderer.sprite == null)
        {
            BubbleManager.GetInstance().TakeObjects(col.gameObject, spriteRenderer,child, circleCollider2D);
        }
    }

    public void Pop()
    {
        //Me exploto
        BubbleManager.GetInstance().DestroyBubble(this.gameObject, spriteRenderer, child);
    }
}
