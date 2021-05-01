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
    Bastet bastet;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(this.gameObject);

        bars = startingBars;
        bullets = startingBullets;
    }

    private void Start()
    {
        Debug.Log("SE EJECUTO EL START");
        switch (GameState.currentCheckpoint)
        {
            case Checkpoint.spawnpoint:
                GameObject.Find("Player").transform.position = GameObject.Find("spawnpoint").transform.position;
                GameObject.Find("Camera").transform.position = GameObject.Find("spawnpoint").transform.position + new Vector3(5, 0, -10);
                break;
            case Checkpoint.checkpoint1: // Delete Key1, Door1

                Destroy(GameObject.Find("Key_1"));
                Destroy(GameObject.Find("Door_1"));

                GameObject.Find("Player").transform.position = GameObject.Find("checkpoint1").transform.position;

                GameObject.Find("Camera").transform.position = GameObject.Find("checkpoint1").transform.position + new Vector3(5,0, -10);
                break;

            case Checkpoint.checkpoint2: // Delete Key1, Door1

                Destroy(GameObject.Find("Key_2"));
                Destroy(GameObject.Find("Door_2"));

                GameObject.Find("Player").transform.position = GameObject.Find("checkpoint2").transform.position;

                GameObject.Find("Camera").transform.position = GameObject.Find("checkpoint2").transform.position + new Vector3(5, 0, -10);
                break;

        }

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

    public void SetBastet(Bastet b)
    {
        bastet = b;
    }

    public Bastet GetBastet()
    {
        return bastet;
    }

    public void NextPhase()
    {
        //Pasa a la siguiente fase de Bastet
        bastet.PieceOff();
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
