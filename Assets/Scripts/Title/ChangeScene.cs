using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public string scene;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>()) NextScene();
    }

    public void NextScene()
    {
        if (GameObject.Find("GasPipeline"))
        {
            SceneManager.LoadScene(scene); //Cambiar luego por comic
        }
        

        GameState.currentCheckpoint = Checkpoint.checkpoint5;
        
        Time.timeScale = 1;
    }
    
    public void Load()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(scene);
    }
}
