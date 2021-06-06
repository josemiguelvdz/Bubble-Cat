using UnityEngine;

public class WarningRadius : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Bubble"))
        {
            //Aparece el aviso, se pone la pompa roja
            col.GetComponent<SpriteRenderer>().material.SetInt("_Lejos", 1);
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Bubble")
        {
            //Desaparece, la pompa vuelve a su color original
            col.GetComponent<SpriteRenderer>().material.SetInt("_Lejos", 0);
        }
    }

}
