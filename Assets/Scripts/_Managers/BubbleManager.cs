using UnityEngine;

public class BubbleManager : MonoBehaviour
{
    static BubbleManager instance;
    GameObject collissioned;

    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else Destroy(this.gameObject);
    }

    public void ActivateTrigger(CircleCollider2D circleCollider2D)
    {
        circleCollider2D.isTrigger = true;
    }


    public void TakeObjects(GameObject col , SpriteRenderer bubble ,GameObject child, CircleCollider2D circleCollider2D)
    {
        bubble.sprite = col.GetComponent<SpriteRenderer>().sprite;
        child.transform.rotation = col.transform.rotation;
        circleCollider2D.isTrigger = false;
        collissioned = col;
        collissioned.SetActive(false);
        if (col.name == "Lizard")
        {
            col.gameObject.GetComponent<Lizard>().StopShooting(false, true);  
        }        
    }


    public void DestroyBubble(GameObject col, SpriteRenderer bubble,GameObject child)
    {
        Destroy(col);

        if (bubble.sprite != null)
        {
            collissioned.transform.position = child.transform.position;
            collissioned.transform.rotation = child.transform.rotation;
            collissioned.SetActive(true);
            if (collissioned.gameObject.name.Contains("Dirt_Projectile"))
            {
                collissioned.gameObject.GetComponent<Dirt_projectile>().rotationProjectil(collissioned.transform.eulerAngles.z);
            }
        }

        GameManager.GetInstance().ActivatePlayerController();
    }


    public static BubbleManager GetInstance()
    {
        //Consigue la referencia al GameManager
        return instance;
    }
}
