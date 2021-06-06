using UnityEngine;

public class Fists : MonoBehaviour
{
    [Tooltip("Puños"), SerializeField]
    private GameObject rightFist, leftFist;
    [Tooltip("Tiempo que pasa hasta que empieza a pegar puñetazos"), SerializeField]
    private float start;
    [Tooltip("Tiempo que pasa sin hacer nada hasta que vuelve a atacar"), SerializeField]
    private float cooldown;

    [Tooltip("Probabilidad de que el siguiente ataque sean los rayos mágicos si estamos en la segunda fase"), Range(0, 100f), SerializeField]
    private float prob = 70f;

    private int which, numAttacks;
    private Bastet bastet;
    private Animator rightFistAnim, leftFistAnim;

    public void ShowArms() // Activa los 2 brazos
    {
        rightFist.SetActive(true);
        leftFist.SetActive(true);
    }

    void OnEnable()
    {
        numAttacks = Random.Range(2, 6); // Se decide el número de puñetazos
        InvokeRepeating("InvokeFist", start, cooldown); // Ataca

        bastet = GetComponent<Bastet>(); // Acceder a la máquina de estados
    }

    private void Start()
    {
        rightFistAnim = rightFist.GetComponent<Animator>();
        leftFistAnim = leftFist.GetComponent<Animator>();
    }

    void InvokeFist()
    {
        which = Random.Range(0, 2); // Decide si el puñetazo irá arriba o abajo

        if (which == 0)
        {
            //Puño abajo
            rightFistAnim.SetBool("PuñetazoAbajo", true);
        }

        else
        {
            //Puño arriba
            leftFistAnim.SetBool("PuñetazoArriba", true);
        }

        numAttacks--;

        if (numAttacks == 0) // Cuando se queda sin puñetazos, decide el siguiente ataque de Bastet (máquina de estados)
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
        rightFistAnim.SetBool("PuñetazoAbajo", false);
        leftFistAnim.SetBool("PuñetazoArriba", false);
        CancelInvoke();
    }
}
