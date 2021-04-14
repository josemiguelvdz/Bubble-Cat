﻿using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    Text barText=null, bulletText=null;

    [Tooltip("Color que adquirirá BulletText al quedarse con poca munición"), SerializeField]
    Color lowAmmunition=Color.black, noBars=Color.black, noAmmo=Color.black;

    Color startingColor;


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
}
