using UnityEngine;

public class BubbleManager : MonoBehaviour
{
    static BubbleManager instance;

    BubbleController currentBubble = null;
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


    public void TakeObjects(GameObject col , SpriteRenderer bubble ,GameObject child)
    {
        bubble.sprite = col.GetComponent<SpriteRenderer>().sprite;
        child.transform.rotation = col.transform.rotation;
        collissioned = col;
        collissioned.SetActive(false);   
    }


    public void DestroyBubble(GameObject col, SpriteRenderer bubble,GameObject child)
    {
        Destroy(col);

        if (bubble.sprite != null)
        {
            collissioned.transform.position = child.transform.position;
            collissioned.transform.rotation = child.transform.rotation;
            collissioned.SetActive(true);
            if (collissioned.gameObject.name.Contains("Dirt_Projectile") || collissioned.gameObject.name.Contains("Magic_ray"))
            {
                if (collissioned.gameObject.name.Contains("Dirt_Projectile"))
                    collissioned.gameObject.GetComponent<Dirt_projectile>().rotationProjectil(collissioned.transform.eulerAngles.z);
                else
                    collissioned.gameObject.GetComponent<MagicRay>().rotationProjectil(collissioned.transform.eulerAngles.z);
            }
            if (collissioned.gameObject.name.Contains("Lizard"))
            {
                collissioned.gameObject.GetComponent<Lizard>().isDestructible();
            }
        }

        GameManager.GetInstance().ActivatePlayerController();
    }


    public static BubbleManager GetInstance()
    {
        //Consigue la referencia al BubbleManager
        return instance;
    }

    public void SetBubble(BubbleController b)
    {
        currentBubble = b;
    }

    public BubbleController GetBubble()
    {
        return currentBubble;
    }
}
