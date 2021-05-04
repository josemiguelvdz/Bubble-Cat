using UnityEngine;

public class TrashAttack : MonoBehaviour
{
    public GameObject Pillars;

    Rigidbody2D rb;

    private bool afterSpawn = false;
    void Start()
    {
        rb = Pillars.transform.GetChild(0).GetComponent<Rigidbody2D>();
        // ANIMACIÓN BASTET    
        Invoke("TrashSpawn", 1f);

        
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

    }
    public void TrashDespawn()
    {
        Pillars.SetActive(false);
    }
}
