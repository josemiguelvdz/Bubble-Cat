using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Tooltip("Jugador al que sigue la camara"), SerializeField]
    GameObject player = null;


    [Tooltip("Vector de distancia"), SerializeField]
    int distanceLimit = 0;

    [Tooltip("LookAtTarget"), SerializeField]
    bool lookAtTarget = true;

    [Tooltip("Offset"), SerializeField]
    bool takeOffsetFromInitialPos = true;

    [Tooltip("Coordenadas del offset"), SerializeField]
    Vector3 generalOffset;

    Vector3 whereCameraShouldBe;
    bool warningAlreadyShown = false;

    GameObject bubble;
    Transform target;

    float distance;

    bool isFollowing = false;

    void Start() 
    {
        if (player != null)
        {
            //Debug.Log("Player camarita");
            target = player.transform;
        }
        else
            Debug.LogError("No estoy siguiendo al jugador");

        if (takeOffsetFromInitialPos && target != null) generalOffset = transform.position - target.position;
    }


    void Update()
    {
        distance = Mathf.Abs(Vector2.Distance(transform.position, target.position));

        if (distance > distanceLimit)
        {
            isFollowing = true;
        }
        else if (distance < distanceLimit / 2)
        {
            isFollowing = false;
        }

        if (isFollowing && target != null) 
        {
            whereCameraShouldBe = target.position + generalOffset;
            transform.position = Vector3.Lerp(transform.position, whereCameraShouldBe, Time.deltaTime);


            if (lookAtTarget) transform.LookAt(target);
        } 
        else 
        {
            if (!warningAlreadyShown) 
            {
                Debug.Log("Warning: You should specify a target in the simpleCamFollow script.", gameObject);
                warningAlreadyShown = true;
            }
        }
    }

    void LateUpdate()
    {
        // CAMBIAR FIND POR LLAMAR A UN METODO SETBUBBLE(b) QUE HAGA bubble = b;
        bubble = GameObject.Find("Bubble(Clone)");

        if (bubble)
        {
            target = bubble.transform;
        }
        else
        {
            target = player.transform;
        }
    }
}
