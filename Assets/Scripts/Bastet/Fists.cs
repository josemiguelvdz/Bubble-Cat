using UnityEngine;

public class Fists : MonoBehaviour
{
    public GameObject fist;
    public float start, cooldown;
    int spawn, firstSon, secondSon;

    void Start()
    {
        InvokeRepeating("InvokeFit", start, cooldown);
    }

    void InvokeFit()
    {
        spawn = Random.Range(0, 2);

        if (spawn == 0)
        {
            firstSon++;
        }

        else
        {
            secondSon++;
        }

        if (firstSon == 3)
        {
            spawn = 1;
            secondSon = 1;
            firstSon = 0;
        }

        else if (secondSon == 3)
        {
            spawn = 0;
            firstSon = 1;
            secondSon = 0;
        }

        Debug.Log(spawn);

        Instantiate(fist, transform.GetChild(spawn).position, fist.transform.rotation);
    }
}
