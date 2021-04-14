using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager instance;

    [SerializeField]
    int startingBars=0, startingBullets=0;

    [Tooltip("Las balas deberán ser menor o iguales que este número para permitir la recarga."), SerializeField]
    int reloadLimit=10;

    int bars; //Numero de pastillas que tiene el jugador
    int bullets; //Numero de balas que tiene el jugador

    UIManager uim;
    PlayerController playerController;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);

        bars = startingBars;
        bullets = startingBullets;
    }

    public static GameManager GetInstance()
    {
        //Consigue la referencia al GameManager
        return instance;
    }

    public void SetUIManager(UIManager uimanager)
    {
        uim = uimanager;
    }

    public void CollectBar()
    {
        //Suma una pastilla a nuestras pastillas
        bars++;
        SetUI();
    }

    public void SetBars(int n)
    {
        //Asigna el numero de pastillas a n
        bars = n;
        SetUI();
    }

    public void SetUI()
    {
        uim.UpdateUI(bars, bullets, reloadLimit);
    }



    public void Shoot()
    {
        bullets--;
        SetUI();
    }



    public bool CanShoot()
    {
        if (bullets > 0)
            return true;
        else
            return false;
    }



    public void Reload()
    {
        if (bars > 0 && bullets <= reloadLimit)
        {
            bars--;
            bullets = startingBullets;
            SetUI();
        }    
    }


    public void ActivatePlayerController()
    {
        playerController.enabled = true;
    }
    

    public void DeactivatePlayerController()
    {
        playerController.enabled = false;
    }



    public void SetPlayerController(PlayerController p)
    {
        playerController = p;
    }



    public void BubbleHelmet()
    {
        if(bars > 0)
        {
            bars--;
            SetUI();
            ActivatePlayerController();
        }
    }

    public bool CanReplaceHelmet()
    {
        return bars > 0;
    }

    public float localScaleX()
    {
        return playerController.transform.localScale.x;
    }

    public GameObject GetPlayer()
    {
        return playerController.gameObject;
    }
}
