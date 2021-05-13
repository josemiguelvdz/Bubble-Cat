using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    static UIManager instance;

    public YunoArrow yunoArrow;

    [SerializeField]
    Text barText=null, bulletText=null;

    [Tooltip("Color que adquirirá BulletText al quedarse con poca munición"), SerializeField]
    Color lowAmmunition=Color.black, noBars=Color.black, noAmmo=Color.black;

    [SerializeField]
    GameObject key;

    Color startingColor;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(this.gameObject);
    }

    public static UIManager GetInstance()
    {
        //Consigue la referencia al UIManager
        return instance;
    }

    void Start()
    {
        startingColor = bulletText.color;

        GameManager.GetInstance().SetUIManager(this);
        GameManager.GetInstance().SetUI();
    }

    public void UpdateUI(int bars, int bullets, int reloadLimit)
    {
        barText.text = "× " + bars;
        bulletText.text = "× " + bullets;
        if (bullets == 0) 
            bulletText.color = noAmmo;
        else if (bullets <= reloadLimit)
            bulletText.color = lowAmmunition;
        else 
            bulletText.color = startingColor;

        if (bars == 0)
            barText.color = noBars;
        else
            barText.color = startingColor;
    }

    

    public void ActivateArrow()
    {
        yunoArrow.gameObject.SetActive(true);
    }

    public void DeactivateArrow()
    {
        yunoArrow.gameObject.SetActive(false);
    }

    public void ClosestPoint(Vector3 point)
    {
        yunoArrow.ClosestPoint(point);
    }

    public void GetKey()
    {
        key.SetActive(true);
    }

    public void UseKey()
    {
        key.SetActive(false);
    }
}
