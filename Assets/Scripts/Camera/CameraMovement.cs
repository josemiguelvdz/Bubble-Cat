using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Tooltip("Jugador al que sigue la camara"), SerializeField]
    private Transform player = null;

    [Tooltip("Coordenadas del offset"), SerializeField]
    private Vector3 generalOffset;

    private Vector3 whereCameraShouldBe;
    private bool warningAlreadyShown = false;


    private Transform target; // Target al que va a seguir la cámara


    void Start() // Pone al player como target al principio de cada partida
    {
        if (player != null)
        {
            target = player;
        }
        else
            Debug.LogError("No estoy siguiendo al jugador");
    }


    void Update()
    {

        if (target != null)  // La cámara sigue al jugador o a la pompa
        {
            whereCameraShouldBe = target.position + generalOffset;
            transform.position = Vector3.Lerp(transform.position, whereCameraShouldBe, Time.deltaTime);

        } 
        else 
        {
            if (!warningAlreadyShown) 
            {
                Debug.LogError("No se ha encontrado un target", gameObject);
                warningAlreadyShown = true;
            }
        }
    }

    void LateUpdate()
    {
        BubbleController bubble = BubbleManager.GetInstance().GetBubble();

        if (bubble) // Si hay burbuja, el target pasa a ser la burbuja
        {
            target = bubble.transform;
        }
        else
        {
            target = player;
        }
    }
}
