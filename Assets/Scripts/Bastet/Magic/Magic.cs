using UnityEngine;

public class Magic : MonoBehaviour
{
    public Transform spawner;
    public GameObject magicRay;
    public float rateTime;
    public float cooldownTime;
    private GameObject player; //Posicion del jugador
    Vector3 directionMagicRay;
    private int numMagicRays;
    private Bastet bastet;

    [Tooltip("Probabilidad de que el siguiente ataque sean la caida de cajas si estamos en la segunda fase"), Range(0, 100f), SerializeField]
    int prob = 30;

    private void OnEnable()
    {
        InvokeRepeating("InvokeRay", rateTime, cooldownTime);
        numMagicRays = Random.Range(2, 6);
    }

    void Start()
    {
        player = GameManager.GetInstance().GetPlayer();
        bastet = GetComponent<Bastet>();
    }

    void InvokeRay()
    {        
        setDirection(player.transform.position - spawner.position);
        float angle = Vector3.Angle(getDirection(), transform.forward); //Angulo que indica la direccion del rayo para rotar el transform del rayo magico
        Instantiate(magicRay, spawner.position, Quaternion.Euler(new Vector3(0, 0, 180)));

        numMagicRays--;

        if (numMagicRays == 0)
        {
            CancelInvoke();

            if (Random.Range(0, 101) < prob)
                bastet.DesiredState(Bastet.States.box);
            else
            {
                bastet.DesiredState(Bastet.States.fists);
                bastet.DesiredState(Bastet.States.trash);
            }
        }
    }

    public void setDirection(Vector3 vector)
    {
        directionMagicRay = vector;
    }

    public Vector3 getDirection()
    {
        return directionMagicRay;
    }

    private void OnDisable()
    {
        CancelInvoke();
    }
}
