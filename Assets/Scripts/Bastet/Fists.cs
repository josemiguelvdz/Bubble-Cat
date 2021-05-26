using UnityEngine;

public class Fists : MonoBehaviour
{
    public GameObject rightFist, leftFist;
    public float start, cooldown;
    public float timeDestroyer;

    [Tooltip("Probabilidad de que el siguiente ataque sean los rayos mágicos si estamos en la segunda fase"), Range(0, 100f), SerializeField]
    float prob = 70f;

    int which, numAttacks;
    Bastet bastet;
    Animator rightFistAnim, leftFistAnim;

    public AudioClip fistSound;
    AudioSource audioSource;

    public void ShowArms()
    {
        rightFist.SetActive(true);
        leftFist.SetActive(true);
    }

    void OnEnable()
    {
        numAttacks = Random.Range(2, 6);
        InvokeRepeating("InvokeFist", start, cooldown);

        bastet = GetComponent<Bastet>();
    }

    private void Start()
    {
        rightFistAnim = rightFist.GetComponent<Animator>();
        leftFistAnim = leftFist.GetComponent<Animator>();

        audioSource = GetComponent<AudioSource>();
    }

    void InvokeFist()
    {
        which = Random.Range(0, 2);

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
        audioSource.PlayOneShot(fistSound);

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
        rightFistAnim.SetBool("PuñetazoAbajo", false);
        leftFistAnim.SetBool("PuñetazoArriba", false);
        CancelInvoke();
    }
}
