using UnityEngine;

public class BubbleHelmet : MonoBehaviour
{
    
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
        if (collision.gameObject.GetComponent<EnemyHealth>())
        {
            if (helmetOn)
            {
                inProgress = false;
                helmetOn = false;
                helmet.SetActive(false);
                CancelInvoke();
            }
            else
                Debug.Log("Has muerto");
        }
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
