using UnityEngine;
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

    [SerializeField, Tooltip("Cañon 1")]
    GameObject cannon1 = null;
    [SerializeField, Tooltip("Cañon 2")]
    GameObject cannon2 = null;
    [SerializeField, Tooltip("Protector de aluminio de los cañones")]
    GameObject cannonProtector = null;
    [SerializeField, Tooltip("Protector de aluminio de los cañones (final)")]
    GameObject cannonProtector2 = null;

    [SerializeField, Tooltip("Puño izquierdo")]
    GameObject leftArm = null;
    [SerializeField, Tooltip("Puño derecho")]
    GameObject rightArm = null;

    [SerializeField, Tooltip("Prefab de la pastilla de jabón")]
    GameObject bar;

    [SerializeField, Tooltip("Spawn Point de las pastillas de jabón")]
    Transform barSpawn;

    [SerializeField, Tooltip("Fuerza con la que salen las pastillas de jabón")]
    Vector2 barForce;

    [SerializeField, Tooltip("Tiempo que tarda la nueva fase de Bastet en empezar")]
    float waitingTime = 2f;

    [SerializeField, Tooltip("Música que empezará a sonar cuando Bastet aparezca")]
    AudioSource music;

    public GameObject arm, fakePiece1, fakePiece2;

    public Material piece1Material, piece2Matierial;
    Material staffMaterial;

    int piecesNum = 3; //Número de piezas que tenemos que quitar a Bastet para desmontar su robot
    int currentHealth;

    BubbleController bubble;

    Animator anim;

    private Random rnd;

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
        //Bastet funciona gracias a una máquina de estados
        //Este método se encargará de manejar a qué estados puede pasar en qué momento

        if(currentState != States.start && newState == States.start)
        {
            //Inicialización
            piecesNum = pieces.Length;
            RestoreHealth();
            currentState = States.start;
        }
        else
            switch (piecesNum)
            {
                case 3:
                    //Fase 1
                    if (newState == States.fists || newState == States.shoot || 
                        newState == States.bomb || newState == States.ko)
                        nextState = newState;
                    break;
                case 2:
                    //Fase 2
                    if (newState == States.fists || newState == States.magic || 
                        newState == States.box || newState == States.bomb || newState == States.ko)
                        nextState = newState;
                    break;
                case 1:
                    //Fase 3
                    if (newState == States.magic || newState == States.box || 
                        newState == States.trash || newState == States.bomb || newState == States.ko)
                        nextState = newState;
                    break;
                case 0:
                    //Fin
                    nextState = States.dead;
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

        staffMaterial = pieces[0].GetComponent<SpriteRenderer>().material;

        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        //Si el estado ha cambiado
        if(currentState != nextState)
        {
            if (currentComponent != null)
                currentComponent.enabled = false;

            components[(int)nextState].enabled = true;
            currentComponent = components[(int)nextState];

            currentState = nextState;

            //Debug.Log(currentState);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (currentState != States.ko && currentState != States.dead && 
            collision.gameObject.GetComponent<Damageable>() && !collision.gameObject.GetComponent<EnemyHealth>())
            TakeDamage(1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (currentState != States.ko && currentState != States.dead && collision.gameObject.GetComponent<Melee>())
            TakeDamage(1);
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        //Debug.Log(currentHealth);
        if (currentHealth <= 0)
        {
            box.CancelNextAttack();
            CancelInvoke();
            DesiredState(States.ko);
        }
    }

    public void Appear()
    {
        //Empieza la animación de aparición desde el agua
        anim.SetTrigger("Appear");

        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<CapsuleCollider2D>().enabled = true;
        fists.ShowArms();
    }

    public void AppearFinish()
    {
        //Termina la animación de aparición
        rightArm.GetComponent<SpriteRenderer>().sortingLayerName = "Puño";

        //Cambiazo
        fakePiece1.SetActive(false);
        fakePiece2.SetActive(false);
        pieces[2].SetActive(true);
        pieces[1].SetActive(true);

        //Empieza a sonar la musica
        music.Play();

        FirstAttack();
    }

    public void PieceAppear()
    {
        //Activa una pieza de Bastet para que podamos quitarsela del robot con la pompa
        pieces[piecesNum - 1].GetComponent<Collider2D>().enabled = true;

        if (piecesNum == 1)
        {
            pieces[0].GetComponent<SpriteRenderer>().material = piece1Material;
            piece1Material.SetInt("_Activo", 1);
        }
        else if (piecesNum == 2)
        {
            piece1Material.SetInt("_Activo", 1);
            piece2Matierial.SetInt("_Activo", 1);
        }
        else if (piecesNum == 3)
            piece1Material.SetInt("_Activo", 1);
    }

    public void PieceDisappear()
    {
        //Desactiva la pieza
        if (piecesNum == 1)
        {
            piece1Material.SetInt("_Activo", 0);
            pieces[0].GetComponent<SpriteRenderer>().material = staffMaterial;
        }
        else if (piecesNum == 2)
        {
            piece2Matierial.SetInt("_Activo", 0);
            piece1Material.SetInt("_Activo", 0);
        }
            
        else if (piecesNum == 3)
            piece1Material.SetInt("_Activo", 0);

        if (piecesNum > 0)
        {
            pieces[piecesNum - 1].GetComponent<Collider2D>().enabled = false;

            ChangeAttack();
        }
    }

    public void PieceOff()
    {
        //Hemos conseguido quitarle la pieza, por lo que pasamos a la siguiente fase
        if (piecesNum > 1)
        {
            //Spawneamos una pastilla de jabón como recompensa
            Rigidbody2D barInstance = Instantiate(bar, barSpawn.position, Quaternion.identity).GetComponent<Rigidbody2D>();
            barInstance.bodyType = RigidbodyType2D.Dynamic;
            barInstance.AddForce(barForce, ForceMode2D.Impulse);
            barInstance.AddTorque(.1f, ForceMode2D.Impulse);

            if (piecesNum - 1 == 2) // Si la que cae es la primera pieza
            {
                //El cañón salta por los aires
                pieces[0].SetActive(true);
                arm.SetActive(true);

                Rigidbody2D rb_cannon1 = cannon1.GetComponent<Rigidbody2D>();
                cannon1.GetComponent<Animator>().enabled = false;

                rb_cannon1.bodyType = RigidbodyType2D.Dynamic;
                rb_cannon1.mass = 4;
                rb_cannon1.AddForce(new Vector2(Random.Range(-10, 10), Random.Range(5, 10)), ForceMode2D.Impulse);

                Rigidbody2D rb_cannon2 = cannon2.GetComponent<Rigidbody2D>();
                cannon2.GetComponent<Animator>().enabled = false;

                rb_cannon2.bodyType = RigidbodyType2D.Dynamic;
                rb_cannon2.mass = 4;
                rb_cannon2.AddForce(new Vector2(Random.Range(-10, 10), Random.Range(5, 10)), ForceMode2D.Impulse);

                Rigidbody2D rb_cannonProtector = cannonProtector.GetComponent<Rigidbody2D>(); // 30 grados de Z
                cannonProtector.GetComponent<Animator>().enabled = false;
                rb_cannonProtector.bodyType = RigidbodyType2D.Dynamic;
                rb_cannonProtector.AddTorque(-30);

                cannonProtector2.SetActive(true);
            }
            else if (piecesNum - 1 == 1) //Si es la segunda pieza
            {
                //Los brazos saltan por los aires
                leftArm.GetComponent<Animator>().enabled = false;

                Rigidbody2D rb_leftArm = leftArm.GetComponent<Rigidbody2D>();

                rb_leftArm.bodyType = RigidbodyType2D.Dynamic;
                rb_leftArm.AddTorque(30);
                rb_leftArm.AddForce(new Vector2(Random.Range(5, 20), Random.Range(2, 20)), ForceMode2D.Impulse);

                rightArm.GetComponent<Animator>().enabled = false;

                Rigidbody2D rb_rightArm = rightArm.GetComponent<Rigidbody2D>();

                rb_rightArm.bodyType = RigidbodyType2D.Dynamic;
                rb_rightArm.AddTorque(-30);
                rb_rightArm.AddForce(new Vector2(Random.Range(5, 20), Random.Range(-5, 10)), ForceMode2D.Impulse);

            }
        }

        //La pieza desaparece
        PieceDisappear();
        pieces[piecesNum - 1].SetActive(false);
        piecesNum--;
        //Debug.Log(piecesNum);
    }

    public void RestoreHealth()
    {
        currentHealth = health;
    }

    public void FirstAttack()
    {
        Invoke("ChangeAttack", waitingTime);
    }

    void ChangeAttack()
    {
        //Ataque inicial por defecto de cada fase

        switch (piecesNum)
        {
            case 3:
                DesiredState(States.fists);
                break;
            case 2:
                DesiredState(States.box);
                break;
            case 1:
                DesiredState(States.magic);
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
        TakeDamage(bombDamage);
    }
}
