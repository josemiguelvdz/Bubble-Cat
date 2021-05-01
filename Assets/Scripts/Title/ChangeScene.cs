using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public string scene;

    public void StartGame()
    {
        SceneManager.LoadScene(scene); //Cambiar luego por comic
        Time.timeScale = 1;
    }
}
