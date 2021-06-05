using UnityEngine;

public class KO : MonoBehaviour
{
    [SerializeField, Tooltip("Tiempo en segundos que tarda Bastet en salir de su estado KO")]
    float koTime = 15;

    Bastet bastet;

    private void Awake()
    {
        bastet = GetComponent<Bastet>();
    }

    private void OnEnable()
    {
        Invoke("KOExit", koTime);

        bastet.PieceAppear();
    }

    private void OnDisable()
    {
        CancelInvoke();

        if (bastet.GetBubble() != null)
        {
            //Si hay pompa, la explota y repite la fase
            bastet.GetBubble().Pop();
        }

        //Cuando salimos del estado KO, Bastet recupera toda su vida y desaparece la pieza correspondiente
        bastet.RestoreHealth();
        bastet.PieceDisappear();
    }

    void KOExit()
    {
        enabled = false;
    }
}
