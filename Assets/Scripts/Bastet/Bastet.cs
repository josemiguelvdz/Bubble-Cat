﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class Bastet : MonoBehaviour
{
    public enum States { fists, shoot, magic, box, trash, bomb, ko, dead };

    States currentState, nextState;

    MonoBehaviour currentComponent;
    Fists fists;
    Shoot shoot;
    Magic magic;
    Box box;
    Trash trash;
    Bomb bomb;
    KO ko;
    Dead dead;
    MonoBehaviour[] components;

    [SerializeField, Tooltip("Vida que tiene cada fase de Bastet. " +
        "Cuando llegue a 0, su próximo ataque será el de lanzar " +
        "la bomba de butano que deberemos devolver para dejarla KO")]
    int health = 50;

    [SerializeField, Tooltip("Tiempo en segundos que tarda Bastet en salir de su estado KO")]
    float koTime = 10;

    [SerializeField, Tooltip("Piezas que quitaremos de Bastet en orden descendente. La 0 sera la última pieza que se quita.")]
    GameObject [] pieces = null;

    int piecesNum = 3; //Número de piezas que tenemos que quitar a Bastet para desmontar su robot
    int currentHealth;

    Transform player;
    BubbleController bubble;

    //Ataques que hay que meter:

    //Fase 1
    //Ataque con puños
    //Lanzar proyectiles de basura o de mugre whatever

    //Lanzar botella de butano

    //Fase 2: Le quitamos los proyectiles de basura
    //Lanza bolas mágicas con el bastón
    //Spawnear cajas desde el "techo" para dificultar el movimiento

    //Lanzar botella de butano

    //Fase 3: Le quitamos los puños
    //Rayos que salen del suelo

    //Lanzar botella de butano
    //Devolver la botella de butano

    //Final epico: Le quitamos el bastón, Bastet cae a la arena y le disparamos jabón fuertemente

    //Finales
    // - Bastet se transforma de vuelta y huye?
    // - Bastet se transforma de vuelta y se hacen amigos? Yuno le regala un cuenco de comida
    // - Tiramos a Bastet al mar para darle un baño?

    public void DesiredState(States newState)
    {
        switch(piecesNum)
        {
            case 3:
                if (newState == States.fists || newState == States.shoot || newState == States.bomb || newState == States.ko)
                    nextState = newState;
                break;
            case 2:
                if(newState == States.fists || newState == States.magic || newState == States.box || newState == States.bomb || newState == States.ko)
                    nextState = newState;
                break;
            case 1:
                if (newState == States.magic || newState == States.box || newState == States.trash || newState == States.bomb || newState == States.ko)
                    nextState = newState;
                break;
            case 0:
                if(newState == States.dead)
                    nextState = newState;
                break;
            default:
                nextState = newState;
                break;
        }
    }

    private void Start()
    {
        piecesNum = pieces.Length;
        currentHealth = health;

        GameManager.GetInstance().SetBastet(this);

        fists = GetComponent<Fists>();
        shoot = GetComponent<Shoot>();
        magic = GetComponent<Magic>();
        box = GetComponent<Box>();
        trash = GetComponent<Trash>();
        bomb = GetComponent<Bomb>();
        ko = GetComponent<KO>();
        dead = GetComponent<Dead>();

        currentState = States.fists;
        components = new MonoBehaviour[8] { fists, shoot, magic, box, trash, bomb, ko, dead };
    }

    private void Update()
    {
        if(currentState != nextState)
        {
            if (currentComponent != null)
                currentComponent.enabled = false;

            components[(int)nextState].enabled = true;
            currentComponent = components[(int)nextState];

            currentState = nextState;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!ko.enabled && collision.gameObject.GetComponent<Damageable>() && !collision.gameObject.GetComponent<EnemyHealth>())
        {
            currentHealth--;
            Debug.Log(currentHealth);
            if (currentHealth <= 0)
                KOEnter();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!ko.enabled && collision.gameObject.GetComponent<Melee>())
        {
            currentHealth--;
            if (currentHealth <= 0)
                KOEnter();
        }
    }

    public void Appear()
    {
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<CapsuleCollider2D>().enabled = true;
    }

    void KOEnter()
    {
        Invoke("KOExit", koTime);

        pieces[piecesNum - 1].SetActive(true);
    }

    void KOExit()
    {
        try
        {
            bubble.Pop();
        }
        catch { }
        currentHealth = health;
        pieces[piecesNum - 1].SetActive(false);
    }

    public void PieceOff()
    {
        CancelInvoke();
        KOExit();
        currentHealth = health;
        piecesNum--;
        Debug.Log(piecesNum);

        if (piecesNum == 0)
            Win();
    }

    public void SetBubble(BubbleController b)
    {
        bubble = b;
    }

    void Win()
    {
        SceneManager.LoadScene("Credits");
    }
}
