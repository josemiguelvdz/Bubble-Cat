using UnityEngine;

public class BubbleHelmet : MonoBehaviour
{
    public GDTFadeEffect fadeEffect;

    public float time;
    public GameObject helmet;
    bool helmetOn = true;
    bool inProgress = false;
    bool inmunity;
    private void ReplaceHelmet()
    {
        helmet.SetActive(true);
        helmetOn = true;
        inProgress = false;
        GameManager.GetInstance().BubbleHelmet();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Damageable>())
        {
            if (helmetOn)
            {
                inProgress = false;
                helmetOn = false;
                helmet.SetActive(false);
                CancelInvoke();
            }
            else
            {
                // PARTICULAS

                fadeEffect.StartEffect();
                Invoke("InvokeRespawn", 1f);

                helmetOn = true;
                helmet.SetActive(true);
            }
                
            
        }
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
            Invoke("ReplaceHelmet", time);
        }
    }
}
