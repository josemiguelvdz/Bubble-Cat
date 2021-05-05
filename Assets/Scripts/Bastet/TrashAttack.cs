using UnityEngine;

public class TrashAttack : MonoBehaviour
{
    public GameObject Pillars;

    Vector3 initialPosition;
    Rigidbody2D rb;
    Bastet bastet;

    private bool afterSpawn = false;

    void Start()
    {
        rb = Pillars.transform.GetChild(0).GetComponent<Rigidbody2D>();
        initialPosition = Pillars.transform.position;
        // ANIMACIÓN BASTET    
        Invoke("TrashSpawn", 1f);

        bastet = GetComponent<Bastet>();
    }

    private void OnEnable()
    {
        if(afterSpawn)
            Pillars.transform.position = initialPosition;
    }

    private void Update()
    {
        if (rb.velocity == Vector2.zero && afterSpawn)
        {
            Invoke("TrashDespawn", 2f);
        }
    }
    public void TrashSpawn()
    {
        Pillars.SetActive(true);
        afterSpawn = true;
        Invoke("NextAttack", 3f);
    }
    public void TrashDespawn()
    {
        Pillars.SetActive(false);
    }

    void NextAttack()
    {
        bastet.DesiredState(Bastet.States.bomb);
    }
}
