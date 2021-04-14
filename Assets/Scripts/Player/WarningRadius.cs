using UnityEngine;

public class WarningRadius : MonoBehaviour
{
    // Start is called before the first frame update

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Bubble"))
        {
            //Debug.Log("Aparece"); // aparece
            col.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Bubble")
        {
            //Debug.Log("Desaparece"); // desaparece
            col.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

}
