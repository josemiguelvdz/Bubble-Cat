using UnityEngine;
using UnityEngine.SceneManagement;

public class BubbleHelmet : MonoBehaviour
{
    public GDTFadeEffect fadeEffect;

    public GameObject dead;
    float timeParticle = 1f;

    public float replaceTime, inmunityTime;
    public SpriteRenderer helmet;
    public bool invencible = false;
    bool helmetOn = true;
    bool inProgress = false;

    bool inmunity = false;
    SpriteRenderer yuno;
    BubbleController bubble;

    string sceneName;

    Rigidbody2D rb;
    Animator animator;


    private void Start()
    {
        yuno = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        Scene scene = SceneManager.GetActiveScene();
        sceneName = scene.name;
    }


    private void ReplaceHelmet()
    {
        helmetOn = true;
        inProgress = false;
        animator.SetBool("bubbleHelmet", false);

        //Llamamos al gameManager para activar de nuevo el playerController
        GameManager.GetInstance().BubbleHelmet();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!inmunity && collision.gameObject.GetComponent<Damageable>())
            MakeDamage();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!inmunity && collision.gameObject.GetComponent<Damageable>())
            MakeDamage();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!inmunity && (collision.GetComponent<Gas>() || collision.GetComponent<PillarMovement>() || collision.tag == "Fists"))
            MakeDamage();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!inmunity && collision.GetComponent<Water>())
            MakeDamage();
    }

    public void MakeDamage()
    {
        if(!invencible)
        {
            bubble = BubbleManager.GetInstance().GetBubble();
            if (bubble) bubble.Pop();

            //Si tenemos casco
            if (helmetOn)
            {
                inProgress = false;
                helmetOn = false;
                helmet.enabled = false;
                CancelInvoke();
                inmunity = true;
                yuno.color = new Color(1f, 1f, 1f, .5f);
                Invoke("StopInmunity", inmunityTime);
            }

            //Si no tenemos casco
            else
            {
                //Activamos el periodo de inmunidad
                inmunity = true;
                yuno.color = new Color(1f, 1f, 1f, .5f);

                //Lo desactivamos
                Invoke("StopInmunity", inmunityTime);


                PlayerController pc = gameObject.transform.GetComponentInParent<PlayerController>();
                pc.enabled = false;
                Rigidbody2D rb = gameObject.transform.GetComponentInParent<Rigidbody2D>();
                rb.velocity = Vector2.zero;

                // PARTICULAS
                dead.SetActive(true);
                Invoke("OffParticles", timeParticle);

                fadeEffect.StartEffect();

                //Hacemos respawn
                Invoke("InvokeRespawn", 1f);

                //Volvemos a activar el casco pompa
                helmetOn = true;
                helmet.enabled = true;
            }
        }
    }

    void StopInmunity()
    {
        inmunity = false;
        yuno.color = Color.white;
    }

    public void InvokeRespawn()
    {
        //Cargamos de nuevo la escena en el ultimo checkpoint
        SceneManager.LoadScene(sceneName);
    }

    void OffParticles()
    {
        dead.SetActive(false);
    }

    public void InvokeReplace()
    {
        //Si no tenemos casco,    y tenemos las suficentes pastillas de jabon
        if(!helmetOn && !inProgress && GameManager.GetInstance().CanReplaceHelmet())
        {
            //Desactivamos el PlayerController
            GameManager.GetInstance().DeactivatePlayerController();
            rb.velocity = Vector2.zero;
            inProgress = true;
            animator.SetBool("bubbleHelmet", true);

            helmet.enabled = true;

            Invoke("ReplaceHelmet", replaceTime);
        }
    }
}
