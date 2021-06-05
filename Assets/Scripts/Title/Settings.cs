using UnityEngine;

public class Settings : MonoBehaviour
{
    [SerializeField]
    GameObject settingsCanvas;

    public void OpenSettings()
    {
        settingsCanvas.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsCanvas.SetActive(false);
    }
}
