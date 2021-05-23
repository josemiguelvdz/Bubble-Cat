using UnityEngine;

public class Pause : MonoBehaviour
{
    public GameObject pause, confirmation, settings;
    bool isPaused = false;

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                GameManager.GetInstance().DeactivatePlayerController();
                pause.SetActive(true);
                Time.timeScale = 0;
                isPaused = true;
            }
        }
    }

    public void Resume()
    {
        pause.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
        GameManager.GetInstance().ActivatePlayerController();
    }

    public void OpenConfirmation()
    {
        confirmation.SetActive(true);
    }

    public void CloseConfirmation()
    {
        confirmation.SetActive(false);
    }

    public void OpenSetting()
    {
        settings.SetActive(true);
    }

    public void CloseSettings()
    {
        settings.SetActive(false);
    }
}
