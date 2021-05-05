using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public string scene;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>()) StartGame();
    }
    public void StartGame()
    {
        if (GameObject.Find("GasPipeline"))
        {
            SceneManager.LoadScene(scene); //Cambiar luego por comic
        }
        

        GameState.currentCheckpoint = Checkpoint.checkpoint5;
        
        Time.timeScale = 1;
    }
}
