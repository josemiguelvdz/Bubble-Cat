using UnityEngine;

public class Magic : MonoBehaviour
{
    public Transform spawner;
    public GameObject magicRay;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("InvokeRay", 3, 5);
    }

    // Update is called once per frame
    void Update()
    {
        Instantiate(magicRay, spawner.position, Quaternion.Euler(new Vector3(0, 0, 180)));
    }
}
