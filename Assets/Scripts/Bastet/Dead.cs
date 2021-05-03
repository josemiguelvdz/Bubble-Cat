using UnityEngine;
using UnityEngine.SceneManagement;

public class Dead : MonoBehaviour
{
    void OnEnable()
    {
        SceneManager.LoadScene("Credits");
    }
}
