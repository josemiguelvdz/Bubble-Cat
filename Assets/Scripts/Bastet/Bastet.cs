﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class Bastet : MonoBehaviour
{
    public enum States { fists, shoot, magic, box, trash, bomb, ko, dead, start };

    States currentState, nextState;

    MonoBehaviour currentComponent;
    Fists fists;
    Shoot shoot;
    Magic magic;
    Box box;
    TrashAttack trash;
    Bomb bomb;
    KO ko;
    Dead dead;
    MonoBehaviour[] components;

    [SerializeField, Tooltip("Vida que tiene cada fase de Bastet.")]
    int health = 50;

    [SerializeField, Tooltip("Vida que le quita la bomba de butano a Bastet.")]
    int bombDamage = 20;

    [SerializeField, Tooltip("Piezas que quitaremos de Bastet en orden descendente. La 0 sera la última pieza que se quita.")]
    GameObject [] pieces = null;

    [SerializeField, Tooltip("Prefab de la pastilla de jabón")]
    GameObject bar;

    [SerializeField, Tooltip("Spawn Point de las pastillas de jabón")]
    Transform barSpawn;

    [SerializeField, Tooltip("Fuerza con la que salen las pastillas de jabón")]
    Vector2 barForce;

    int piecesNum = 3; //Número de piezas que tenemos que quitar a Bastet para desmontar su robot
    int currentHealth;

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
        if(newState == States.start)
        {
            piecesNum = pieces.Length;
            RestoreHealth();
            currentState = States.start;
        }
        else
            switch (piecesNum)
            {
                case 3:
                    if (newState == States.fists || newState == States.shoot || 
                        newState == States.bomb || newState == States.ko)
                        nextState = newState;
                    break;
                case 2:
                    if (newState == States.fists || newState == States.magic || 
                        newState == States.box || newState == States.bomb || newState == States.ko)
                        nextState = newState;
                    break;
                case 1:
                    if (newState == States.magic || newState == States.box || 
                        newState == States.trash || newState == States.bomb || newState == States.ko)
                        nextState = newState;
                    break;
                case 0:
                    if (newState == States.dead)
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
        RestoreHealth();

        GameManager.GetInstance().SetBastet(this);

        fists = GetComponent<Fists>();
        shoot = GetComponent<Shoot>();
        magic = GetComponent<Magic>();
        box = GetComponent<Box>();
        trash = GetComponent<TrashAttack>();
        bomb = GetComponent<Bomb>();
        ko = GetComponent<KO>();
        dead = GetComponent<Dead>();

        currentState = nextState = States.start;
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

            Debug.Log(currentState);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (currentState != States.ko && currentState != States.dead && 
            collision.gameObject.GetComponent<Damageable>() && !collision.gameObject.GetComponent<EnemyHealth>())
            TakeDamage();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (currentState != States.ko && currentState != States.dead && collision.gameObject.GetComponent<Melee>())
            TakeDamage();
    }

    void TakeDamage()
    {
        currentHealth--;
        if (currentHealth <= 0)
            DesiredState(States.ko);
    }

    public void Appear()
    {
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<CapsuleCollider2D>().enabled = true;
        
        Invoke("FirstAttack", 2);
    }

    public void PieceAppear()
    {
        //Activa una pieza de Bastet para que podamos quitarsela del robot con la pompa
        pieces[piecesNum - 1].SetActive(true);
    }

    public void PieceDisappear()
    {
        if(piecesNum > 0)
            pieces[piecesNum - 1].SetActive(false);
    }

    public void PieceOff()
    {
        PieceDisappear();
        piecesNum--;
        Debug.Log(piecesNum);

        Rigidbody2D barInstance = Instantiate(bar, barSpawn.position, Quaternion.identity).GetComponent<Rigidbody2D>();
        barInstance.bodyType = RigidbodyType2D.Dynamic;
        barInstance.AddForce(barForce);
    }

    public void RestoreHealth()
    {
        currentHealth = health;
    }

    public void FirstAttack()
    {
        switch (piecesNum)
        {
            case 3:
                DesiredState(States.fists);
                break;
            case 2:
                DesiredState(States.box);
                break;
            case 1:
                DesiredState(States.trash);
                break;
            case 0:
                DesiredState(States.dead);
                break;
            default:
                DesiredState(States.fists);
                break;
        }
    }

    public void SetBubble(BubbleController b)
    {
        bubble = b;
    }

    public BubbleController GetBubble()
    {
        return bubble;
    }

    public void BombDamage()
    {
        currentHealth -= bombDamage;
    }
}
