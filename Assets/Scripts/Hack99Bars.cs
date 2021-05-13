using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hack99Bars : MonoBehaviour
{
    void Start()
    {
        GameManager.GetInstance().SetBars(99); 
    }
}
