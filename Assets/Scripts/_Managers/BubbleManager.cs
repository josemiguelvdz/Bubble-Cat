using UnityEngine;

public class BubbleManager : MonoBehaviour
{
    static BubbleManager instance;

    public Material bubbleable;
    public Material grate;

    public AudioClip explosionBubble;

    BubbleController currentBubble = null;
    GameObject collissioned;

    private void Awake()
    {
        //Accedemos al material bubbleable y hacemos que aun no brille
        if (bubbleable.HasProperty("Vector1_E336CF1E"))
        {
            bubbleable.SetInt("Vector1_E336CF1E", 0);
            bubbleable.EnableKeyword("Vector1_E336CF1E");
        }

        if (grate.HasProperty("Vector1_E336CF1E"))
        {
            grate.SetInt("Vector1_E336CF1E", 0);
            grate.EnableKeyword("Vector1_E336CF1E");
        }

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else Destroy(this.gameObject);
    }

    //Activa el sprite del hijo de la burbuja y desactiva el objeto temporalmente
    public void TakeObjects(GameObject col, SpriteRenderer bubble, GameObject child) 
    {
        HighlightOff();
        bubble.sprite = col.GetComponent<SpriteRenderer>().sprite;
        child.transform.rotation = col.transform.rotation;
        collissioned = col;
        collissioned.SetActive(false);
    }


    //Destruye la burbuja
    public void DestroyBubble(GameObject col, SpriteRenderer bubble, GameObject child)
    {
        Destroy(col);

        if (bubble.sprite != null) //Si tenía un objeto, lo vuelve a activar en la posición del hijo
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

        AudioSource audiosource = GetComponent<AudioSource>();
        audiosource.PlayOneShot(explosionBubble);

        AllHighlightOff();
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
        HighlightOn();
    }

    public BubbleController GetBubble()
    {
        return currentBubble;
    }

    void HighlightOn()
    {
        //Hacemos que el material bubbleable brille
        if (bubbleable.HasProperty("Vector1_E336CF1E"))
            bubbleable.SetInt("Vector1_E336CF1E", 1);

        if (grate.HasProperty("Vector1_E336CF1E"))
            grate.SetInt("Vector1_E336CF1E", 1);
    }

    void HighlightOff()
    {
        if (bubbleable.HasProperty("Vector1_E336CF1E"))
            bubbleable.SetInt("Vector1_E336CF1E", 0);
    }

    void AllHighlightOff()
    {
        if (bubbleable.HasProperty("Vector1_E336CF1E"))
            bubbleable.SetInt("Vector1_E336CF1E", 0);

        if (grate.HasProperty("Vector1_E336CF1E"))
            grate.SetInt("Vector1_E336CF1E", 0);
    }
}
