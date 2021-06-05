using UnityEngine;

public class Shooter : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float coolingDownSeconds, shootCadenceSecs;
    public bool autoShoot;

    bool disparoPosible = true;

    void Start()
    {
        //parent = GetComponentInParent<Transform>();

        if (autoShoot)
            InvokeRepeating("Shoot", shootCadenceSecs, shootCadenceSecs);
    }

    //Dispara bala en la direccion dada
    public void Shoot()
    {
        if (disparoPosible)
        {
            Instantiate(bulletPrefab, transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
        }
    }
}
