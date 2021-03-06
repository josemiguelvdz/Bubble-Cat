using UnityEngine;

public class Box : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject box;
    public GameObject boxSpawner;
    public int boxNumber;

    public float minTime;
    public float maxTime;

    private BoxCollider2D bc;

    private Vector3 maxPoint;
    private Vector3 minPoint;
    private Transform btrans;

    public float waitingTime;

    private int cont = 0;

    Bastet bastet;

    void OnEnable()
    { 
        // ANIMACION?
        Invoke("BoxSpawn", 1f);
        cont = 0;
    }
    private void Start()
    {
        bc = boxSpawner.GetComponent<BoxCollider2D>();
        btrans = boxSpawner.GetComponent<Transform>();

        maxPoint = bc.bounds.max;
        minPoint = bc.bounds.min;

        bastet = GetComponent<Bastet>();
    }

    public void BoxSpawn()
    {
        float time;


        if (cont < boxNumber)
        {
            cont += 1;


            btrans.position = new Vector3(Mathf.RoundToInt(Random.Range(minPoint.x, maxPoint.x)), maxPoint.y, 0);

            Instantiate(box, btrans.position, Quaternion.Euler(Vector3.zero));

            Rigidbody2D rb = box.GetComponent<Rigidbody2D>();

            Vector2 vector2 = new Vector2(0, rb.velocity.y + 10);

            rb.AddForce(vector2, ForceMode2D.Impulse);

            time = Random.Range(minTime, maxTime);

            //Reinvocamos el método
            Invoke("BoxSpawn", time);

        }
        else
        {
            Invoke("NextAttack", waitingTime);
        }
    }

    void NextAttack()
    {
        bastet.DesiredState(Bastet.States.magic);
        bastet.DesiredState(Bastet.States.fists);
    }

    public void CancelNextAttack()
    {
        CancelInvoke();
    }
}
