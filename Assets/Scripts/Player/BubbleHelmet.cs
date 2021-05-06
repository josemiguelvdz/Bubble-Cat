using UnityEngine;

public class BubbleHelmet : MonoBehaviour
{
    public GDTFadeEffect fadeEffect;

    public float replaceTime, inmunityTime;
    public SpriteRenderer helmet;
    public bool invencible = false;
    bool helmetOn = true;
    bool inProgress = false;

    bool inmunity = false;
    SpriteRenderer yuno;
    BubbleController bubble;

    private void Start()
    {
        yuno = GetComponent<SpriteRenderer>();
    }

    private void ReplaceHelmet()
    {
        helmet.enabled = true;
        helmetOn = true;
        inProgress = false;
        GameManager.GetInstance().BubbleHelmet();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!inmunity && collision.gameObject.GetComponent<Damageable>())
            MakeDamage();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!inmunity && (collision.GetComponent<Gas>() || collision.GetComponent<PillarMovement>() || collision.tag == "Fists") || collision.GetComponent<Water>())
            MakeDamage();
    }

    public void MakeDamage()
    {
        if(!invencible)
        {
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
            else
            {
                // PARTICULAS

                fadeEffect.StartEffect();
                Invoke("InvokeRespawn", 1f);

                helmetOn = true;
                helmet.enabled = true;
            }

            bubble = BubbleManager.GetInstance().GetBubble();
            if (bubble) bubble.Pop();
        }
    }

    void StopInmunity()
    {
        //Debug.Log("Fuera inmunidad");
        inmunity = false;
        yuno.color = Color.white;
    }

    public void InvokeRespawn()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Sewer");
    }

    public void InvokeReplace()
    {
        if(!helmetOn && !inProgress && GameManager.GetInstance().CanReplaceHelmet())
        {
            GameManager.GetInstance().DeactivatePlayerController();
            inProgress = true;
            Invoke("ReplaceHelmet", replaceTime);
        }
    }
}
