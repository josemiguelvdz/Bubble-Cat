using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class FullscreenLogic : MonoBehaviour
{
    [Tooltip("Toggle"), SerializeField]
    private Toggle toggle;

    [Tooltip("Dropdown"), SerializeField]
    private TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;
    // Start is called before the first frame update
    void Start()
    {
        if (Screen.fullScreen) // Comprueba si el juego está o no en pantalla completa.
        {
            toggle.isOn = true;
        }
        else
        {
            toggle.isOn = false;
        }

        CheckResolution();
    }

    public void FullScreenOn(bool fullScreen) // Ocurre al marcar la opción de poner el juego en pantalla completa
    {
        Screen.fullScreen = fullScreen;
    }
    public void CheckResolution()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        int currentResolution = 0;

        for (int i = 0; i < resolutions.Length; i++) // Se añaden a una lista todas las resoluciones disponibles
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (Screen.fullScreen && resolutions[i].width == Screen.currentResolution.width
                && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolution = i;
            }
        }
        resolutionDropdown.AddOptions(options); // Se añaden al dropdown todas las resoluciones que se pueden seleccionar
        resolutionDropdown.value = currentResolution;
        resolutionDropdown.RefreshShownValue();

        resolutionDropdown.value = PlayerPrefs.GetInt("resolutionNumber", 0);
    }
    public void ChangeResolution(int resIndex) // Cambia la resolución por la que selecciones
    {
        PlayerPrefs.SetInt("resolutionNumber", resolutionDropdown.value);

        Resolution resolution = resolutions[resIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
