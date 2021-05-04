using UnityEngine;

public class Fists : MonoBehaviour
{
    public GameObject fist;
    public float start, cooldown;
    public float timeDestroyer;

    [Tooltip("Probabilidad de que el siguiente ataque sean los rayos mágicos si estamos en la segunda fase"), Range(0, 100f), SerializeField]
    float prob = 70f;

    int spawn, firstSon, secondSon, numAttacks;
    Bastet bastet;

    void OnEnable()
    {
        numAttacks = Random.Range(2, 6);
        InvokeRepeating("InvokeFit", start, cooldown);

        bastet = GetComponent<Bastet>();
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

        GameObject fistInstance = Instantiate(fist, transform.GetChild(spawn).position, fist.transform.rotation);
        Destroy(fistInstance, timeDestroyer);

        numAttacks--;

        if (numAttacks == 0)
        {
            CancelInvoke();

            if (Random.Range(0, 101) < prob)
                bastet.DesiredState(Bastet.States.magic);
            else
                bastet.DesiredState(Bastet.States.box);

            bastet.DesiredState(Bastet.States.shoot);
        }
    }

    private void OnDisable()
    {
        CancelInvoke();
    }
}
